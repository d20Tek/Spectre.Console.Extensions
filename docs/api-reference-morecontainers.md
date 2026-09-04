# API Reference: MoreContainers

Package: `D20Tek.Spectre.Console.Extensions.MoreContainers`
Namespace: `D20Tek.Spectre.Console.Extensions.Injection` (builder extensions in `D20Tek.Spectre.Console.Extensions`)

This document covers `ITypeRegistrar`/`ITypeResolver` support for Autofac, Lamar, LightInject, and Ninject, along with the builder extensions that select each container.

## Contents

- [CommandAppBuilderExtensions](#commandappbuilderextensions)
- [Registrars and Resolvers](#registrars-and-resolvers)

## CommandAppBuilderExtensions

Extension methods that configure a specific container on a `CommandAppBuilder`.

| Member | Signature | Description |
|---|---|---|
| `WithNinjectContainer` | `static CommandAppBuilder WithNinjectContainer(this CommandAppBuilder builder, StandardKernel? container = null)` | Configures the Ninject registrar, creating a new `StandardKernel` when none is provided. |
| `WithAutofacContainer` | `static CommandAppBuilder WithAutofacContainer(this CommandAppBuilder builder, ContainerBuilder? container = null)` | Configures the Autofac registrar, creating a new `ContainerBuilder` when none is provided. |
| `WithLightInjectContainer` | `static CommandAppBuilder WithLightInjectContainer(this CommandAppBuilder builder, ServiceContainer? container = null, ServiceLifetime lifetime = ServiceLifetime.Singleton)` | Configures the LightInject registrar with an optional container and default lifetime. |
| `WithLamarContainer` | `static CommandAppBuilder WithLamarContainer(this CommandAppBuilder builder, ServiceRegistry? serviceRegistry = null, ServiceLifetime lifetime = ServiceLifetime.Singleton)` | Configures the Lamar registrar with an optional service registry and default lifetime. |

## Registrars and Resolvers

Each container has a registrar (`ITypeRegistrar`) and resolver (`ITypeResolver`) that bridge Spectre.Console.Cli to that framework. They are configured for you by the builder extensions above; you typically do not construct them directly.

| Container | Registrar | Resolver |
|---|---|---|
| Ninject | `NinjectTypeRegistrar(StandardKernel kernel)` | `NinjectTypeResolver(StandardKernel provider)` |
| Autofac | `AutofacTypeRegistrar(ContainerBuilder container)` | `AutofacTypeResolver(ILifetimeScope scope)` |
| LightInject | `LightInjectTypeRegistrar(ServiceContainer container)` | `LightInjectTypeResolver(IServiceFactory container)` |
| Lamar | `LamarTypeRegistrar(ServiceRegistry registry, ServiceLifetime lifetime = ServiceLifetime.Singleton)` (also implements `ISupportLifetimes`) | `LamarTypeResolver(Container container)` |

Each registrar implements the standard `ITypeRegistrar` members: `Build`, `Register`, `RegisterInstance`, and `RegisterLazy`. Each resolver implements `Resolve` and, where applicable, `IDisposable.Dispose`.

## Related

- [Guide: Using Additional DI Containers](guide-more-containers.md)
- [API Reference: Core](api-reference-core.md#dependency-injection)
- [API Reference hub](api-reference.md)
