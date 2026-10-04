# Backend fundamentals

Practise one narrow behaviour per session: HTTP/status codes, SQL, authentication versus authorization, transactions/rollback, or retry/idempotency.

First SQL exercise: use a small schema with `accounts(id, name)` and `transfers(id, source_account_id, target_account_id, amount, created_at)`. Create your own synthetic rows, including an account with no transfer and an account appearing on both sides.

Write independently:

1. Look up an account by ID.
2. List transfers with both sender and receiver names.
3. Show every account's outgoing amount total, including accounts with zero outgoing transfers.

Write expected results before running queries. Do not equate outgoing totals with account balances. Compare your queries with the output, then explain joins, grouping, and how missing rows are represented.

Use [SQLBolt](https://sqlbolt.com/) for a missing SQL concept and [PostgreSQL tutorials](https://www.postgresql.org/docs/current/tutorial.html) for database behaviour. Store each exercise's schema, seed data, queries, and short notes together. Use synthetic data only.
