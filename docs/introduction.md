# Building Better Spectre.Console Applications with Spectre.Console.Extensions

Spectre.Console has become a popular choice for building modern .NET command‑line applications. It provides a rich rendering engine, a clean command model, and a set of primitives that make CLI development feel productive and expressive. But once you move beyond small utilities and start building real applications, you quickly run into the same set of challenges: dependency injection, configuration, command registration, and testing.

Spectre.Console gives you the foundation, but it intentionally avoids prescribing an application architecture. That flexibility is valuable, but it also means developers often end up writing the same boilerplate over and over again. Every project needs a DI registrar. Every project needs a way to bind configuration. Every project needs a pattern for wiring commands. And every project eventually needs a way to test those commands.

Spectre.Console.Extensions exists to solve those problems.

It provides a set of patterns, helpers, and integrations that make it easier to build structured, testable, maintainable Spectre.Console applications. The goal is not to replace Spectre.Console’s design, but to complement it with a consistent application model that scales as your CLI grows.

---

## Why This Library Exists

When building Spectre.Console applications, several architectural questions come up repeatedly:

- How should commands be registered?
- How should services be wired into commands?
- How should configuration be loaded and bound?
- How should commands be tested without manually constructing the entire application?
- How can multiple DI containers be supported without rewriting the same registrar logic?

Spectre.Console.Extensions provides answers to these questions by offering a unified approach to application setup. Instead of hand‑rolling DI registrars or building custom test harnesses, you can rely on a set of extensions that handle these concerns consistently across projects.

The library is built around a few core ideas:

### 1. A predictable application startup model  
Command‑line applications benefit from the same structure that web and desktop applications use: a startup pipeline that configures services, loads configuration, registers commands, and builds the final application. The `CommandAppBuilder` type provides this structure without hiding Spectre.Console’s underlying model.

### 2. First‑class dependency injection  
Spectre.Console supports DI through its `ITypeRegistrar` abstraction, but developers still need to implement the registrar themselves. This library provides ready‑made DI integrations for Microsoft.Extensions.DependencyInjection and several other containers, so you can use the DI system you already rely on in your other .NET applications.

### 3. Configuration binding  
Real applications need configuration. This library integrates Spectre.Console settings with Microsoft.Extensions.Configuration, allowing command settings to be bound automatically from configuration sources.

### 4. Testability  
Spectre.Console applications can be difficult to test because commands are typically executed through the full `CommandApp` pipeline. The library provides a test context that makes it possible to run commands, capture output, and assert results without manually wiring the application.

### 5. Reusable CLI components  
Many CLI applications need prompts, validation, and common interaction patterns. The library includes additional controls and helpers that simplify these scenarios.

---

## What the Library Provides

Spectre.Console.Extensions includes several focused components:

- **CommandAppBuilder**: A fluent builder for configuring DI, commands, settings, and configuration.
- **DI Container Integrations**: Support for Microsoft.Extensions.DependencyInjection, Autofac, Lamar, LightInject, and Ninject.
- **Configuration Binding**: Automatic binding of command settings from configuration sources.
- **Testing Infrastructure**: A test harness for running commands and capturing output.
- **Reusable Controls**: Additional prompts and helpers for common CLI workflows.
- **Sample Applications**: Practical examples demonstrating how to structure real Spectre.Console applications.

Each component is designed to be optional. You can adopt the parts that fit your project without committing to a rigid framework.

---

## A More Structured Way to Build CLI Applications

The goal of Spectre.Console.Extensions is not to change how Spectre.Console works, but to give developers a consistent, scalable way to build applications on top of it. If you’ve ever built a Spectre.Console application and found yourself rewriting DI registrars, configuration loaders, or test harnesses, this library is meant to save you that effort.

It provides a foundation that feels familiar to .NET developers: dependency injection, configuration, and testing integrated into a clean application startup model. With these pieces in place, you can focus on building commands and features instead of wiring infrastructure.

If you’re building Spectre.Console applications that need structure, testability, or integration with the broader .NET ecosystem, Spectre.Console.Extensions gives you the tools to do it cleanly and consistently.

---

## What it does not try to do

This library is a set of builders, extensions, and helpers, not a framework that takes ownership of your application. It does not replace Spectre.Console.Cli or hide its command model; you continue to write your commands, settings, and configurators against Spectre directly, and you can drop down to the raw `CommandApp` at any time. It does not impose a container choice: the core package stays lean with a single DI dependency, and alternative containers are opt-in. It also does not add features that belong to Spectre itself; when the framework already does something well, this library gets out of the way and lets you use it.

---

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
