# API Reference

This is the API reference hub for D20Tek.Spectre.Console.Extensions. Because the library spans a core package and several add-ons, the reference is split into focused documents that mirror the namespaces and packages. Start here, then follow the link for the area you are working in.

## Core Package Reference

| Document | Namespace | Covers |
|---|---|---|
| [Core](api-reference-core.md) | `D20Tek.Spectre.Console.Extensions` | The `CommandAppBuilder` and startup types, the DI type registrar/resolver and lifetime helpers, verbosity-aware logging, the verbosity output service, and the extra prompt and console controls. |
| [Testing](api-reference-test.md) | `D20Tek.Spectre.Console.Extensions.Testing` | The test context classes, the end-to-end runner, and the result types. |

## Add-on Package Reference

| Document | Package | Covers |
|---|---|---|
| [Configuration](api-reference-configuration.md) | `D20Tek.Spectre.Console.Extensions.Configuration` | The `WithConfiguration` and `WithOptions<TOptions>` builder extensions. |
| [Hosting](api-reference-hosting.md) | `D20Tek.Spectre.Console.Extensions.Hosting` | Generic Host bridging, `HostStartupBase`, and the host command-app extensions and builder. |
| [MoreContainers](api-reference-morecontainers.md) | `D20Tek.Spectre.Console.Extensions.MoreContainers` | The Autofac, Lamar, LightInject, and Ninject registrars, resolvers, and builder extensions. |

## Conventions

- The core package targets `net9.0` and `net10.0` and depends only on `Microsoft.Extensions.DependencyInjection`; other containers are additive through the MoreContainers package.
- Extension methods are documented under the type they extend or the package that provides them.
- Types and members marked `internal` are excluded from this reference; only the public surface is documented.
- For task-oriented walkthroughs, see the [guides](getting-started-detailed.md#guides).
