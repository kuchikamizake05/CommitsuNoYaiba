# Course 05 — Code Quality

Course: https://rocketversity.com/course/view.php?id=71

## Plan and scope

Reuse Tribun Auth from Course 04. Preserve all endpoints and responses, extract account verification and token creation into focused services, register them with dependency injection, and run the same HTTP tests. No duplicate app, additional database, or production authentication features. Source notes use independent examples, not copied course material.

## Clean code

- Give names that describe intent: `VerifyCredentials` and `CreateToken`.
- Keep one responsibility together: account/password operations in `DemoUserStore`; JWT creation in `TokenService`; HTTP responses remain at the endpoint.
- Keep input validation and 401/403 behavior when moving code. Refactoring changes structure, not intended behavior.
- Avoid interfaces or extra layers until there is a useful reason. DI works with concrete classes too.
- Shared token settings remove repeated issuer, audience, and expiration constants.

## Dependency injection

Without DI, an endpoint constructs its services itself. With DI, register the service once and ask for it in the endpoint parameters. The framework supplies an instance and manages its lifetime.

```csharp
builder.Services.AddSingleton<DemoUserStore>();
builder.Services.AddTransient<TokenService>();
app.MapPost("/login", (LoginRequest request, DemoUserStore users, TokenService tokens) => { /* ... */ });
```

Lifetimes:

| Registration | Lifetime | Typical use |
| --- | --- | --- |
| Transient | New instance each resolution | Lightweight stateless service |
| Scoped | One instance per HTTP request scope | EF DbContext and request-specific work |
| Singleton | One instance per application/service provider | Read-only shared data; mutable state must be thread-safe |

The demo account store is initialized once and then read-only. TokenService is stateless and transient. Do not inject a scoped DbContext into a singleton. DI is a construction/lifetime mechanism, not automatically a clean architecture.

## Quality tools

1. Compiler and .NET analyzers: identify compile errors and selected code-quality issues.
2. `dotnet format ... --verify-no-changes`: checks formatting/analyzer fixes without editing files.
3. HTTP integration tests: check behavior after refactoring.
4. NuGet vulnerability audit: checks dependencies against current advisory sources, not application business logic.
5. Coverage: shows executed code paths, not proof of correctness.

SonarQube adds static analysis, rules, duplication/coverage reporting and quality gates, usually through a scanner connected to a SonarQube server or cloud project. Visual Studio diagnostics/analysis expose warnings locally. VS Code plus .NET CLI can run the checks used here. No SonarQube scan, dashboard, or Visual Studio-specific analysis has been run in this exercise; those subtopics remain conceptual and should not be marked hands-on complete.

## Read the change

See `../course-04/tribun-auth/Program.cs`, `DemoUserStore.cs`, and `TokenService.cs`. Compare this branch with `course/04-auth` to see the before/after without copying the project. Use the Course 04 README for local-only credentials/setup and security limitations.

Official reference: https://learn.microsoft.com/aspnet/core/fundamentals/dependency-injection

## Verified 13 November 2026

Ubuntu WSL: all 13 existing HTTP tests still pass after refactoring, plus one DI lifetime test (14/14 total). Line coverage 97.87% (92/94), branch coverage 83.33%. `dotnet format` verification on the app passed without changes. NuGet audit including transitive packages found no known vulnerable packages at check time. No SonarQube scan or Visual Studio-specific tool execution; no claim that all course subtopics have been completed.
