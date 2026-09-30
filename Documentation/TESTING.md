# Testing

## Philosophy

Behavior changes should follow:

```
TEST → RED → Source → GREEN → Coverage → PR → merge
```

Tests must specify meaningful JSON-processing, query, graph, indexing, AI-pipeline, boundary or regression behavior.

## Test Projects

```text
Tests/
├── KUKULCAN.SharedKernel.JsonEngine.UnitTests/
└── KUKULCAN.SharedKernel.JsonEngine.Integration/
```

## Unit Tests

Unit tests validate deterministic behavior without databases or live external services.

They cover:

- JSON parsing and navigation;
- typed getters and conditions;
- SQL parsing and execution;
- graph construction and algorithms;
- indexes;
- AI QUERY translation and execution;
- boundary and regression cases;
- fuzzing-oriented cases where appropriate.

## Integration Tests

Integration tests validate behavior across the engine components, including AI execution and graph/query integration.

They may include fuzzing and stress-oriented scenarios when the objective is to validate robustness rather than infrastructure integration.

## Mocking

The AI pipeline can isolate graph and SQL behavior through:

- `IJsonGraph`;
- `IJsonSqlEngine`.

Moq may be used for those boundaries when a test is specifically concerned with orchestration rather than the concrete implementation.

## Test Matrix

| Area | Representative behavior |
|---|---|
| JSON Core | parse, path navigation, typed access, conditions |
| SQL | projection, filtering, numeric/string ordering, limit |
| Graph | build, BFS, DFS, path finding, PageRank |
| Index | simple, composite and full-text lookup |
| AI QUERY | FIND, WHERE, GRAPH EXPAND, PAGERANK, RETURN |
| NL helper | supported rule-based translations |
| Robustness | malformed/boundary/fuzzing inputs |

## TDD Rules

A production behavior change should be preceded by its corresponding behavior test.

If a newly formalized test documents behavior already guaranteed by production code, immediate GREEN is valid and production code should not be changed merely to manufacture RED.

Do not add artificial tests solely for coverage.

## CI and Coverage

The CI workflow validates restore, build, unit tests and integration tests.

The coverage workflow separately runs the same functional suites with Cobertura collection and publishes artifacts.
