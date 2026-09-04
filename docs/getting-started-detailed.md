# Getting Started

This guide walks you from an empty project to a running Spectre.Console CLI built with D20Tek.Spectre.Console.Extensions. It covers installation, the `CommandAppBuilder`, a `StartupBase` class, dependency injection, and running the app. Deeper topics are covered in the targeted [guides](#guides) at the end.

## Installation

The library ships as NuGet packages. Install the core package, and add the optional packages you need:

```
PM > Install-Package D20Tek.Spectre.Console.Extensions
PM > Install-Package D20Tek.Spectre.Console.Extensions.Configuration
PM > Install-Package D20Tek.Spectre.Console.Extensions.Hosting
PM > Install-Package D20Tek.Spectre.Console.Extensions.MoreContainers
```

The core package depends only on `Microsoft.Extensions.DependencyInjection`. The other packages are additive: install them only when you need configuration binding, Generic Host integration, or an alternative DI container.

## Build a CommandApp with CommandAppBuilder

`CommandAppBuilder` is the recommended entry point. It creates the `CommandApp`, wires up the DI container, and runs your commands. A minimal `Program.cs` looks like this:

```csharp
using D20Tek.Spectre.Console.Extensions;

namespace MyCli;

public static class Program
{
	public static Task<int> Main(string[] args) =>
		new CommandAppBuilder()
			.WithDIContainer()
			.WithStartup<Startup>()
			.WithDefaultCommand<DefaultCommand>()
			.Build()
			.RunAsync(args);
}
```

- `WithDIContainer` configures the `Microsoft.Extensions.DependencyInjection` registrar.
- `WithStartup<TStartup>` registers your startup class.
- `WithDefaultCommand<TDefault>` sets the command that runs when no command name is supplied.
- `Build` creates the `CommandApp` and applies your service and command configuration.
- `RunAsync` (or `Run`) executes the app and returns its exit code.

## Configure services and commands with StartupBase

Derive from `StartupBase` to keep service registration and command configuration together:

```csharp
using D20Tek.Spectre.Console.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

public sealed class Startup : StartupBase
{
	public override void ConfigureServices(ITypeRegistrar registrar)
	{
		registrar.WithLifetimes()
				 .RegisterSingleton<IGreetingService, GreetingService>();
	}

	public override IConfigurator ConfigureCommands(IConfigurator config)
	{
		config.AddCommand<GreetCommand>("greet");
		return config;
	}
}
```

`ConfigureServices` runs against the type registrar during `Build`, and `ConfigureCommands` runs against the Spectre `IConfigurator`. Your commands can then take dependencies through their constructors, resolved from the container.

## Run the app

With the builder configured, `RunAsync(args)` executes the app and returns the process exit code. Pass that value back from `Main` so the CLI reports the correct status to the shell.

## Next steps

Once the basics work, layer in the features you need. Each targeted guide is focused on a single task, and the [API Reference](api-reference.md) documents the complete public surface.

## Guides

- [Building CommandApps with CommandAppBuilder](guide-command-app-builder.md)
- [Dependency Injection and Lifetimes](guide-dependency-injection.md)
- [Verbosity and Logging](guide-verbosity-logging.md)
- [Controls](guide-controls.md)
- [Testing CLI Applications](guide-testing-cli-apps.md)
- [Configuration and Options Binding](guide-configuration.md)
- [Generic Host Integration](guide-generic-host.md)
- [Using Additional DI Containers](guide-more-containers.md)
