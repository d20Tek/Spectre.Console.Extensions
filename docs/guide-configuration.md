# Guide: Configuration and Options Binding

The `D20Tek.Spectre.Console.Extensions.Configuration` package adds `Microsoft.Extensions.Configuration` and strongly typed options binding to a `CommandAppBuilder`. Both hooks require a DI container, so call `WithDIContainer` first.

## Add configuration

`WithConfiguration` builds an `IConfiguration` and registers it in the container. By default it reads from an optional `appsettings.json` file and environment variables:

```csharp
using D20Tek.Spectre.Console.Extensions;
using D20Tek.Spectre.Console.Extensions.Configuration;

var builder = new CommandAppBuilder()
	.WithDIContainer()
	.WithConfiguration()
	.WithStartup<Startup>();
```

Pass a delegate to customize the configuration sources:

```csharp
builder.WithConfiguration(config =>
{
	config.SetBasePath(AppContext.BaseDirectory)
		  .AddJsonFile("appsettings.json", optional: true)
		  .AddJsonFile("appsettings.Development.json", optional: true)
		  .AddEnvironmentVariables();
});
```

`WithConfiguration` throws `InvalidOperationException` if no DI container has been configured.

## Bind strongly typed options

`WithOptions<TOptions>` binds a configuration section to an options class and registers it so it can be injected as `IOptions<TOptions>`. Data annotations on the options class are validated:

```csharp
public sealed class GreetingOptions
{
	[Required]
	public string DefaultName { get; init; } = string.Empty;
}

builder.WithConfiguration()
	   .WithOptions<GreetingOptions>("Greeting");
```

Call `WithConfiguration` first so an `IConfiguration` is available in the container. `WithOptions` throws `ArgumentException` when the section name is null or whitespace.

## Inject configuration and options

Inject `IConfiguration` or `IOptions<TOptions>` into your commands:

```csharp
public sealed class GreetCommand : Command
{
	private readonly GreetingOptions _options;

	public GreetCommand(IOptions<GreetingOptions> options) => _options = options.Value;
}
```

## Related

- [API Reference: Configuration](api-reference-configuration.md)
