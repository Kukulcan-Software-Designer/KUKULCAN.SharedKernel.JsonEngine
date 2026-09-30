# KUKULCAN.SharedKernel.JsonEngine Architecture

## Purpose

`KUKULCAN.SharedKernel.JsonEngine` is a reusable .NET 10 library for deterministic, in-memory JSON processing.

It combines JSON navigation, a compact SQL-like query engine, graph operations, indexes and a rule-based AI QUERY pipeline without owning persistence or an application host.

## Main Boundaries

```text
Consuming application
        |
        v
KUKULCAN.SharedKernel.JsonEngine
  |       |       |       |
  v       v       v       v
Core     SQL     Graph   Index
          \       /
           \     /
            v   v
          AI QUERY
```

### JSON Core

`JsonCore` provides parsing, JSONPath-style navigation, typed getters and basic condition evaluation.

### SQL Engine

`JsonSqlEngine` executes a deliberately compact dialect:

```text
SELECT -> FROM -> optional WHERE -> optional ORDER BY -> optional LIMIT
```

It operates on arrays inside `JsonNode` documents and returns projected JSON rows.

### Graph Engine

`JsonGraph` builds an in-memory directed graph from JSON nodes and dependency arrays. It supports BFS, DFS, path finding and PageRank.

### Index Engine

`JsonIndexEngine` provides simple, composite and lightweight full-text indexes over `JsonArray` values.

### AI QUERY

`JsonAiEngine` executes ordered query steps involving FIND, GRAPH EXPAND, PAGERANK and RETURN. `JsonAiNl` provides deterministic rule-based translation from supported natural-language patterns to AI QUERY text.

The AI layer does not call an LLM or remote AI service.

## Supporting Projects

### Benchmark

`KUKULCAN.SharedKernel.JsonEngine.Benchmark` contains BenchmarkDotNet scenarios for representative JSON, graph and AI-query operations.

### Source Client

`KUKULCAN.SharedKernel.JsonEngine.Client` is a console demonstration. It is not part of the core library API and does not become the application host for consumers.

### Tests

Unit tests validate deterministic engine behavior. Integration tests validate cross-component execution and fuzzing scenarios.

## Dependency Boundary

The production engine works in memory over `System.Text.Json.Nodes`.

It intentionally does not own:

- databases;
- caches;
- HTTP endpoints;
- authentication;
- authorization;
- external AI providers;
- application-specific persistence.

The consuming application owns persistence and lifecycle concerns around the JSON data.

## Data Flow

```text
Json text / JsonNode
        |
        v
     JsonCore
        |
   +----+----+------+
   |         |      |
  SQL      Graph   Index
   |         |      |
   +----+----+------+
        |
        v
    JsonAiEngine
        |
        v
   projected results
```

The boundaries are intentionally small so each engine can be tested independently.
