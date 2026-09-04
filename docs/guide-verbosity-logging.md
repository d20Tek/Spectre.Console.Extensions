# Guide: Verbosity and Logging

The core package ties output verbosity and logging together: a shared `VerbosityLevel` controls both how much output an application emits through `IVerbosityWriter` and the minimum level of logging that renders through Spectre's `IAnsiConsole`.

## Verbosity Levels and Output

Many CLI applications let users control how much output they see. The core package provides a `VerbosityLevel` enum, a `VerbositySettings` base class for command settings, and an `IVerbosityWriter` service that writes or marks up text only when the current verbosity allows it.

### Accept a verbosity option

Derive your command settings from `VerbositySettings` to add a `--verbosity` option that binds to the `Verbosity` property:

```csharp
public sealed class GreetSettings : VerbositySettings
{
	[CommandArgument(0, "<name>")]
	public string Name { get; init; } = string.Empty;
}
```

The `Verbosity` property defaults to `VerbosityLevel.Normal`.

### Write verbosity-aware output

Register and inject `IVerbosityWriter` (the default implementation is `ConsoleVerbosityWriter`) to emit messages that respect the configured verbosity. Set the writer's `Verbosity` from the command settings, then call the level-specific methods:

```csharp
public sealed class GreetCommand : Command<GreetSettings>
{
	private readonly IVerbosityWriter _writer;

	public GreetCommand(IVerbosityWriter writer) => _writer = writer;

	protected override int Execute(CommandContext context, GreetSettings settings)
	{
		_writer.Verbosity = settings.Verbosity;

		_writer.WriteSummary("Starting.");        // shown at Minimal and above
		_writer.MarkupNormal($"Hello, [green]{settings.Name}[/]!");
		_writer.WriteDetailed("Extra detail.");    // shown at Detailed and above
		_writer.WriteDiagnostics("Diagnostics.");  // shown at Diagnostic

		return 0;
	}
}
```

Each level has a plain-text `Write*` method and a Spectre markup `Markup*` method:

- `WriteSummary` / `MarkupSummary` (Minimal)
- `WriteNormal` / `MarkupNormal` (Normal)
- `WriteDetailed` / `MarkupDetailed` (Detailed)
- `WriteDiagnostics` / `MarkupDiagnostics` (Diagnostic)

A message is emitted only when the writer's `Verbosity` is at or above the message's level.

## Verbosity-Aware Logging

The same `VerbosityLevel` drives logging that renders through Spectre's `IAnsiConsole` and derives its minimum log level from the requested verbosity.

### Enable logging

Call `WithLogging` on the builder after configuring a DI container. It registers the Spectre console logger provider in the container:

```csharp
using D20Tek.Spectre.Console.Extensions;

var builder = new CommandAppBuilder()
	.WithDIContainer()
	.WithLogging(minimumVerbosity: VerbosityLevel.Normal)
	.WithStartup<Startup>();
```

`WithLogging` requires a container that supports lifetimes, so call `WithDIContainer` (or a container extension) first. It throws `InvalidOperationException` otherwise.

### Configure rendering

Pass a configuration delegate to control how entries are rendered through `SpectreConsoleLoggerOptions`:

```csharp
builder.WithLogging(
	minimumVerbosity: VerbosityLevel.Detailed,
	configure: options =>
	{
		options.IncludeLevelLabel = true;
		options.IncludeCategory = true;
		options.IncludeTimestamp = true;
		options.TimestampFormat = "HH:mm:ss";
	});
```

You can also pass a specific `IAnsiConsole` to render to; when omitted, `AnsiConsole.Console` is used.

### Inject loggers into commands

Once logging is enabled, inject `ILogger<T>` into your commands like any other service:

```csharp
public sealed class GreetCommand : AsyncCommand
{
	private readonly ILogger<GreetCommand> _logger;

	public GreetCommand(ILogger<GreetCommand> logger) => _logger = logger;

	protected override Task<int> ExecuteAsync(CommandContext context)
	{
		_logger.LogInformation("Greeting the user.");
		return Task.FromResult(0);
	}
}
```

### Verbosity to log level mapping

The minimum verbosity is mapped to a `LogLevel` through `VerbosityLevelExtensions.ToLogLevel`, so a more detailed verbosity emits Debug and Trace entries. This is the same `VerbosityLevel` that `VerbositySettings` and `IVerbosityWriter` use, so a single option can control both console output and log level.

## Related

- [API Reference: Core](api-reference-core.md#verbosity-output)
