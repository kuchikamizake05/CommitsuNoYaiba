import test from 'node:test';
import assert from 'node:assert/strict';
import { normalize, merge, render, fetchRecent } from './leetcode-sync.mjs';

const row = { id: '123', title: 'Two Sum', titleSlug: 'two-sum', timestamp: '1700000000' };
test('validates and stores only public metadata', () => {
  assert.equal(normalize({ ...row, code: 'private' }).id, '123');
  assert.equal(normalize({ ...row, code: 'private' }).code, undefined);
  for (const bad of [{ ...row, titleSlug: '../secret' }, { ...row, timestamp: 'bad' }, { ...row, id: 'x' }]) {
    assert.throws(() => normalize(bad));
  }
});
test('merges idempotently and preserves older records with an empty response', () => {
  const old = [normalize(row)];
  assert.deepEqual(merge(old, [row, row]), old);
  assert.deepEqual(merge(old, []), old);
  assert.equal(merge(old, [{ ...row, id: '124' }]).length, 2);
});
test('renders safely without claiming mastery', () => {
  const output = render('student', [normalize({ ...row, title: 'Two | Sum\n<img>' })]);
  assert.match(output, /not evidence of independent mastery/);
  assert.doesNotMatch(output, /<img>/);
  assert.match(render('student', []), /No public Accepted submissions/);
});
test('fetch sends fixed endpoint and username without credentials', async () => {
  const rows = await fetchRecent('student', async (url, options) => {
    assert.equal(url, 'https://leetcode.com/graphql/');
    assert.equal(JSON.parse(options.body).variables.username, 'student');
    assert.equal(options.headers.Cookie, undefined);
    return { ok: true, json: async () => ({ data: { matchedUser: { username: 'student' }, recentAcSubmissionList: [row] } }) };
  });
  assert.equal(rows.length, 1);
});
test('rejects invalid username, HTTP and GraphQL errors and missing profile', async () => {
  await assert.rejects(fetchRecent('../bad'));
  await assert.rejects(fetchRecent('student', async () => ({ ok: false, status: 429 })));
  for (const body of [{ errors: [{ message: 'error' }] }, { data: { matchedUser: null } }, { data: { matchedUser: {}, recentAcSubmissionList: null } }]) {
    await assert.rejects(fetchRecent('student', async () => ({ ok: true, json: async () => body })));
  }
});
