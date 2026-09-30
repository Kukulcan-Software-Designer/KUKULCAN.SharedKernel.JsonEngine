# Architecture Decisions

## ADR-001 — Keep the Engine In-Memory

**Status:** Accepted

The library operates over `System.Text.Json.Nodes` in memory. Persistence, caching and storage lifecycle remain responsibilities of consuming applications.

This keeps the core deterministic and avoids coupling the engine to a database or hosting environment.

## ADR-002 — Use a Compact SQL-Like Dialect

**Status:** Accepted

JsonEngine implements a deliberately small query language rather than attempting to become a standards-compliant SQL engine.

The supported model is SELECT/FROM with optional WHERE, ORDER BY and LIMIT.

## ADR-003 — Separate Engine Responsibilities

**Status:** Accepted

JSON navigation, SQL execution, graph operations, indexes and AI QUERY execution remain separate components.

This prevents query syntax, graph algorithms and JSON primitives from becoming one coupled implementation.

## ADR-004 — Keep AI Query Translation Deterministic

**Status:** Accepted

Natural-language translation is rule-based and local.

The current implementation does not call an LLM, does not require network access and does not delegate semantics to an external provider.

## ADR-005 — Expose Small Testable Contracts

**Status:** Accepted

`IJsonGraph` and `IJsonSqlEngine` provide injectable boundaries where the AI pipeline needs isolated graph and query behavior.

Public abstractions should only be introduced when they represent a stable reusable boundary.

## ADR-006 — Preserve In-Memory Graph Semantics

**Status:** Accepted

Graph construction rebuilds the current graph state and traversal/ranking operate on the resulting in-memory node and edge sets.

Unknown traversal starts return no traversal result rather than inventing an unknown graph node.

## ADR-007 — Coverage Is an Audit Signal

**Status:** Accepted

Tests are added for behavior, security-relevant input handling and regressions. Tests are not created solely to increase a coverage percentage.

Coverage should be interpreted alongside the behavior represented by the tests.
