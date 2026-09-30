# Configuration

## Scope

`KUKULCAN.SharedKernel.JsonEngine` is a class library and does not define an ASP.NET Core configuration section.

The consuming application supplies the JSON documents, query text and object lifecycle.

## Runtime Requirements

- .NET SDK 10.0.
- Nullable reference types enabled.
- No database, Docker service or external provider is required by the production library.

## Data Input

The core APIs accept `JsonNode` data or JSON text parsed through `JsonCore`.

The library does not own where JSON originates or where results are persisted.

## Query Configuration

`JsonSqlEngine` accepts query text at execution time. Its grammar is intentionally limited to the documented SELECT/FROM/WHERE/ORDER BY/LIMIT model.

Invalid or unsupported query input should be treated as caller input and covered by tests where behavior is defined.

## Graph Configuration

`JsonGraph.Build` receives the JSON root, node path, identifier field and dependency field. The caller therefore defines the graph projection from the JSON document.

## AI QUERY

`JsonAiEngine` receives the root document, graph, query text and SQL engine boundary. `JsonAiNl` converts only the currently supported natural-language patterns.

No API key, remote endpoint or model configuration is required.

## Dependency Injection

The library does not require a particular dependency-injection framework. Where isolation is useful, callers and tests can use the public `IJsonGraph` and `IJsonSqlEngine` contracts.

## Security

Treat input JSON and query expressions as untrusted data. Do not place secrets in JSON fixtures, query examples or logs.

The library does not log or transmit input to external services.
