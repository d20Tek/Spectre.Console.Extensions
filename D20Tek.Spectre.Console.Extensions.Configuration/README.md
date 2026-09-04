[![NuGet](https://img.shields.io/nuget/v/D20Tek.Spectre.Console.Extensions.Configuration.svg)](https://www.nuget.org/packages/D20Tek.Spectre.Console.Extensions.Configuration)

# D20Tek.Spectre.Console.Extensions.Configuration

`D20Tek.Spectre.Console.Extensions.Configuration` adds [Microsoft.Extensions.Configuration](https://learn.microsoft.com/dotnet/core/extensions/configuration) and Options binding to the core library's `CommandAppBuilder`. Commands can then inject `IConfiguration` or strongly typed `IOptions<T>` through their constructors, alongside the existing dependency injection container.

This is a separate package that references the core `D20Tek.Spectre.Console.Extensions` package. It keeps the `Microsoft.Extensions.Configuration` dependencies out of the core package, consistent with the other add-on packages in this library.

## Why a separate package?

The core library ships a lean `CommandAppBuilder` with a minimal dependency footprint. Configuration and Options binding are opt-in concerns that not every CLI tool needs, so they live in this add-on package. Add it only when you want layered configuration sources and validated, strongly typed options.

## Installation

```shell
dotnet add package D20Tek.Spectre.Console.Extensions.Configuration
```

## How it works

The package extends `CommandAppBuilder` with two fluent hooks that operate on the builder's service collection:

- `WithConfiguration` builds an `IConfiguration` and registers it as a singleton in the DI container. By default it reads from an optional `appsettings.json` file and environment variables. Pass a configure delegate to customize the configuration sources.
- `WithOptions<TOptions>` binds a configuration section to a strongly typed options class, registered so it can be injected as `IOptions<TOptions>`. Data annotations on the options class are validated.

Both hooks require that a DI container has already been configured, for example by calling `WithDIContainer` first. Configuration values remain separate from command-line `CommandSettings`, so each command decides precedence explicitly.

## Usage

### Binding a strongly typed options class

After configuring a DI container, call `WithConfiguration` to build and register an `IConfiguration`, then `WithOptions<T>` to bind a section to a validated options class:

```csharp
using D20Tek.Spectre.Console.Extensions;
using D20Tek.Spectre.Console.Extensions.Configuration;

return await new CommandAppBuilder()
                 .WithDIContainer()
                 .WithConfiguration()
                 .WithOptions<GreetingOptions>(GreetingOptions.SectionName)
                 .WithStartup<Startup>()
                 .WithDefaultCommand<GreetCommand>()
                 .Build()
                 .RunAsync(args);
```

By default `WithConfiguration` reads from an optional `appsettings.json` file and environment variables. `WithOptions<T>` binds the named section and validates any data annotations on the options class. Any command can then inject `IOptions<T>` through its constructor.

### Customizing configuration sources

Pass a configure delegate to `WithConfiguration` to add or replace configuration sources:

```csharp
return await new CommandAppBuilder()
                 .WithDIContainer()
                 .WithConfiguration(config =>
                 {
                     config.SetBasePath(AppContext.BaseDirectory)
                           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                           .AddJsonFile("appsettings.Development.json", optional: true)
                           .AddEnvironmentVariables()
                           .AddCommandLine(args);
                 })
                 .WithDefaultCommand<GreetCommand>()
                 .Build()
                 .RunAsync(args);
```

### Injecting IConfiguration directly

You do not have to bind to a strongly typed options class. A command can inject `IConfiguration` directly and read individual keys or sections:

```csharp
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using Spectre.Console.Cli;

internal sealed class InfoCommand(IConfiguration configuration, IAnsiConsole console) : Command
{
    protected override int Execute(CommandContext context, CancellationToken cancellation)
    {
        var title = configuration["App:Title"];
        var version = configuration.GetValue<string>("App:Version");
        var features = configuration.GetSection("App:Features").Get<string[]>() ?? [];

        console.MarkupLineInterpolated($"[bold]{title}[/] v[yellow]{version}[/]");
        console.MarkupLineInterpolated($"Features: [green]{string.Join(", ", features)}[/]");
        return 0;
    }
}
```

## Public API

- `ConfigurationCommandAppBuilderExtensions.WithConfiguration(this CommandAppBuilder, Action<IConfigurationBuilder>?)` - builds an `IConfiguration` and registers it in the builder's DI container.
- `ConfigurationCommandAppBuilderExtensions.WithOptions<TOptions>(this CommandAppBuilder, string sectionName)` - binds a configuration section to a validated options class for injection as `IOptions<TOptions>`.

## Sample

See the [Configuration.Cli](https://github.com/d20Tek/Spectre.Console.Extensions/tree/main/samples/Configuration.Cli) sample for a complete, runnable example that binds configuration and injects `IOptions<T>` into a command.

## Feedback

If you have any feedback, questions, or issues, please open an issue on the [GitHub repository](https://github.com/d20Tek/Spectre.Console.Extensions).
