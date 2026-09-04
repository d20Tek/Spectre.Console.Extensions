# API Reference: Hosting

Package: `D20Tek.Spectre.Console.Extensions.Hosting`
Namespace: `D20Tek.Spectre.Console.Extensions.Hosting`

This document covers bridging Spectre.Console.Cli to the .NET Generic Host, the host-aware startup, and the host command-app extensions and builder.

## Contents

- [HostCommandAppExtensions](#hostcommandappextensions)
- [HostCommandAppBuilder](#hostcommandappbuilder)
- [HostStartupBase](#hoststartupbase)
- [HostStartupExtensions](#hoststartupextensions)
- [HostRegistration](#hostregistration)
- [HostTypeResolver](#hosttyperesolver)

## HostCommandAppExtensions

Extension methods on `IHost` that create and run a bridged `CommandApp`.

| Member | Signature | Description |
|---|---|---|
| `RunCommandAppAsync` | `static Task<int> RunCommandAppAsync(this IHost host, string[] args, Action<IConfigurator> configure)` | Creates a bridged CommandApp and runs it asynchronously. Throws `ArgumentNullException` when host, args, or configure is null. |
| `RunCommandApp` | `static int RunCommandApp(this IHost host, string[] args, Action<IConfigurator> configure)` | Creates a bridged CommandApp and runs it synchronously. Throws `ArgumentNullException` when host, args, or configure is null. |
| `CreateCommandApp` | `static CommandApp CreateCommandApp(this IHost host, Action<IConfigurator> configure)` | Creates a bridged CommandApp using a `HostTypeRegistrar`, applying any registered `HostStartupBase` command configuration followed by the supplied configuration. Throws `ArgumentNullException` when host or configure is null. |
| `CreateCommandAppBuilder` | `static HostCommandAppBuilder CreateCommandAppBuilder(this IHost host)` | Creates a fluent `HostCommandAppBuilder` bridged to the host. Throws `ArgumentNullException` when host is null. |

## HostCommandAppBuilder

A sealed fluent builder for a host-bridged CommandApp.

| Member | Signature | Description |
|---|---|---|
| Constructor | `HostCommandAppBuilder(IHost host)` | Creates the builder bridged to the given host. Throws `ArgumentNullException` when host is null. |
| `Host` | `IHost Host { get; }` | The host associated with this builder. |
| `WithDefaultCommand<TDefault>` | `HostCommandAppBuilder WithDefaultCommand<TDefault>()` where `TDefault : class, ICommand` | Sets the default command. |
| `ConfigureCommands` | `HostCommandAppBuilder ConfigureCommands(Action<IConfigurator> configure)` | Adds command configuration. Throws `ArgumentNullException` when configure is null. |
| `Build` | `HostCommandAppBuilder Build()` | Builds the bridged CommandApp. |
| `RunAsync` | `Task<int> RunAsync(string[] args)` | Runs the app asynchronously, building it first when needed. Throws `ArgumentNullException` when args is null. |
| `Run` | `int Run(string[] args)` | Runs the app synchronously, building it first when needed. Throws `ArgumentNullException` when args is null. |

## HostStartupBase

Abstract host-aware startup that splits service registration from command configuration.

| Member | Signature | Description |
|---|---|---|
| `ConfigureServices` | `abstract void ConfigureServices(IServiceCollection services)` | Override to register services against the host's service collection (pre-build). |
| `ConfigureCommands` | `abstract IConfigurator ConfigureCommands(IConfigurator config)` | Override to configure commands, applied post-build when the CommandApp is created. |

## HostStartupExtensions

| Member | Signature | Description |
|---|---|---|
| `WithStartup<TStartup>` | `static IHostApplicationBuilder WithStartup<TStartup>(this IHostApplicationBuilder builder)` where `TStartup : HostStartupBase, new()` | Registers a `HostStartupBase`, running its `ConfigureServices` immediately and registering it so `ConfigureCommands` is applied when the CommandApp is built. Throws `ArgumentNullException` when builder is null. |

## HostRegistration

A sealed descriptor for a runtime registration captured from Spectre and resolved through the host's provider.

| Member | Signature | Description |
|---|---|---|
| `ForType` | `static HostRegistration ForType(Type implementationType)` | Creates a registration for an implementation type. |
| `ForInstance` | `static HostRegistration ForInstance(object instance)` | Creates a registration for an existing instance. |
| `ForFactory` | `static HostRegistration ForFactory(Func<object> factory)` | Creates a registration backed by a factory. |
| `Resolve` | `object Resolve(IServiceProvider provider)` | Resolves the registration using the given provider. |

## HostTypeResolver

A sealed `ITypeResolver` that resolves types through a composite of the host provider and captured registrations.

| Member | Signature | Description |
|---|---|---|
| Constructor | `HostTypeResolver(IServiceProvider provider, IReadOnlyDictionary<Type, HostRegistration> registrations)` | Creates the resolver over the provider and captured registrations. |
| `Resolve` | `object? Resolve(Type? type)` | Resolves a type, with host services taking precedence over captured registrations. |

## Related

- [Guide: Generic Host Integration](guide-generic-host.md)
- [API Reference hub](api-reference.md)
