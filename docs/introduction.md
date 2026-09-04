# Introducing D20Tek.Spectre.Console.Extensions

D20Tek.Spectre.Console.Extensions is a family of packages that removes the repetitive plumbing you write around a real Spectre.Console.Cli application. A command-line tool is rarely just a set of commands: it needs dependency injection, a predictable startup sequence, configuration binding, logging that respects a verbosity flag, richer prompts, and a way to test the whole thing end to end. Spectre gives you the command model and the extension points; this library gives you the wiring that most applications end up writing by hand. The idea is simple, but doing it well and consistently across every project is where the time goes.

[Spectre.Console](https://github.com/spectreconsole/spectre.console) is one of the best things to happen to .NET command-line development in years. Its `CommandApp` model, rich prompts, tables, and styling turn ordinary console programs into polished, testable tools. This library exists only because the Spectre.Console team did such a great job on the foundation, and because they deliberately designed the framework to be extended through public abstractions like `ITypeRegistrar`, `ITypeResolver`, and `IPrompt<T>`. A sincere thank you to the Spectre.Console maintainers and contributors: D20Tek.Spectre.Console.Extensions is possible only because of your framework, and everything here is meant to complement it, not replace it.

## What these packages do

The library is organized as a small core package plus focused add-ons, so you only take on the dependencies you actually use.

The **core package** (`D20Tek.Spectre.Console.Extensions`) provides a fluent `CommandAppBuilder` that creates, configures, and runs a `CommandApp`, and a `StartupBase` class that cleanly separates service registration (`ConfigureServices`) from command configuration (`ConfigureCommands`). It integrates `Microsoft.Extensions.DependencyInjection` through purpose-built `ITypeRegistrar` and `ITypeResolver` implementations, with lifetime-aware registration helpers. It adds verbosity-aware logging that renders through Spectre's `IAnsiConsole` and maps a shared `VerbosityLevel` onto the standard `LogLevel`, plus an `IVerbosityWriter` service and a `VerbositySettings` base class so users can dial output up or down with a single option. 

It also ships extra controls: a culture-aware `CurrencyPrompt` and `CurrencyPresenter`, a history-enabled `HistoryTextPrompt<T>` with arrow-key recall and tab completion, and table helpers. 

Finally, a `Testing` namespace provides context classes and an end-to-end runner that capture console output and exit codes so commands are straightforward to unit test.

The **add-on packages** extend the same builder without adding weight to the core:

- `D20Tek.Spectre.Console.Extensions.Configuration` adds `Microsoft.Extensions.Configuration` and strongly typed options binding through `WithConfiguration` and `WithOptions<TOptions>`.
- `D20Tek.Spectre.Console.Extensions.Hosting` bridges Spectre.Console.Cli to the .NET Generic Host, so command types resolve from the host's service provider while Spectre-registered types still work, and adds a host-aware `HostStartupBase`.
- `D20Tek.Spectre.Console.Extensions.MoreContainers` adds `ITypeRegistrar`/`ITypeResolver` support for Autofac, Lamar, LightInject, and Ninject, so teams already invested in one of those containers can keep using it.

## Why not just wire it up yourself

Everything this library does is possible with Spectre.Console.Cli directly. Spectre exposes `ITypeRegistrar` and `ITypeResolver` precisely so that you can plug in a container of your choice, and you can absolutely hand-write that bridge, build your own startup convention, add a logging provider, and stand up test harnesses per project. The question is whether you want to write and maintain that plumbing repeatedly. I know I set this up a few times for some CLI apps and quickly started building this library because I was tired of repeating the same patterns and making the same mistakes.

The bridge between a DI container and Spectre's registrar/resolver contract is easy to get subtly wrong, especially around lifetimes and instance registration. Startup ordering matters: services must be configured before the `CommandApp` is created, and commands after. Logging should honor the same verbosity the user requested rather than a separate switch. Configuration and options binding follow a well-known pattern that is tedious to repeat. And testing a CLI usually means capturing console output and exit codes through a fake console. This library encodes those decisions once, as a small set of composable methods, so each new application starts from working infrastructure instead of a blank `Program.cs`. It never hides Spectre from you: you still author `Command`/`AsyncCommand` classes and settings exactly as the framework intends.

## Problems it solves

**Repetitive DI bridging.** `WithDIContainer` and the container-specific extensions build the correct `ITypeRegistrar` for you, so you never hand-write the Spectre registrar/resolver bridge or its lifetime handling. The core package depends only on `Microsoft.Extensions.DependencyInjection`; other containers are additive through MoreContainers.

**Scattered startup code.** `StartupBase` keeps `ConfigureServices` and `ConfigureCommands` together in one class, and `CommandAppBuilder.Build` invokes them in the correct order, so the setup sequence is consistent across every project.

**Logging that ignores verbosity.** `WithLogging` maps the user's requested `VerbosityLevel` to a minimum `LogLevel` and renders entries through the same `IAnsiConsole` as the rest of your output, so `--verbosity detailed` actually surfaces Debug and Trace messages.

**Configuration without ceremony.** `WithConfiguration` and `WithOptions<TOptions>` add `IConfiguration` and validated `IOptions<T>` to the container with a single call each, following the standard configuration and data-annotations validation patterns.

**Host integration.** The Hosting package bridges Spectre.Console.Cli to the Generic Host, so command types resolve from the host container, Spectre-registered types continue to work through a composite provider, and a host-aware `HostStartupBase` splits pre-build service registration from post-build command configuration.

**Plain prompts.** The extra controls fill common gaps: culture-aware currency input and display, and a history-enabled text prompt with recall and auto-completion.

**Hard-to-test CLIs.** `CommandAppTestContext`, `CommandAppBuilderTestContext`, and `CommandAppE2ERunner` drive an app through a fake console and expose the exit code and captured output, so you can assert on real behavior without spawning a process.

## What it does not try to do

This library is a set of builders, extensions, and helpers, not a framework that takes ownership of your application. It does not replace Spectre.Console.Cli or hide its command model; you continue to write your commands, settings, and configurators against Spectre directly, and you can drop down to the raw `CommandApp` at any time. It does not impose a container choice: the core package stays lean with a single DI dependency, and alternative containers are opt-in. It also does not add features that belong to Spectre itself; when the framework already does something well, this library gets out of the way and lets you use it.

## Getting started

The packages target .NET 9.0 and .NET 10.0. Install the core package, and add only the optional packages you need:

```
PM > Install-Package D20Tek.Spectre.Console.Extensions
PM > Install-Package D20Tek.Spectre.Console.Extensions.Configuration
PM > Install-Package D20Tek.Spectre.Console.Extensions.Hosting
PM > Install-Package D20Tek.Spectre.Console.Extensions.MoreContainers
```

Create a `Program.cs` that builds and runs a `CommandApp` through the fluent builder:

```csharp
using D20Tek.Spectre.Console.Extensions;

namespace MyCli;

public static class Program
{
	public static Task<int> Main(string[] args) =>
		new CommandAppBuilder()
			.WithDIContainer()
			.WithStartup<Startup>()
			.WithDefaultCommand<GreetCommand>()
			.Build()
			.RunAsync(args);
}
```

Put service registration and command configuration in a `StartupBase` class:

```csharp
using D20Tek.Spectre.Console.Extensions;
using Spectre.Console.Cli;

public sealed class Startup : StartupBase
{
	public override void ConfigureServices(ITypeRegistrar registrar) =>
		registrar.WithLifetimes()
				 .RegisterSingleton<IGreetingService, GreetingService>();

	public override IConfigurator ConfigureCommands(IConfigurator config)
	{
		config.AddCommand<GreetCommand>("greet");
		return config;
	}
}
```

Then author commands as usual for Spectre.Console.Cli, taking dependencies through the constructor:

```csharp
using Spectre.Console;
using Spectre.Console.Cli;

public sealed class GreetCommand : Command
{
	private readonly IGreetingService _greetings;

	public GreetCommand(IGreetingService greetings) => _greetings = greetings;

	protected override int Execute(CommandContext context)
	{
		AnsiConsole.MarkupLine(_greetings.Greet());
		return 0;
	}
}
```

That is the entire setup. From here you can layer in configuration binding, verbosity-aware logging, Generic Host integration, an alternative DI container, or the extra prompt controls, each through a single additional call. The [Getting Started](getting-started-detailed.md) guide walks through these in order, and the targeted [guides](getting-started-detailed.md#guides) cover each feature in depth.

## Links

- **Getting Started:** [End-to-end walkthrough](getting-started-detailed.md)
- **API Reference:** [Complete API Reference](api-reference.md)
- **Changelog:** [Release history](../CHANGELOG.md)
- **Source and samples:** [GitHub repository](https://github.com/d20Tek/Spectre.Console.Extensions)
- **Spectre.Console:** [The framework this library extends](https://github.com/spectreconsole/spectre.console)
