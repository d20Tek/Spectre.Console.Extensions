# D20Tek.Spectre.Console.Extensions.Hosting

`D20Tek.Spectre.Console.Extensions.Hosting` bridges [Spectre.Console.Cli](https://spectreconsole.net/cli/) to the .NET Generic Host (`Microsoft.Extensions.Hosting`). It lets the host own configuration, options binding, logging, hosted services, and application lifetime, while your command types resolve from the host's service provider.

This is a separate package that references the core `D20Tek.Spectre.Console.Extensions` package. It keeps the `Microsoft.Extensions.Hosting` dependency out of the core package, consistent with the other add-on packages in this library.

## Why use the Generic Host?

The core library ships a lean `CommandAppBuilder` that is ideal for small, self-contained CLI tools. The Generic Host is the standard .NET app-composition model, and this package is a sibling path for teams that want the full .NET application stack:

- Layered configuration defaults (`appsettings.json`, environment variables, command-line, user secrets) with no bespoke wiring.
- Options binding and validation through `IOptions<T>`, `IOptionsSnapshot<T>`, and `IOptionsMonitor<T>`.
- Logging providers configured through `ILoggingBuilder`, injected as `ILogger<T>`.
- Hosted services (`IHostedService`), host lifetime, and graceful shutdown.
- Reuse of existing service registrations shared with the rest of a larger application.

It complements the core `CommandAppBuilder` rather than replacing it.

## Installation

```shell
dotnet add package D20Tek.Spectre.Console.Extensions.Hosting
```

## How it works

Spectre.Console.Cli registers its own types (command types, `IAnsiConsole`, its configuration) through an `ITypeRegistrar` when the app runs, which happens *after* the host has already been built. Because the host's `IServiceProvider` is immutable at that point, this package does not try to mutate it. Instead:

- `HostTypeRegistrar` accepts Spectre's run-time `Register` / `RegisterInstance` / `RegisterLazy` calls into an internal registration map. It does not throw after the host is built.
- `HostTypeResolver` fuses the two sources. It first tries `host.Services.GetService(type)`. If that returns `null` and the type is in the registration map, it constructs the instance with `ActivatorUtilities.CreateInstance(compositeProvider, implementationType)`, so command constructor dependencies (`IOptions<T>`, `ILogger<T>`, `IConfiguration`, and your own services) are injected from the host's provider. The composite provider resolves host services first and falls back to the registration map, so a Spectre-registered type can also depend on another Spectre-registered type. Instance and factory registrations are honored directly from the map.

The result is that command types Spectre discovers at run time are still fully constructor-injected from the host container.

## Usage

### Fluent builder

Build a standard host, then call `CreateCommandAppBuilder` to get a fluent builder that mirrors the core `CommandAppBuilder`:

```csharp
using D20Tek.Spectre.Console.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<GreetingOptions>(
    builder.Configuration.GetSection(GreetingOptions.SectionName));
builder.Services.AddSingleton(AnsiConsole.Console);

var host = builder.Build();

return await host.CreateCommandAppBuilder()
                 .WithDefaultCommand<GreetCommand>()
                 .RunAsync(args);
```

You can also register named commands through `ConfigureCommands`:

```csharp
return await host.CreateCommandAppBuilder()
                 .ConfigureCommands(config =>
                 {
                     config.AddCommand<GreetCommand>("greet");
                     config.AddCommand<FarewellCommand>("bye");
                 })
                 .RunAsync(args);
```

`Build` is optional. `RunAsync` and `Run` call it automatically when the app has not been built yet, so you can call `Build` explicitly only when you want to inspect or reuse the configured app.

### IHost extension methods

For lower-level control, the package also provides extension methods directly on `IHost`:

```csharp
using D20Tek.Spectre.Console.Extensions.Hosting;

var host = Host.CreateApplicationBuilder(args).Build();

// Async
var exitCode = await host.RunCommandAppAsync(
    args,
    config => config.AddCommand<GreetCommand>("greet"));

// Synchronous
var exitCode = host.RunCommandApp(
    args,
    config => config.AddCommand<GreetCommand>("greet"));

// Create the CommandApp without running it
var app = host.CreateCommandApp(config => config.AddCommand<GreetCommand>("greet"));
```

### Using the IHostBuilder style

The same wiring works with the classic `Host.CreateDefaultBuilder` / `IHostBuilder` model:

```csharp
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<GreetingOptions>(
            context.Configuration.GetSection(GreetingOptions.SectionName));
        services.AddSingleton(AnsiConsole.Console);
    })
    .Build();

return await host.CreateCommandAppBuilder()
                 .WithDefaultCommand<GreetCommand>()
                 .RunAsync(args);
```

### Injecting host services into a command

Because command types resolve from the host's service provider, they can inject anything the host registered, exactly like a regular hosted class:

```csharp
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

internal sealed class GreetCommand(
    IOptions<GreetingOptions> options,
    IAnsiConsole console,
    ILogger<GreetCommand> logger)
    : Command<GreetCommand.Settings>
{
    private readonly GreetingOptions _options = options.Value;
    private readonly IAnsiConsole _console = console;
    private readonly ILogger<GreetCommand> _logger = logger;

    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "[NAME]")]
        [Description("The name to greet.")]
        [DefaultValue("world")]
        public string Name { get; set; } = "world";
    }

    protected override int Execute(
        CommandContext context,
        Settings settings,
        CancellationToken cancellation)
    {
        _logger.LogInformation("Greeting {Name}.", settings.Name);
        _console.MarkupLineInterpolated(
            $"[green]{_options.Message}[/], [yellow]{settings.Name}[/]{_options.Punctuation}");
        return 0;
    }
}
```

Command-line `CommandSettings` remain separate from host-driven configuration and options, so each command decides precedence explicitly.

### Organizing setup with a startup class

For larger apps, `HostStartupBase` keeps service registration and command configuration in one reusable place. Because the host owns the container and is immutable once built, the startup's two responsibilities run in different phases: `ConfigureServices` runs before the host is built (against the host's `IServiceCollection`), and `ConfigureCommands` runs after the host is built (when the CommandApp is created).

```csharp
using D20Tek.Spectre.Console.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

internal sealed class AppStartup : HostStartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IGreetingService, GreetingService>();
    }

    public override IConfigurator ConfigureCommands(IConfigurator config)
    {
        config.AddCommand<GreetCommand>("greet");
        return config;
    }
}
```

Register it on the host builder with `WithStartup<TStartup>()`. Its `ConfigureServices` runs immediately, and its `ConfigureCommands` is applied automatically when the CommandApp is built:

```csharp
var builder = Host.CreateApplicationBuilder(args);
builder.WithStartup<AppStartup>();

var host = builder.Build();

return await host.CreateCommandAppBuilder()
                 .RunAsync(args);
```

`WithStartup` works with the `IHost` extension methods too, and you can still add more commands through `ConfigureCommands` or the `configure` delegate; startup commands are applied first.

## Public API

- `HostCommandAppExtensions.CreateCommandAppBuilder(this IHost)` - creates a fluent `HostCommandAppBuilder`.
- `HostCommandAppExtensions.CreateCommandApp(this IHost, Action<IConfigurator>)` - creates a configured `CommandApp` without running it.
- `HostCommandAppExtensions.RunCommandAppAsync(this IHost, string[], Action<IConfigurator>)` - creates and runs a `CommandApp` asynchronously.
- `HostCommandAppExtensions.RunCommandApp(this IHost, string[], Action<IConfigurator>)` - creates and runs a `CommandApp` synchronously.
- `HostCommandAppBuilder` - fluent builder with `WithDefaultCommand<T>`, `ConfigureCommands`, `Build`, `RunAsync`, and `Run`.
- `HostStartupBase` - host-aware startup base with `ConfigureServices(IServiceCollection)` (pre-build) and `ConfigureCommands(IConfigurator)` (post-build).
- `HostStartupExtensions.WithStartup<TStartup>(this IHostApplicationBuilder)` - registers a `HostStartupBase` and runs its `ConfigureServices` pre-build.
- `HostTypeRegistrar` / `HostTypeResolver` / `HostRegistration` - the bridge types that capture Spectre's run-time registrations and resolve them from the host provider.

## Sample

See the [GenericHost.Cli](https://github.com/d20Tek/Spectre.Console.Extensions/tree/main/samples/GenericHost.Cli) sample for a complete, runnable example that binds configuration, injects `IOptions<T>`, `IAnsiConsole`, and `ILogger<T>`, and runs a default command through the Generic Host.

## Feedback

If you have any feedback, questions, or issues, please open an issue on the [GitHub repository](https://github.com/d20Tek/Spectre.Console.Extensions).
