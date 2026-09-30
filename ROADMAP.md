# Roadmap

## Purpose

This roadmap describes the intended evolution of **KUKULCAN.SharedKernel.JsonEngine**. It communicates direction rather than fixed delivery dates.

Correctness, deterministic behavior, testability, performance and a small API take precedence over feature volume.

## Current Scope

The project currently provides:

- JSON parsing and path navigation;
- typed JSON access and simple condition evaluation;
- a compact SQL-like query engine;
- directed graph construction, traversal, path finding and PageRank;
- simple, composite and lightweight full-text indexes;
- a rule-based natural-language to AI QUERY translator;
- AI QUERY execution over JSON and graph data;
- BenchmarkDotNet performance scenarios;
- a console demonstration client.

## Completed Foundations

### JSON Core

- parsing;
- dot-path navigation;
- JSONPath-style navigation used by the query engine;
- typed getters;
- basic comparison conditions.

### Query and Data Operations

- SELECT/FROM/WHERE/ORDER BY/LIMIT execution;
- numeric and string ordering;
- graph BFS/DFS/path finding;
- PageRank with empty and dangling graph handling;
- in-memory simple, composite and full-text indexes.

### AI QUERY

- FIND;
- optional WHERE;
- GRAPH EXPAND;
- PAGERANK ranking;
- RETURN projection;
- rule-based natural-language translation.

## Next Development Areas

### Query Language

Clarify and extend the deliberately compact SQL-like grammar without turning it into a full SQL implementation.

### Graph Semantics

Document and test edge cases for dependency resolution, traversal, ranking and graph rebuilding.

### Index Semantics

Define behavior for missing fields, duplicate values, tokenization and composite-key boundaries.

### AI QUERY

Expand the supported pipeline grammar only when backed by executable behavior tests and clear deterministic semantics.

### Performance

Use BenchmarkDotNet to identify regressions in JSON access, graph ranking and AI QUERY execution before introducing optimizations.

### Packaging and Release

When a formal public release is prepared, document package metadata, versioning policy, SourceLink/repository metadata, NuGet publication and compatibility policy.

## Architectural Boundaries

The project must not become:

- a database or persistence framework;
- an ASP.NET Core application host;
- an LLM provider or remote AI service;
- a general-purpose SQL engine;
- a general-purpose graph database.

New functionality should enter the shared component only when it is a reusable, deterministic JSON-processing requirement.

## Quality Goals

Maintain deterministic unit tests, integration/regression tests, fuzzing where useful, XML documentation, minimal public APIs and behavior-based coverage auditing.

Future behavior changes should continue to follow:

```
TEST → RED → Source → GREEN → Coverage → PR → merge
```
