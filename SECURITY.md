# Security Policy

**KUKULCAN.SharedKernel.JsonEngine** is an in-memory JSON processing library. It does not authenticate users, persist credentials or provide an HTTP service, but it processes caller-supplied JSON and query expressions.

## Supported Versions

| Version | Support |
|---|---|
| Latest stable release | Supported |
| Older unsupported releases | Not supported |
| Pre-release versions | Evaluation/testing only |

## Reporting a Vulnerability

Do not report security vulnerabilities through public Issues or Discussions.

Use GitHub Private Vulnerability Reporting when enabled. Otherwise, contact the project maintainers privately.

Include the affected version or commit, .NET version, operating system, description, reproduction steps, proof of concept where safe, security impact and suggested mitigation.

Do not include real credentials, secrets or private data.

## Relevant Security Areas

Reports are especially important for:

- unsafe processing of untrusted JSON;
- query-parser bypasses or unexpected execution;
- excessive resource consumption through pathological JSON or graph inputs;
- incorrect graph traversal or ranking behavior that can expose unintended data;
- malformed input causing process-level failures;
- accidental introduction of network, database or filesystem dependencies.

## Security Principles

- JSON input is treated as untrusted data.
- The query language remains deliberately constrained.
- The AI/NL helper is rule-based and does not execute arbitrary code.
- The library does not perform network calls as part of its core processing.
- The library does not own persistence or credential storage.
- Security fixes should include regression tests whenever feasible.

## Dependencies

Dependencies should be evaluated before introduction and kept current where supported. Production functionality should remain independent of unnecessary external infrastructure.

## Secure Development

Changes affecting parsing, query execution, graph traversal, indexing or AI-query interpretation should be tested against malformed and boundary inputs where relevant.
