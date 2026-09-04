# Guide: Dependency Injection and Lifetimes

The core package integrates Spectre.Console.Cli with `Microsoft.Extensions.DependencyInjection` through the `DependencyInjectionTypeRegistrar` and `DependencyInjectionTypeResolver`. This guide shows how to register services and control their lifetimes.

## Configure the container

Call `WithDIContainer` on the builder to set up the DI registrar. You can pass an existing `IServiceCollection` and the default `ServiceLifetime` used by the registrar's `Register` calls:

```csharp
var builder = new CommandAppBuilder()
	.WithDIContainer(lifetime: ServiceLifetime.Singleton);
```

## Register services in a startup class

Inside `StartupBase.ConfigureServices`, use the type registrar to register your services. The `WithLifetimes` extension exposes lifetime-aware registration helpers:

```csharp
public override void ConfigureServices(ITypeRegistrar registrar)
{
	registrar.WithLifetimes()
			 .RegisterSingleton<IGreetingService, GreetingService>()
			 .RegisterScoped<IUnitOfWork, UnitOfWork>()
			 .RegisterTransient<IReport, DailyReport>();
}
```

The lifetime helpers available on `ISupportLifetimes` are:

- `RegisterSingleton<TService, TImplementation>()` and `RegisterSingleton<TService>(TService instance)`
- `RegisterScoped<TService, TImplementation>()`
- `RegisterTransient<TService, TImplementation>()`

## Resolve services in commands

Commands and their dependencies are resolved from the container. Constructor-inject the services your command needs, and Spectre resolves them through the `DependencyInjectionTypeResolver`:

```csharp
public sealed class GreetCommand : AsyncCommand
{
	private readonly IGreetingService _greetings;

	public GreetCommand(IGreetingService greetings) => _greetings = greetings;

	protected override Task<int> ExecuteAsync(CommandContext context) =>
		Task.FromResult(_greetings.Greet());
}
```

## Access the service collection directly

When you need to register services outside a startup class (for example from an extension method), call `CommandAppBuilder.GetServiceCollection` to get the underlying `IServiceCollection`.

## Related

- [Using Additional DI Containers](guide-more-containers.md)
- [API Reference: Core](api-reference-core.md#dependency-injection)
