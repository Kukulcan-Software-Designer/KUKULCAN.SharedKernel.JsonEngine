# Governance

## Purpose

`KUKULCAN.SharedKernel.JsonEngine` is governed as a focused JSON processing library. Repository decisions should preserve predictable behavior, small contracts, low coupling and long-term maintainability.

The project is intended to provide reusable in-memory JSON capabilities without taking ownership of application persistence, authentication, HTTP infrastructure or external AI services.

## Core Principles

- **Single responsibility:** JsonEngine provides JSON processing capabilities and related deterministic query/graph/index operations.
- **Explicitness:** public behavior should be visible through named APIs and documented contracts.
- **Low coupling:** the core library should avoid unnecessary dependencies on application infrastructure.
- **Determinism:** equivalent inputs should produce predictable results; AI QUERY translation is rule-based and does not depend on a remote LLM service.
- **Stability:** public contracts should change only for justified requirements, defects or architectural improvements.
- **Testability:** behavior should be independently verifiable through focused tests.
- **Documentation:** architecture, public behavior and significant decisions are part of the project contract.

## Branch Model

The repository uses the following branch roles:

- `main`: stable, releasable history.
- `Develop`: integration branch for completed feature work before promotion to `main`.
- `FEATURES/*`: short-lived branches for a specific feature, correction, documentation change or other isolated work.

Feature branches should normally be created from `Develop`.

The normal promotion flow is:

```text
FEATURES/*
    ↓
Develop
    ↓
main
```

A feature branch should not bypass `Develop` unless an explicit repository decision requires it.

## Pull Request Process

Pull requests should:

1. Have a clear English title.
2. Describe the purpose and scope of the change in English.
3. Identify the relevant behavior, documentation or maintenance change.
4. Include appropriate labels.
5. Have the appropriate assignee.
6. Pass the applicable GitHub Actions checks before merge.
7. Avoid unrelated changes.

For behavior changes, the preferred development cycle is:

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

Documentation-only and repository-maintenance changes may omit the test-first production-code step when no executable behavior is changed.

## GitHub Actions Quality Gates

The repository currently separates fast validation from coverage:

### CI

`.github/workflows/ci.yml` runs on pushes to all branches and on pull requests targeting `main`.

It validates:

- restore;
- Release build;
- unit tests;
- JsonEngine integration tests.

### Code Coverage

`.github/workflows/coverage.yml` runs on pushes to `main`, pull requests targeting `main`, and manual dispatch.

It executes unit and integration tests with XPlat Code Coverage and publishes Cobertura artifacts.

Coverage is an audit signal. It must be interpreted together with behavior-focused tests and the actual production paths exercised.

### Client Validation

The `SourceClient/KUKULCAN.SharedKernel.JsonEngine.Client` project is a supported executable reference client and has a dedicated workflow:

`.github/workflows/client.yml`

The workflow validates that the client restores and builds successfully.

## Merge Policy

Changes targeting `Develop` should have a passing CI validation before merge.

Changes promoted from `Develop` to `main` should have the applicable CI and coverage checks in a successful state before merge.

When a merge triggers post-merge Actions on the target branch, those checks should be allowed to complete successfully before considering the promotion complete.

Direct changes to `main` should be avoided when the change can follow the normal pull request process.

## Architecture and Public API

Every new public type or member should have a clear consumer-facing purpose.

Prefer implementation details to remain non-public whenever possible.

Before introducing a public API change, consider:

- responsibility;
- cohesion;
- coupling;
- compatibility;
- naming consistency;
- testability;
- documentation impact.

Breaking changes should be exceptional and normally reserved for an appropriate major-version change.

## Testing and Coverage

Tests are part of the behavioral contract.

Unit tests should isolate individual production behaviors. Integration tests should verify interactions that cross meaningful component boundaries.

New behavior should normally be introduced through a failing behavior test before production implementation.

Coverage should be used to discover untested paths and support audit decisions, not as a target achieved through artificial tests.

## Documentation Policy

Documentation is part of the repository contract.

Changes that alter public behavior, architecture, configuration, testing strategy or repository workflow should update the relevant documentation.

The `Documentation/` directory contains technical documentation, while repository-level governance documents define contribution, security, support and maintenance expectations.

## Maintenance

Maintenance work should prioritize:

- correctness;
- compatibility;
- deterministic behavior;
- performance where evidence justifies it;
- documentation quality;
- test quality;
- dependency and SDK consistency.

Unrelated refactoring should not be mixed into focused feature or documentation changes.

## Release and Versioning

Releases should preserve the project's public API expectations and document user-visible changes in `CHANGELOG.md`.

Versioning should follow Semantic Versioning where applicable:

```text
Major.Minor.Patch
```

Before a release, the project should have:

- a successful build;
- passing applicable tests;
- successful required Actions;
- reviewed public API changes;
- updated documentation;
- updated changelog information.

## Governance Changes

Significant architectural or repository-process changes should be documented and reviewed through the normal pull request process.

The governance model itself may evolve when the project's architecture, distribution model or maintenance requirements change.
