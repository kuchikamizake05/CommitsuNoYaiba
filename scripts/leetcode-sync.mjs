import { readFile, writeFile, mkdir } from 'node:fs/promises';
import { fileURLToPath, pathToFileURL } from 'node:url';
import path from 'node:path';

const username = 'kuchikamizake05';
const query = 'query ($username: String!, $limit: Int!) { matchedUser(username: $username) { username } recentAcSubmissionList(username: $username, limit: $limit) { id title titleSlug timestamp } }';

export function normalize(row) {
  if (!row || !/^\d+$/.test(row.id) || !/^[a-z0-9]+(?:-[a-z0-9]+)*$/.test(row.titleSlug) ||
      typeof row.title !== 'string' || row.title.length > 300 || !/^\d+$/.test(row.timestamp) ||
      !Number.isSafeInteger(Number(row.timestamp)) || Number(row.timestamp) > 253402300799) {
    throw new Error('Invalid submission metadata; existing history will not be replaced.');
  }
  return { id: String(row.id), title: row.title, titleSlug: row.titleSlug, timestamp: String(row.timestamp) };
}

export function merge(previous, incoming) {
  if (!Array.isArray(previous) || !Array.isArray(incoming)) throw new Error('Invalid history');
  const records = new Map();
  for (const row of [...previous, ...incoming]) {
    const clean = normalize(row);
    records.set(clean.id, clean);
  }
  return [...records.values()].sort((a, b) => Number(b.timestamp) - Number(a.timestamp) || a.id.localeCompare(b.id));
}

const escape = (text) => text.replace(/[\r\n]/g, ' ').replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/\|/g, '&#124;').replace(/\[/g, '&#91;').replace(/\]/g, '&#93;').replace(/`/g, '&#96;').replace(/\\/g, '&#92;');
export function render(user, records) {
  const header = `# LeetCode Accepted activity\n\nProfile: https://leetcode.com/u/${user}/\n\nAccepted is not evidence of independent mastery. Help, explanations, and cold retries remain in [the manual tracker](../tracker.md).\n\nThis archive accumulates the latest 20 public Accepted submissions seen at each sync. It is not a complete historical export and contains no solution code. Dates below are UTC.\n\n`;
  if (!records.length) return header + 'No public Accepted submissions were returned at the last successful sync. This does not prove zero past activity.\n';
  return header + '| Problem | Accepted at (UTC) | Submission ID |\n| --- | --- | --- |\n' + records.map((row) => `| [${escape(row.title)}](https://leetcode.com/problems/${row.titleSlug}/) | ${new Date(Number(row.timestamp) * 1000).toISOString()} | ${row.id} |`).join('\n') + '\n';
}

export async function fetchRecent(user, request = fetch) {
  if (!/^[a-zA-Z0-9_-]{1,50}$/.test(user)) throw new Error('Invalid username');
  const response = await request('https://leetcode.com/graphql/', {
    method: 'POST', redirect: 'error', signal: AbortSignal.timeout(20000),
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ query, variables: { username: user, limit: 20 } }),
  });
  if (!response.ok) throw new Error(`LeetCode HTTP ${response.status}; history unchanged.`);
  const body = await response.json();
  if (body.errors?.length || !body.data?.matchedUser || !Array.isArray(body.data.recentAcSubmissionList)) {
    throw new Error('LeetCode returned errors or no accessible profile; history unchanged.');
  }
  return body.data.recentAcSubmissionList.map(normalize);
}

export async function sync(root, request = fetch) {
  const historyPath = path.join(root, 'leetcode', 'accepted.json');
  let previous = [];
  try {
    const stored = JSON.parse(await readFile(historyPath, 'utf8'));
    if (stored.username !== username) throw new Error('History belongs to another profile');
    previous = stored.submissions;
  } catch (error) {
    if (error.code !== 'ENOENT') throw error;
  }
  const records = merge(previous, await fetchRecent(username, request));
  await mkdir(path.dirname(historyPath), { recursive: true });
  await writeFile(historyPath, JSON.stringify({ username, submissions: records }, null, 2) + '\n');
  await writeFile(path.join(root, 'leetcode', 'README.md'), render(username, records));
  console.log(`Synced public Accepted activity: ${records.length} archived submissions. Manual tracker unchanged.`);
}

if (process.argv[1] && import.meta.url === pathToFileURL(process.argv[1]).href) {
  sync(fileURLToPath(new URL('../', import.meta.url))).catch((error) => {
    console.error(error.message);
    process.exitCode = 1;
  });
}
