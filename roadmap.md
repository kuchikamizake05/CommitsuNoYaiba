# How to use this repository

This is a practice system. The first cycle is an adjustable one-week experiment, not a deadline or a fixed interview curriculum.

## Tools

- VS Code or your existing editor, Java 21, and Git. Java is the initial practice language because LedgerFlow already uses it; review that choice after the baseline.
- A free LeetCode account for problem statements, execution, and submissions.
- A timer. Record explanations with your phone if useful; keep recordings outside this public repository.
- Existing LedgerFlow source for backend and project exercises. Docker/PostgreSQL are only needed for exercises that actually execute database operations.
- Official language documentation for syntax lookup. Turn off AI completion during independent attempts.

No new framework or paid subscription is required for the first cycle.

## One ordinary session: up to 60 minutes

1. **5 minutes:** read the task, restate inputs/outputs, and invent two examples.
2. **25 minutes:** attempt a solution independently. Write a simple approach before trying to optimize. Compiler feedback and syntax documentation are allowed; log syntax lookups.
3. **10 minutes:** if blocked, request one hint, then try again. If unblocked, use this time to improve tests or examine another approach.
4. **10 minutes:** test boundaries and explain why the solution works, its time cost, and its extra memory cost.
5. **10 minutes:** save your attempt, write a short note, and update the tracker. Schedule a retry.

Stop when the timebox ends. An incomplete attempt is useful evidence. On a busy day, do a 15-minute recall/retry session. After a missed day, resume the next task; do not double the workload.

## AI rules

- For assessment/retry sessions: no AI, editor AI completion, editorial, or old solution.
- For normal sessions: attempt for 25 minutes first. Send your own code and explain exactly where you are stuck.
- Ask: "Give me one guiding question. Do not write code or reveal the full solution."
- After another genuine attempt, you may study an explanation or full solution. Record that help and mark the task **needs retry**.
- Close the explanation and reconstruct the reasoning yourself. Same-session reconstruction does not prove retention; retry on another day.
- For a new concept, a short tutor explanation is allowed before practice. The independent exercise must use a fresh example.

## First cycle

Use session numbers rather than calendar deadlines. Aim to start four sessions; use the remaining sessions if capacity allows.

| Session | Task | Evidence |
| --- | --- | --- |
| 1 | Baseline: Contains Duplicate, up to 40 minutes without help; explain LedgerFlow's transfer flow from memory for 5 minutes | Your attempt, tests, and the exact point you got stuck |
| 2 | Review session 1; learn only the missing concept, then try Two Sum | Attempt + help used + reasoning |
| 3 | Retry Contains Duplicate from a blank editor; inspect the LedgerFlow transfer flow afterward | Retry result + one corrected project explanation |
| 4 | Try Valid Anagram | Attempt + cases + complexity explanation |
| 5 | Write three SQL queries against a small account/transfer schema: lookup, joined transfer history, grouped totals | Queries + expected output; see backend guide |
| 6 | Retry Two Sum while speaking aloud, up to 25 minutes | Whether you could clarify, code, test, and explain independently |
| 7 | Review for 20 minutes; retry Valid Anagram if due | Actual session count, retained skills, and next adjustment |

Change the next cycle based on evidence: syntax trouble -> a focused fundamentals exercise; approach trouble -> another small problem in the same family; explanation trouble -> repeat aloud. Do not add more topics just because a week has passed.

## What counts as progress

- **Not started:** no attempt yet.
- **Attempted:** an independent attempt exists, even if incorrect.
- **Needs retry:** help was required or explanation/testing is incomplete.
- **Retained:** solved from scratch on another day, with tests and a correct explanation.
- **Transfer checked:** handled a small variation and explained why the approach changes or stays valid.

An accepted submission alone is not a mastery score. Record a successful cold retry after about 2 days, then again after about 7 days. If a retry fails, record the missing step and schedule another attempt after focused review. Treat these intervals as defaults, not obligations.

At the end of each session, select the next action: first any due retry, otherwise the next starter problem. Log in [tracker.md](tracker.md). Do not publish personal reflections, applications, recordings, passwords, tokens, or private company material.
