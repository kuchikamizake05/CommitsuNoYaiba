# Sync guide

Account: [kuchikamizake05](https://leetcode.com/u/kuchikamizake05/).

Run from the repository root with Node.js 24:

```bash
node scripts/leetcode-sync.mjs
```

The script fetches up to 20 recent public Accepted submissions, merges them by submission ID into `accepted.json`, and generates [the activity table](README.md). Repeated runs do not duplicate records. Older captured records remain even when they disappear from the recent list. Existing history is preserved when the API request fails or returns invalid data.

This is an unofficial integration using LeetCode's website GraphQL endpoint. Access or response fields may change or be blocked. No password, session cookie, API key, browser extension, or solution-code access is required. An empty response is not proof of zero lifetime submissions. This cannot backfill a full account history.

Sync is on demand, not automatic after every submission. After solving, run the script locally, review changes, and commit/push the two generated files on main. Your manual understanding tracker is never edited by the script. Do not mark a task Retained just because it appears here.

Tests (no network calls):

```bash
node --test --experimental-test-coverage scripts/leetcode-sync.test.mjs
```
