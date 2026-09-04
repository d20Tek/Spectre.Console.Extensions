# Guide: Testing CLI Applications

The `D20Tek.Spectre.Console.Extensions.Testing` namespace provides context classes and an end-to-end runner that reduce the boilerplate of testing Spectre.Console CLIs. It captures console output and exit codes so you can assert on them.

## Test a CommandAppBuilder-based app

`CommandAppBuilderTestContext` wraps a `CommandAppBuilder` and a `TestConsole`. Configure the builder as your app does, then run and assert:

```csharp
var context = new CommandAppBuilderTestContext();
context.Builder
	   .WithDIContainer()
	   .WithStartup<Startup>()
	   .Build();

var result = await context.RunAsync(new[] { "greet", "World" });

Assert.AreEqual(0, result.ExitCode);
StringAssert.Contains(result.Output, "Hello, World");
```

Use `Run`/`RunAsync` for normal execution and `RunWithException<T>`/`RunWithExceptionAsync<T>` when you expect the app to throw a specific exception type.

## Test with a type registrar directly

`CommandAppTestContext` sets up a registrar and a `TestConsole` without the builder. Configure commands through its `Configure` method:

```csharp
var context = new CommandAppTestContext();
context.Configure(config => config.AddCommand<GreetCommand>("greet"));

var result = context.Run(new[] { "greet", "World" });
Assert.AreEqual(0, result.ExitCode);
```

## Test command configuration

`CommandConfigurationTestContext` exposes a registrar, a resolver, and an `ITestConfigurator` so you can assert on how commands and branches are configured without running them.

## End-to-end runs

`CommandAppE2ERunner` invokes a real `Main` entry point and captures its output as a `CommandAppBasicResult`:

```csharp
var result = CommandAppE2ERunner.Run(Program.Main, "greet World");

Assert.AreEqual(0, result.ExitCode);
StringAssert.Contains(result.Output, "Hello, World");
```

`Run` has overloads that accept a command-line string or a pre-split `string[]`.

## Result types

- `CommandAppResult` - exposes the exit code and captured console output for context-based runs.
- `CommandAppBasicResult` - exposes `ExitCode` and `Output` for end-to-end runs.

## Related

- [API Reference: Testing](api-reference-test.md)
