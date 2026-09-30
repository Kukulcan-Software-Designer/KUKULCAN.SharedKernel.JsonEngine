# KUKULCAN.SharedKernel.JsonEngine — Test, TDD and Coverage Audit

## Purpose

This document defines the audit approach used to verify the relationship between behavior-driven development, automated tests and code coverage in JsonEngine.

The goal is not to maximize a coverage percentage artificially. The goal is to establish that relevant production behavior is intentionally exercised and that coverage results can reveal missing scenarios.

## Test Structure

JsonEngine currently separates automated tests into:

| Area                                           | Responsibility                                                                                      |
|------------------------------------------------|-----------------------------------------------------------------------------------------------------|
| `KUKULCAN.SharedKernel.JsonEngine.UnitTests`   | Focused production behavior and component-level contracts                                           |
| `KUKULCAN.SharedKernel.JsonEngine.Integration` | Behavior that requires meaningful interaction between JsonEngine components or execution boundaries |

The repository does not require database-provider integration projects because JsonEngine does not own relational persistence infrastructure.

## TDD Development Cycle

For behavior changes, the preferred workflow is:

```text
TEST
  ↓
RED
  ↓
Source
  ↓
GREEN
  ↓
Coverage
  ↓
PR
  ↓
merge
```

### TEST

Define the externally observable behavior that is missing.

### RED

Add the smallest behavior-focused test that demonstrates the missing behavior and fails for the expected reason.

### Source

Implement only the production change required to satisfy the failing behavior.

### GREEN

Run the relevant tests and then the applicable complete test suite to verify that the implementation does not regress existing behavior.

### Coverage

Collect Cobertura coverage and inspect uncovered production paths. Missing coverage should lead to additional meaningful behavior tests when a real scenario is not represented.

### PR and merge

Open the pull request only after the behavior and applicable quality checks are green.

## Test Design Principles

Tests should:

- describe observable behavior;
- use stable, deterministic inputs;
- avoid testing implementation details when the public contract is sufficient;
- isolate test state;
- include regression coverage for corrected defects;
- avoid artificial branches or assertions whose only purpose is increasing coverage.

For JSON processing, tests should pay particular attention to:

- valid and invalid JSON;
- scalar and structured values;
- query filtering and ordering;
- limits and boundary values;
- graph construction and traversal;
- index behavior;
- deterministic AI QUERY translation;
- empty and unknown inputs;
- repeated execution where state could otherwise leak between tests.

## Coverage Workflow

`.github/workflows/coverage.yml`:

1. Restores the solution.
2. Builds the solution in Release configuration.
3. Runs Unit Tests with XPlat Code Coverage.
4. Runs Integration Tests with XPlat Code Coverage.
5. Produces Cobertura reports.
6. Publishes the unit and integration coverage artifacts.

Coverage is configured to include JsonEngine production assemblies and exclude the test assemblies.

## Coverage Interpretation

Coverage percentages are evidence, not a definition of test quality.

A high line rate does not prove that behavior is correct, while an uncovered line may be intentional or may identify a missing scenario.

Coverage review should therefore consider:

- uncovered lines;
- uncovered branches;
- boundary conditions;
- error paths;
- state transitions;
- regression scenarios;
- whether the test exercises the public behavior rather than an implementation detail.

## Audit Completion

A behavior change should not be considered complete until:

- the intended behavior has an automated test;
- the relevant tests pass;
- the complete applicable test suite passes;
- coverage has been inspected;
- meaningful uncovered production paths have been assessed;
- documentation has been updated when the public contract or architecture changes.

The audit result should be recorded in the pull request or associated project documentation when the change is substantial.
