# Guide: Using Additional DI Containers

The core package uses `Microsoft.Extensions.DependencyInjection`. The `D20Tek.Spectre.Console.Extensions.MoreContainers` package adds `ITypeRegistrar`/`ITypeResolver` support for Autofac, Lamar, LightInject, and Ninject, so you can keep the core package's dependencies minimal and only pull in another container when you need it.

## Choose a container

Each container has a `CommandAppBuilder` extension that configures the matching registrar. Call it in place of `WithDIContainer`:

```csharp
using D20Tek.Spectre.Console.Extensions;

// Ninject
new CommandAppBuilder().WithNinjectContainer();

// Autofac
new CommandAppBuilder().WithAutofacContainer();

// LightInject
new CommandAppBuilder().WithLightInjectContainer();

// Lamar
new CommandAppBuilder().WithLamarContainer();
```

## Supply a pre-populated container

Each extension accepts an optional, pre-configured container instance so you can register services with the container's native API first:

```csharp
var kernel = new StandardKernel();
kernel.Bind<IGreetingService>().To<GreetingService>();

var builder = new CommandAppBuilder()
	.WithNinjectContainer(kernel)
	.WithStartup<Startup>();
```

- `WithNinjectContainer(StandardKernel?)`
- `WithAutofacContainer(ContainerBuilder?)`
- `WithLightInjectContainer(ServiceContainer?, ServiceLifetime)`
- `WithLamarContainer(ServiceRegistry?, ServiceLifetime)`

The LightInject and Lamar extensions also accept a default `ServiceLifetime` applied to `Register` calls.

## Register services

After selecting a container, register services in your `StartupBase.ConfigureServices` through the `ITypeRegistrar`, or configure the native container instance before passing it in. Commands resolve from the chosen container the same way regardless of the framework.

## Related

- [Dependency Injection and Lifetimes](guide-dependency-injection.md)
- [API Reference: MoreContainers](api-reference-morecontainers.md)
