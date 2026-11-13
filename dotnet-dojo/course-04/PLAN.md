# Tribun Auth: design and verification plan

Scope approved in chat: one .NET 8 learning API with login, JWT, profile, and an Admin-only endpoint. Synthetic accounts stay in memory; use Identity's PasswordHasher, not a custom password algorithm. Token signature, issuer, audience, and lifetime must be validated. Signing key and demo passwords come from environment variables. Bind locally for learning; do not deploy this account/token issuer as production authentication.

Implementation sequence:
1. Add HTTP integration tests for login, input rejection, 401/403, role enforcement, and token tampering.
2. Run tests against an empty app and capture failures.
3. Implement the minimum app and rerun with coverage.
4. Audit dependencies and document limitations and the request flow. Preserve course-platform completion for the learner to confirm.

Future Code Quality study can reuse this app. No registration, refresh tokens, persistent users, SQL Server, or company data in this slice.
