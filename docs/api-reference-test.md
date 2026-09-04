# API Reference: Testing

Package: `D20Tek.Spectre.Console.Extensions`
Namespace: `D20Tek.Spectre.Console.Extensions.Testing` (with extensions in `D20Tek.Spectre.Console.Extensions`)

This document covers the test context classes, the end-to-end runner, and the result types used to test Spectre.Console CLIs. The rest of the core package is documented in [API Reference: Core](api-reference-core.md).

## Contents

- [CommandAppBuilderTestContext](#commandappbuildertestcontext)
- [CommandAppTestContext](#commandapptestcontext)
- [CommandConfigurationTestContext](#commandconfigurationtestcontext)
- [CommandAppE2ERunner](#commandappe2erunner)
- [CommandAppResult](#commandappresult)
- [CommandAppBasicResult](#commandappbasicresult)
- [CommandMetadata](#commandmetadata)
- [CommandAppBuilderTestExtensions](#commandappbuildertestextensions)

## CommandAppBuilderTestContext

Wraps a `CommandAppBuilder` and a `TestConsole` for testing builder-based apps.

| Member | Signature | Description |
|---|---|---|
| `Console` | `TestConsole Console { get; }` | The test console capturing output. |
| `Builder` | `CommandAppBuilder Builder { get; }` | The builder under test. |
| Constructor | `CommandAppBuilderTestContext()` | Creates the context with a test console. |
| `Run` | `CommandAppResult Run(string[] args)` | Runs the app synchronously. |
| `RunAsync` | `Task<CommandAppResult> RunAsync(string[] args)` | Runs the app asynchronously. |
| `RunWithException<T>` | `CommandAppResult RunWithException<T>(string[] args)` | Runs and captures an expected exception of type `T`. |
| `RunWithExceptionAsync<T>` | `Task<CommandAppResult> RunWithExceptionAsync<T>(string[] args)` | Async variant of `RunWithException<T>`. |

## CommandAppTestContext

Sets up a type registrar and a `TestConsole` for testing without the builder.

| Member | Signature | Description |
|---|---|---|
| `Registrar` | `ITypeRegistrar Registrar { get; }` | The registrar used to configure the app. |
| `Console` | `TestConsole Console { get; }` | The test console capturing output. |
| Constructor | `CommandAppTestContext()` | Creates the context. |
| `Configure` | `void Configure(Action<IConfigurator> action)` | Configures the app's commands. |
| `Run` | `CommandAppResult Run(string[] args)` | Runs the app synchronously. |
| `RunAsync` | `Task<CommandAppResult> RunAsync(string[] args)` | Runs the app asynchronously. |
| `RunWithException<T>` | `CommandAppResult RunWithException<T>(string[] args)` | Runs and captures an expected exception of type `T`. |
| `RunWithExceptionAsync<T>` | `Task<CommandAppResult> RunWithExceptionAsync<T>(string[] args)` | Async variant of `RunWithException<T>`. |

## CommandConfigurationTestContext

Exposes a registrar, resolver, and test configurator for asserting on command configuration.

| Member | Signature | Description |
|---|---|---|
| `Registrar` | `ITypeRegistrar Registrar { get; }` | The registrar used during configuration. |
| `Resolver` | `ITypeResolver Resolver { get; }` | The resolver built from the registrar. |
| `Configurator` | `ITestConfigurator Configurator { get; }` | The test configurator capturing command metadata. |
| Constructor | `CommandConfigurationTestContext()` | Creates the context. |

## CommandAppE2ERunner

Static runner that invokes a real `Main` entry point and captures its output. The entry point may be synchronous (`Func<string[], int>`) or asynchronous (`Func<string[], Task<int>>`).

| Member | Signature | Description |
|---|---|---|
| `Run` | `static CommandAppBasicResult Run(Func<string[], int> mainEntryPoint, string commandLine)` | Runs the synchronous entry point with a command-line string. |
| `Run` | `static CommandAppBasicResult Run(Func<string[], int> mainEntryPoint, string[] args)` | Runs the synchronous entry point with pre-split arguments. |
| `RunAsync` | `static Task<CommandAppBasicResult> RunAsync(Func<string[], Task<int>> mainEntryPointAsync, string commandLine)` | Runs the asynchronous entry point with a command-line string. |
| `RunAsync` | `static Task<CommandAppBasicResult> RunAsync(Func<string[], Task<int>> mainEntryPointAsync, string[] args)` | Runs the asynchronous entry point with pre-split arguments. |

## CommandAppResult

Result of a context-based run. Derives from `CommandAppBasicResult`, adding the captured command context and settings.

| Member | Signature | Description |
|---|---|---|
| Constructor | `CommandAppResult(int exitCode, string output, CommandContext? context, CommandSettings? settings)` | Creates the result. |
| `Context` | `CommandContext? Context { get; }` | The command context for this execution result. |
| `Settings` | `CommandSettings? Settings { get; }` | The command settings for this execution result. |

## CommandAppBasicResult

Result of an end-to-end run.

| Member | Signature | Description |
|---|---|---|
| Constructor | `CommandAppBasicResult(int exitCode, string? output)` | Creates the result. |
| `ExitCode` | `int ExitCode { get; }` | The process exit code. |
| `Output` | `string Output { get; }` | The captured output, or an empty string when none. |

## CommandMetadata

Describes a configured command or branch for assertions in configuration tests. Exposes properties such as `Name`, `Aliases`, `Description`, `Data`, `CommandType`, `SettingsType`, `Delegate`, `AsyncDelegate`, `IsDefaultCommand`, `IsHidden`, `Children`, and `Examples`, plus factory methods `FromBranch`, `FromBranch<TSettings>`, `FromType<TCommand>`, `FromDelegate<TSettings>`, and `FromAsyncDelegate<TSettings>`.

## CommandAppBuilderTestExtensions

| Member | Signature | Description |
|---|---|---|
| `WithTestConfiguration` | `static CommandAppBuilder WithTestConfiguration(this CommandAppBuilder builder, Action<IConfigurator> action)` | Applies additional test configuration to the CommandApp after it is built. |

## Related

- [Guide: Testing CLI Applications](guide-testing-cli-apps.md)
- [API Reference: Core](api-reference-core.md)
- [API Reference hub](api-reference.md)
