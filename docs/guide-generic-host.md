# Guide: Generic Host Integration

The `D20Tek.Spectre.Console.Extensions.Hosting` package bridges Spectre.Console.Cli to the .NET Generic Host (`Microsoft.Extensions.Hosting`), so command types resolve from the host's service provider while Spectre-registered types still work.

## Run a CommandApp from a host

Build a host, then create and run a `CommandApp` bridged to it:

```csharp
using D20Tek.Spectre.Console.Extensions.Hosting;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IGreetingService, GreetingService>();
var host = builder.Build();

return await host.RunCommandAppAsync(args, config =>
{
	config.AddCommand<GreetCommand>("greet");
});
```

`RunCommandAppAsync` and `RunCommandApp` create the app, apply your command configuration, and run it, returning the exit code.

## Use the fluent host builder

`CreateCommandAppBuilder` returns a `HostCommandAppBuilder` for a fluent flow:

```csharp
return await host.CreateCommandAppBuilder()
	.WithDefaultCommand<DefaultCommand>()
	.ConfigureCommands(config => config.AddCommand<GreetCommand>("greet"))
	.Build()
	.RunAsync(args);
```

`CreateCommandApp` returns a configured `CommandApp` directly when you want to run it yourself.

## Split startup with HostStartupBase

`HostStartupBase` separates host service registration from command configuration. Register it with `WithStartup<TStartup>` on the host application builder; `ConfigureServices` runs immediately (pre-build), and `ConfigureCommands` is applied automatically when the CommandApp is built (post-build):

```csharp
public sealed class AppStartup : HostStartupBase
{
	public override void ConfigureServices(IServiceCollection services) =>
		services.AddSingleton<IGreetingService, GreetingService>();

	public override IConfigurator ConfigureCommands(IConfigurator config)
	{
		config.AddCommand<GreetCommand>("greet");
		return config;
	}
}

var builder = Host.CreateApplicationBuilder(args);
builder.WithStartup<AppStartup>();
var host = builder.Build();

return await host.RunCommandAppAsync(args, _ => { });
```

Runtime registrations captured from Spectre resolve through a composite provider, so a Spectre-registered type can depend on another Spectre-registered type while host services take precedence.

## Related

- [API Reference: Hosting](api-reference-hosting.md)
