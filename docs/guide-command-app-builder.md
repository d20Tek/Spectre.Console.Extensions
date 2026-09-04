# Guide: Building CommandApps with CommandAppBuilder

`CommandAppBuilder` is the fluent entry point for creating, configuring, and running a Spectre.Console `CommandApp`. This guide covers the common configuration steps.

## Create and run

The typical flow chains a DI container, a startup class, an optional default command, `Build`, and `RunAsync`:

```csharp
using D20Tek.Spectre.Console.Extensions;

return await new CommandAppBuilder()
	.WithDIContainer()
	.WithStartup<Startup>()
	.WithDefaultCommand<DefaultCommand>()
	.Build()
	.RunAsync(args);
```

## Choose a DI container

`WithDIContainer` configures the built-in `Microsoft.Extensions.DependencyInjection` registrar. You can pass a pre-populated `IServiceCollection` and a default `ServiceLifetime`:

```csharp
var services = new ServiceCollection();
services.AddSingleton<IClock, SystemClock>();

var builder = new CommandAppBuilder()
	.WithDIContainer(services, ServiceLifetime.Singleton);
```

To use Autofac, Lamar, LightInject, or Ninject, install the MoreContainers package and call the matching extension (see [Using Additional DI Containers](guide-more-containers.md)). To supply a custom registrar directly, call `SetRegistrar`.

## Set a default command

`WithDefaultCommand<TDefault>` registers the command that runs when the user does not specify a command name. `TDefault` must implement `ICommand`.

## Access the container from extensions

Add-on packages reach the builder's container through `GetServiceCollection`, which returns the registrar's `IServiceCollection`. This throws `InvalidOperationException` if no container has been configured, so always call `WithDIContainer` (or a container extension) first. The `Registrar` property exposes the underlying `ITypeRegistrar` when an extension needs it.

## Build and run

`Build` configures services, creates the `CommandApp`, applies the default command, and configures commands. After building, call `RunAsync(args)` or `Run(args)`; both return the application exit code. `Build` throws `ArgumentNullException` if no startup class was set with `WithStartup`.

## Related

- [Getting Started](getting-started-detailed.md)
- [API Reference: Core](api-reference-core.md)
