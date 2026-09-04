# API Reference: Core

Package: `D20Tek.Spectre.Console.Extensions`

This document covers the full public surface of the core package: the types that create and configure a `CommandApp`, the dependency-injection bridge, verbosity-aware logging, the verbosity output service, and the additional prompt and console controls. The `Testing` namespace is documented separately in [API Reference: Testing](api-reference-test.md).

## Contents

- [Application Building](#application-building)
  - [CommandAppBuilder](#commandappbuilder)
  - [StartupBase](#startupbase)
  - [ConfiguratorExtensions](#configuratorextensions)
  - [ICommandConfiguration](#icommandconfiguration)
- [Dependency Injection](#dependency-injection)
  - [CommandAppBuilderExtensions](#commandappbuilderextensions)
  - [DependencyInjectionTypeRegistrar](#dependencyinjectiontyperegistrar)
  - [DependencyInjectionTypeResolver](#dependencyinjectiontyperesolver)
  - [ISupportLifetimes](#isupportlifetimes)
  - [LifetimeExtensions](#lifetimeextensions)
  - [TypeRegistrarExtensions](#typeregistrarextensions)
- [Logging](#logging)
  - [LoggingCommandAppBuilderExtensions](#loggingcommandappbuilderextensions)
  - [SpectreLoggingExtensions](#spectreloggingextensions)
  - [SpectreConsoleLogger](#spectreconsolelogger)
  - [SpectreConsoleLoggerProvider](#spectreconsoleloggerprovider)
  - [SpectreConsoleLoggerOptions](#spectreconsoleloggeroptions)
  - [VerbosityLevelExtensions](#verbositylevelextensions)
- [Verbosity Output](#verbosity-output)
  - [VerbosityLevel](#verbositylevel)
  - [VerbositySettings](#verbositysettings)
  - [IVerbosityWriter](#iverbositywriter)
  - [ConsoleVerbosityWriter](#consoleverbositywriter)
- [Controls](#controls)
  - [CurrencyPrompt](#currencyprompt)
  - [CurrencyPresenter](#currencypresenter)
  - [HistoryTextPrompt&lt;T&gt;](#historytextpromptt)
  - [HistoryTextPromptExtensions](#historytextpromptextensions)
  - [TableExtensions](#tableextensions)
  - [AnsiConsoleExtensions](#ansiconsoleextensions)

---

## Application Building

Namespace: `D20Tek.Spectre.Console.Extensions`

### CommandAppBuilder

A builder for creating, configuring, and running a `CommandApp`.

| Member | Signature | Description |
|---|---|---|
| `Registrar` | `ITypeRegistrar? Registrar { get; }` | The type registrar configured for this builder, or null if none has been set. Exposed so add-on extension packages can reach the underlying DI container. |
| `GetServiceCollection` | `IServiceCollection GetServiceCollection()` | Gets the registrar's underlying service collection. Throws `InvalidOperationException` when no registrar has been configured. |
| `WithStartup<TStartup>` | `CommandAppBuilder WithStartup<TStartup>()` where `TStartup : StartupBase, new()` | Sets the startup class used to configure services and commands. |
| `SetRegistrar` | `CommandAppBuilder SetRegistrar(ITypeRegistrar registrar)` | Sets a custom type registrar. Throws `ArgumentNullException` when registrar is null. |
| `WithDefaultCommand<TDefault>` | `CommandAppBuilder WithDefaultCommand<TDefault>()` where `TDefault : class, ICommand` | Sets the command that runs when no command name is supplied. |
| `Build` | `CommandAppBuilder Build()` | Configures services, creates the CommandApp, applies the default command, and configures commands. Throws `ArgumentNullException` when no startup was set. |
| `RunAsync` | `Task<int> RunAsync(string[] args)` | Runs the CommandApp asynchronously and returns the exit code. |
| `Run` | `int Run(string[] args)` | Runs the CommandApp synchronously and returns the exit code. |

### StartupBase

Abstract base class for defining a startup class that configures services and commands.

| Member | Signature | Description |
|---|---|---|
| `ConfigureServices` | `abstract void ConfigureServices(ITypeRegistrar registrar)` | Override to register application services in the type registrar. |
| `ConfigureCommands` | `abstract IConfigurator ConfigureCommands(IConfigurator config)` | Override to configure console commands. Returns the configurator that was used. |

### ConfiguratorExtensions

Extension methods for the Spectre.Console.Cli `IConfigurator`.

| Member | Signature | Description |
|---|---|---|
| `ApplyConfiguration` | `static IConfigurator ApplyConfiguration(this IConfigurator configurator, ICommandConfiguration config)` | Applies the specified command configuration instance to the configurator and returns it. |

### ICommandConfiguration

Interface for classes that encapsulate configuration for a grouped set of commands.

| Member | Signature | Description |
|---|---|---|
| `Configure` | `void Configure(IConfigurator config)` | Configures this group's commands on the specified configurator. |

---

## Dependency Injection

Namespace: `D20Tek.Spectre.Console.Extensions.Injection` (the `WithDIContainer` extension is in `D20Tek.Spectre.Console.Extensions`)

### CommandAppBuilderExtensions

Extension methods that supply a DI container to a `CommandAppBuilder`.

| Member | Signature | Description |
|---|---|---|
| `WithDIContainer` | `static CommandAppBuilder WithDIContainer(this CommandAppBuilder builder, IServiceCollection? services = null, ServiceLifetime lifetime = ServiceLifetime.Singleton)` | Configures the `Microsoft.Extensions.DependencyInjection` registrar, optionally with pre-registered services and a default lifetime. |

### DependencyInjectionTypeRegistrar

A sealed `ITypeRegistrar` and `ISupportLifetimes` backed by an `IServiceCollection`.

| Member | Signature | Description |
|---|---|---|
| `Services` | `IServiceCollection Services { get; }` | The underlying service collection. |
| `Build` | `ITypeResolver Build()` | Builds a type resolver over the registered services. |
| `Register` | `void Register(Type service, Type implementation)` | Registers a service and implementation type. |
| `RegisterInstance` | `void RegisterInstance(Type service, object implementation)` | Registers an existing instance for a service type. |
| `RegisterLazy` | `void RegisterLazy(Type service, Func<object> factoryMethod)` | Registers a lazily created instance via a factory. |

### DependencyInjectionTypeResolver

A sealed `ITypeResolver` and `IDisposable` over an `IServiceProvider`.

| Member | Signature | Description |
|---|---|---|
| Constructor | `DependencyInjectionTypeResolver(IServiceProvider provider)` | Creates the resolver over the given provider. |
| `Resolve` | `object? Resolve(Type? type)` | Resolves a service of the requested type. |
| `Dispose` | `void Dispose()` | Disposes the underlying provider scope. |

### ISupportLifetimes

Interface implemented by registrars that support service lifetimes.

### LifetimeExtensions

Lifetime-aware registration helpers on `ISupportLifetimes`.

| Member | Signature | Description |
|---|---|---|
| `RegisterSingleton<TService, TImplementation>` | `static ISupportLifetimes RegisterSingleton<TService, TImplementation>(this ISupportLifetimes registrar)` | Registers a singleton service and implementation. |
| `RegisterSingleton<TService>` | `static ISupportLifetimes RegisterSingleton<TService>(this ISupportLifetimes registrar, TService instance)` | Registers a singleton instance. |
| `RegisterSingleton<TService, TImplementation>` | `static ISupportLifetimes RegisterSingleton<TService, TImplementation>(this ISupportLifetimes registrar, Func<IServiceProvider, TImplementation> implementationFactory)` | Registers a singleton service created by a factory. |
| `RegisterScoped<TService, TImplementation>` | `static ISupportLifetimes RegisterScoped<TService, TImplementation>(this ISupportLifetimes registrar)` | Registers a scoped service and implementation. |
| `RegisterScoped<TService, TImplementation>` | `static ISupportLifetimes RegisterScoped<TService, TImplementation>(this ISupportLifetimes registrar, Func<IServiceProvider, TImplementation> implementationFactory)` | Registers a scoped service created by a factory. |
| `RegisterTransient<TService, TImplementation>` | `static ISupportLifetimes RegisterTransient<TService, TImplementation>(this ISupportLifetimes registrar)` | Registers a transient service and implementation. |
| `RegisterTransient<TService, TImplementation>` | `static ISupportLifetimes RegisterTransient<TService, TImplementation>(this ISupportLifetimes registrar, Func<IServiceProvider, TImplementation> implementationFactory)` | Registers a transient service created by a factory. |

### TypeRegistrarExtensions

| Member | Signature | Description |
|---|---|---|
| `WithLifetimes` | `static ISupportLifetimes WithLifetimes(this ITypeRegistrar registrar)` | Returns the registrar as `ISupportLifetimes` so lifetime helpers can be used. Throws when the registrar does not support lifetimes. |

---

## Logging

Namespaces: `D20Tek.Spectre.Console.Extensions`, `D20Tek.Spectre.Console.Extensions.Logging`

### LoggingCommandAppBuilderExtensions

Extension methods that add verbosity-aware logging to a `CommandAppBuilder`.

| Member | Signature | Description |
|---|---|---|
| `WithLogging` | `static CommandAppBuilder WithLogging(this CommandAppBuilder builder, VerbosityLevel minimumVerbosity = VerbosityLevel.Normal, IAnsiConsole? console = null, Action<SpectreConsoleLoggerOptions>? configure = null)` | Adds logging that renders through an `IAnsiConsole`, with the minimum log level derived from the verbosity. Requires a container that supports lifetimes. Throws `ArgumentNullException` when builder is null and `InvalidOperationException` when no suitable registrar is configured. |

### SpectreLoggingExtensions

Extension methods for `ILoggingBuilder`.

| Member | Signature | Description |
|---|---|---|
| `AddSpectreConsole` | `static ILoggingBuilder AddSpectreConsole(this ILoggingBuilder builder, VerbosityLevel minimumVerbosity, Action<SpectreConsoleLoggerOptions>? configure = null)` | Registers the Spectre console logger provider and sets the builder's minimum level from the mapped verbosity. |

### SpectreConsoleLogger

A sealed `ILogger` that renders log entries through an `IAnsiConsole`.

| Member | Signature | Description |
|---|---|---|
| `BeginScope<TState>` | `IDisposable? BeginScope<TState>(TState state)` where `TState : notnull` | Begins a logical operation scope. Returns null (scopes are not tracked). |
| `IsEnabled` | `bool IsEnabled(LogLevel logLevel)` | Indicates whether the given log level is enabled. |

### SpectreConsoleLoggerProvider

A sealed `ILoggerProvider` that creates `SpectreConsoleLogger` instances.

### SpectreConsoleLoggerOptions

Options controlling how log entries are rendered.

| Member | Signature | Description |
|---|---|---|
| `IncludeLevelLabel` | `bool IncludeLevelLabel { get; set; }` | Whether to include the log level label. Defaults to true. |
| `IncludeCategory` | `bool IncludeCategory { get; set; }` | Whether to include the category name. |
| `IncludeTimestamp` | `bool IncludeTimestamp { get; set; }` | Whether to include a timestamp. |
| `TimestampFormat` | `string TimestampFormat { get; set; }` | The timestamp format string. Defaults to `"HH:mm:ss"`. |

### VerbosityLevelExtensions

Mapping helpers between `VerbosityLevel` and `LogLevel`.

| Member | Signature | Description |
|---|---|---|
| `ToLogLevel` | `static LogLevel ToLogLevel(this VerbosityLevel verbosity)` | Maps a verbosity level to the corresponding minimum log level. |
| `ToVerbosityLevel` | `static VerbosityLevel ToVerbosityLevel(this LogLevel logLevel)` | Maps a log level to the corresponding verbosity level. |

---

## Verbosity Output

Namespaces: `D20Tek.Spectre.Console.Extensions.Settings`, `D20Tek.Spectre.Console.Extensions.Services`

### VerbosityLevel

Enum that describes the amount of output an application should emit, shared across many CLI tools. Ordered from least to most output: `Quiet` (0), `Minimal` (1), `Normal` (2), `Detailed` (3), and `Diagnostic` (4). Each level also has a shorthand alias that maps to the same value: `Q`, `M`, `N`, `D`, and `Diag`.

### VerbositySettings

A `CommandSettings` base class that adds a verbosity option.

| Member | Signature | Description |
|---|---|---|
| `Verbosity` | `VerbosityLevel Verbosity { get; set; }` | The requested verbosity level. Defaults to `VerbosityLevel.Normal`. |

### IVerbosityWriter

Service that writes plain text or Spectre markup only when the current verbosity allows it.

| Member | Signature | Description |
|---|---|---|
| `Verbosity` | `VerbosityLevel Verbosity { get; set; }` | The current verbosity threshold. |
| `MarkupSummary` | `void MarkupSummary(string text = "")` | Writes markup at the Minimal level. |
| `MarkupNormal` | `void MarkupNormal(string text = "")` | Writes markup at the Normal level. |
| `MarkupDetailed` | `void MarkupDetailed(string text = "")` | Writes markup at the Detailed level. |
| `MarkupDiagnostics` | `void MarkupDiagnostics(string text = "")` | Writes markup at the Diagnostic level. |
| `WriteSummary` | `void WriteSummary(string text = "")` | Writes plain text at the Minimal level. |
| `WriteNormal` | `void WriteNormal(string text = "")` | Writes plain text at the Normal level. |
| `WriteDetailed` | `void WriteDetailed(string text = "")` | Writes plain text at the Detailed level. |
| `WriteDiagnostics` | `void WriteDiagnostics(string text = "")` | Writes plain text at the Diagnostic level. |

### ConsoleVerbosityWriter

The default `IVerbosityWriter` implementation that renders through an `IAnsiConsole`.

| Member | Signature | Description |
|---|---|---|
| Constructor | `ConsoleVerbosityWriter(IAnsiConsole console)` | Creates the writer over the given console. |
| `Verbosity` | `VerbosityLevel Verbosity { get; set; }` | The current verbosity threshold. Defaults to `VerbosityLevel.Normal`. |

The `Markup*` and `Write*` members match the `IVerbosityWriter` contract above.

---

## Controls

Namespace: `D20Tek.Spectre.Console.Extensions.Controls`

### CurrencyPrompt

A culture-aware text prompt that validates currency input and converts it to a `decimal`. Implements `IPrompt<decimal>` and `IHasCulture`.

| Member | Signature | Description |
|---|---|---|
| Constructor | `CurrencyPrompt(string promptLabel)` | Creates the prompt with a label. Throws `ArgumentNullException` when the label is null or empty. |
| `Culture` | `CultureInfo? Culture { get; set; }` | The culture used for parsing and formatting; defaults to the current culture. |
| `WithCulture` | `CurrencyPrompt WithCulture(CultureInfo culture)` | Sets the culture used by the prompt. |
| `WithDefaultValue` | `CurrencyPrompt WithDefaultValue(decimal value)` | Sets the default value used when input is empty. |
| `WithMinValue` | `CurrencyPrompt WithMinValue(decimal min)` | Sets the minimum allowed value. |
| `WithMaxValue` | `CurrencyPrompt WithMaxValue(decimal max)` | Sets the maximum allowed value. |
| `WithExampleHint` | `CurrencyPrompt WithExampleHint(decimal value)` | Sets example hint text formatted for the current culture. |
| `WithErrorMessage` | `CurrencyPrompt WithErrorMessage(string message)` | Sets a custom validation error message. |
| `WithPromptStyle` | `CurrencyPrompt WithPromptStyle(Style promptStyle)` | Sets the style used for the prompt label. |
| `WithValidator` | `CurrencyPrompt WithValidator(Func<string, ValidationResult> validator)` | Sets a custom validation function used by the prompt. |
| `Show` | `decimal Show(IAnsiConsole console)` | Shows the prompt and returns the entered value. |
| `ShowAsync` | `Task<decimal> ShowAsync(IAnsiConsole console, CancellationToken token)` | Shows the prompt asynchronously and returns the entered value. |

### CurrencyPresenter

Static helper for culture-aware currency display.

| Member | Signature | Description |
|---|---|---|
| `Render` | `static string Render(this decimal value, string? positiveStyle = null, string? negativeStyle = null)` | Formats a decimal as culture-aware currency markup, with optional positive and negative styles. |
| `RenderAbbreviated` | `static string RenderAbbreviated(this decimal value, string? positiveStyle = null, string? negativeStyle = null)` | Formats a decimal as culture-aware currency markup using an abbreviated presentation for large values, with optional positive and negative styles. |

### HistoryTextPrompt&lt;T&gt;

A sealed text prompt with shell-style history navigation and tab auto-completion. Implements `IPrompt<T>` and `IHasCulture`.

| Member | Signature | Description |
|---|---|---|
| Constructor | `HistoryTextPrompt(string prompt, StringComparer? comparer = null)` | Creates the prompt with markup text and an optional comparer used for choices. Throws `ArgumentNullException` when prompt is null. |
| `PromptStyle` | `Style? PromptStyle { get; set; }` | The prompt style. |
| `Choices` | `List<T> Choices { get; }` | The list of auto-complete choices. |
| `Culture` | `CultureInfo? Culture { get; set; }` | The culture used by the prompt. |
| `InvalidChoiceMessage` | `string InvalidChoiceMessage { get; set; }` | The message shown for invalid choices. |
| `IsSecret` | `bool IsSecret { get; set; }` | Whether input is hidden. |
| `Mask` | `char? Mask { get; set; }` | The character used to mask a secret prompt. Defaults to `'*'`. |
| `ValidationErrorMessage` | `string ValidationErrorMessage { get; set; }` | The message shown for invalid input. |
| `ShowChoices` | `bool ShowChoices { get; set; }` | Whether choices are shown. Defaults to true. |
| `ShowDefaultValue` | `bool ShowDefaultValue { get; set; }` | Whether the default value is shown. Defaults to true. |
| `AllowEmpty` | `bool AllowEmpty { get; set; }` | Whether an empty result is valid. |
| `Converter` | `Func<T, string> Converter { get; set; }` | Converts a value to its display string. Defaults to the type's `TypeConverter`. |
| `Validator` | `Func<T, ValidationResult>? Validator { get; set; }` | The validator applied to input. |
| `DefaultValueStyle` | `Style? DefaultValueStyle { get; set; }` | The style for the default value. Defaults to green when null. |
| `ChoicesStyle` | `Style? ChoicesStyle { get; set; }` | The style for the choices list. Defaults to blue when null. |
| `History` | `List<string> History { get; }` | The history used for up/down arrow selection of previous entries. |
| `Show` | `T Show(IAnsiConsole console)` | Shows the prompt and returns the captured value. |
| `ShowAsync` | `Task<T> ShowAsync(IAnsiConsole console, CancellationToken cancellationToken)` | Shows the prompt asynchronously and returns the captured value. |

### HistoryTextPromptExtensions

Fluent extension methods for `HistoryTextPrompt<T>`.

| Member | Signature | Description |
|---|---|---|
| `AllowEmpty` | `HistoryTextPrompt<T> AllowEmpty<T>(this HistoryTextPrompt<T> obj)` | Permits empty input. |
| `PromptStyle` | `HistoryTextPrompt<T> PromptStyle<T>(this HistoryTextPrompt<T> obj, Style style)` | Sets the prompt style. |
| `ShowChoices` | `HistoryTextPrompt<T> ShowChoices<T>(this HistoryTextPrompt<T> obj, bool show)` / `ShowChoices<T>(this HistoryTextPrompt<T> obj)` | Controls whether choices are displayed. |
| `HideChoices` | `HistoryTextPrompt<T> HideChoices<T>(this HistoryTextPrompt<T> obj)` | Hides choices. |
| `ShowDefaultValue` | `HistoryTextPrompt<T> ShowDefaultValue<T>(this HistoryTextPrompt<T> obj, bool show = true)` | Controls default value display. |
| `HideDefaultValue` | `HistoryTextPrompt<T> HideDefaultValue<T>(this HistoryTextPrompt<T> obj)` | Hides the default value. |
| `ValidationErrorMessage` | `HistoryTextPrompt<T> ValidationErrorMessage<T>(this HistoryTextPrompt<T> obj, string message)` | Sets the validation error message. |
| `InvalidChoiceMessage` | `HistoryTextPrompt<T> InvalidChoiceMessage<T>(this HistoryTextPrompt<T> obj, string message)` | Sets the invalid choice message. |
| `DefaultValue` | `HistoryTextPrompt<T> DefaultValue<T>(this HistoryTextPrompt<T> obj, T value)` | Sets the default value. |
| `Validate` | `HistoryTextPrompt<T> Validate<T>(this HistoryTextPrompt<T> obj, Func<T, bool> validator, string? message = null)` | Adds a boolean validator with an optional error message. |
| `Validate` | `HistoryTextPrompt<T> Validate<T>(this HistoryTextPrompt<T> obj, Func<T, ValidationResult> validator)` | Adds a custom validator. |
| `AddChoice` | `HistoryTextPrompt<T> AddChoice<T>(this HistoryTextPrompt<T> obj, T choice)` | Adds a single choice. |
| `AddChoices` | `HistoryTextPrompt<T> AddChoices<T>(this HistoryTextPrompt<T> obj, IEnumerable<T> choices)` | Adds multiple choices. |
| `Secret` | `HistoryTextPrompt<T> Secret<T>(this HistoryTextPrompt<T> obj, char? mask = '*')` | Masks input for secrets. |
| `WithDisplayConverter` | `HistoryTextPrompt<T> WithDisplayConverter<T>(this HistoryTextPrompt<T> obj, Func<T, string> displaySelector)` | Sets how values are displayed. |
| `DefaultValueStyle` | `HistoryTextPrompt<T> DefaultValueStyle<T>(this HistoryTextPrompt<T> obj, Style? style)` | Sets the default value style. |
| `ChoicesStyle` | `HistoryTextPrompt<T> ChoicesStyle<T>(this HistoryTextPrompt<T> obj, Style? style)` | Sets the choices style. |
| `AddHistory` | `HistoryTextPrompt<T> AddHistory<T>(this HistoryTextPrompt<T> obj, IEnumerable<string> history)` | Seeds the navigable history list. |

### TableExtensions

Extension methods for Spectre.Console `Table` controls.

| Member | Signature | Description |
|---|---|---|
| `AddSeparatorRow` | `static void AddSeparatorRow(this Table table, int[] columnWidths, string style = "grey", char separatorChar = '─')` | Adds a separator row using the given column widths, style, and separator character. |

### AnsiConsoleExtensions

Extension methods for `IAnsiConsole`.

| Member | Signature | Description |
|---|---|---|
| `WriteMessages` | `static void WriteMessages(this IAnsiConsole console, params string[] messages)` | Writes multiple markup messages. |
| `WriteMessagesConditional` | `static void WriteMessagesConditional(this IAnsiConsole console, bool condition, params string[] messages)` | Writes multiple markup messages only when the condition is true. |

## Related

- [Guide: Building CommandApps with CommandAppBuilder](guide-command-app-builder.md)
- [Guide: Dependency Injection and Lifetimes](guide-dependency-injection.md)
- [Guide: Verbosity and Logging](guide-verbosity-logging.md)
- [Guide: Controls](guide-controls.md)
- [API Reference: Testing](api-reference-test.md)
- [API Reference hub](api-reference.md)
