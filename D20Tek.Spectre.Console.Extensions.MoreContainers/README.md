[![NuGet](https://img.shields.io/nuget/v/D20Tek.Spectre.Console.Extensions.MoreContainers.svg)](https://www.nuget.org/packages/D20Tek.Spectre.Console.Extensions.MoreContainers)

# D20Tek.Spectre.Console.Extensions.MoreContainers

`D20Tek.Spectre.Console.Extensions.MoreContainers` provides additional dependency injection container integrations for [Spectre.Console.Cli](https://spectreconsole.net/cli/). It supplies `ITypeRegistrar` and `ITypeResolver` implementations and fluent `CommandAppBuilder` hooks for the Autofac, Lamar, LightInject, and Ninject containers.

This is a separate package that references the core `D20Tek.Spectre.Console.Extensions` package. It keeps the third-party container dependencies out of the core package, so applications that use the built-in Microsoft.Extensions.DependencyInjection container do not pay for them.

## Why a separate package?

The core library integrates the Microsoft.Extensions.DependencyInjection container out of the box. Not every application needs an alternative container, and each supported container brings its own transitive dependencies. Isolating these integrations here keeps the core package lean while still giving teams a first-class option for their preferred container.

## Installation

```shell
dotnet add package D20Tek.Spectre.Console.Extensions.MoreContainers
```

## Supported containers

The package adds fluent `CommandAppBuilder` extension methods for the following containers:

- Autofac - `WithAutofacContainer`
- Lamar - `WithLamarContainer`
- LightInject - `WithLightInjectContainer`
- Ninject - `WithNinjectContainer`

Each method sets the builder's type registrar to the container-specific implementation, so Spectre.Console resolves command types and their dependencies from that container.

## Usage

Call the container-specific hook on `CommandAppBuilder` instead of the core `WithDIContainer` method. Each hook optionally accepts a pre-configured container instance; when omitted, a new empty container is created.

### Autofac

```csharp
using Autofac;
using D20Tek.Spectre.Console.Extensions;

return await new CommandAppBuilder()
                 .WithAutofacContainer()
                 .WithDefaultCommand<GreetCommand>()
                 .Build()
                 .RunAsync(args);
```

### Lamar

`WithLamarContainer` also accepts an optional default `ServiceLifetime` (defaults to `Singleton`) that applies to its registrations:

```csharp
using D20Tek.Spectre.Console.Extensions;
using Lamar;
using Microsoft.Extensions.DependencyInjection;

return await new CommandAppBuilder()
                 .WithLamarContainer(lifetime: ServiceLifetime.Singleton)
                 .WithDefaultCommand<GreetCommand>()
                 .Build()
                 .RunAsync(args);
```

### LightInject

`WithLightInjectContainer` accepts an optional default `ServiceLifetime` (defaults to `Singleton`):

```csharp
using D20Tek.Spectre.Console.Extensions;
using Microsoft.Extensions.DependencyInjection;

return await new CommandAppBuilder()
                 .WithLightInjectContainer(lifetime: ServiceLifetime.Singleton)
                 .WithDefaultCommand<GreetCommand>()
                 .Build()
                 .RunAsync(args);
```

### Ninject

```csharp
using D20Tek.Spectre.Console.Extensions;

return await new CommandAppBuilder()
                 .WithNinjectContainer()
                 .WithDefaultCommand<GreetCommand>()
                 .Build()
                 .RunAsync(args);
```

### Providing a pre-configured container

Each hook accepts an existing container so you can register your own services before Spectre.Console adds its command types:

```csharp
using Autofac;
using D20Tek.Spectre.Console.Extensions;

var container = new ContainerBuilder();
container.RegisterType<GreetingService>().As<IGreetingService>().SingleInstance();

return await new CommandAppBuilder()
                 .WithAutofacContainer(container)
                 .WithDefaultCommand<GreetCommand>()
                 .Build()
                 .RunAsync(args);
```

## Public API

- `CommandAppBuilderExtensions.WithAutofacContainer(this CommandAppBuilder, ContainerBuilder?)` - uses an Autofac `ContainerBuilder` as the type registrar.
- `CommandAppBuilderExtensions.WithLamarContainer(this CommandAppBuilder, ServiceRegistry?, ServiceLifetime)` - uses a Lamar `ServiceRegistry` as the type registrar.
- `CommandAppBuilderExtensions.WithLightInjectContainer(this CommandAppBuilder, ServiceContainer?, ServiceLifetime)` - uses a LightInject `ServiceContainer` as the type registrar.
- `CommandAppBuilderExtensions.WithNinjectContainer(this CommandAppBuilder, StandardKernel?)` - uses a Ninject `StandardKernel` as the type registrar.
- `AutofacTypeRegistrar` / `AutofacTypeResolver`, `LamarTypeRegistrar` / `LamarTypeResolver`, `LightInjectTypeRegistrar` / `LightInjectTypeResolver`, `NinjectTypeRegistrar` / `NinjectTypeResolver` - the container-specific bridge types.

## Samples

For runnable examples, see the container-specific samples in the repository:

- [Autofac.Cli](https://github.com/d20Tek/Spectre.Console.Extensions/tree/main/samples/Autofac.Cli)
- [Lamar.Cli](https://github.com/d20Tek/Spectre.Console.Extensions/tree/main/samples/Lamar.Cli)
- [LightInject.Cli](https://github.com/d20Tek/Spectre.Console.Extensions/tree/main/samples/LightInject.Cli)
- [Ninject.Cli](https://github.com/d20Tek/Spectre.Console.Extensions/tree/main/samples/Ninject.Cli)

## Feedback

If you have any feedback, questions, or issues, please open an issue on the [GitHub repository](https://github.com/d20Tek/Spectre.Console.Extensions).
