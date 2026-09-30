# Contributing to KUKULCAN.SharedKernel.JsonEngine

Contributions should preserve the library's deterministic, in-memory JSON processing model and its architectural boundaries.

## Before Contributing

- Read the repository documentation.
- Search existing Issues and Pull Requests.
- Identify the executable behavior affected by the change.
- Confirm that the proposed behavior belongs in JsonEngine rather than in a consuming application or another KUKULCAN shared library.

## Architecture

The project targets **.NET 10** and separates:

- `Source/` — production library and benchmark code;
- `SourceClient/` — console demonstration client;
- `Tests/` — unit and integration tests;
- repository-level Markdown — governance and technical documentation.

The core library operates in memory over `System.Text.Json.Nodes`. It does not introduce database, Redis, HTTP-host or external-service infrastructure.

## TDD Contract

Behavior changes should follow:

```
TEST → RED → Source → GREEN → Coverage → PR → merge
```

1. Define the behavior with a test.
2. Confirm RED when the behavior is not implemented.
3. Implement the minimum production change.
4. Confirm GREEN.
5. Run the relevant coverage audit.
6. Open a Pull Request with validation evidence.
7. Merge only after required CI is GREEN.

Do not add tests solely to increase a coverage percentage.

## Testing

Unit tests validate deterministic behavior for the JSON core, SQL, graph, index and AI pipeline.

Integration tests validate cross-component behavior and fuzzing scenarios without introducing external infrastructure dependencies.

Moq may be used through the public `IJsonGraph` and `IJsonSqlEngine` contracts when isolation is appropriate.

## Branches

Feature and behavior branches normally start directly from `Develop`.

Use `FEATURES/<BEHAVIOR>` for feature or behavior work and a clearly scoped `Fix/<NAME>` branch for corrections.

## Public API

Keep the public API minimal. Before adding a public abstraction:

- verify that an existing contract cannot be reused;
- document the architectural reason;
- add behavior tests;
- preserve nullable and XML-documentation conventions.

## Coding Standards

Follow the existing .NET 10 conventions:

- nullable reference types;
- implicit usings;
- file-scoped namespaces;
- XML documentation for public APIs;
- deterministic behavior;
- no unnecessary infrastructure dependencies.

## Pull Requests

Pull Requests should address one coherent concern, use an English title and description, explain the behavior or documentation change, list validation performed and identify relevant architectural impact.

## Security

Do not commit passwords, tokens, client secrets, signing keys or private keys.

Security vulnerabilities must be reported privately according to [SECURITY.md](SECURITY.md).
