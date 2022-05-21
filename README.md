# d20Tek Spectre.Console Extensions
[![CI Build](https://github.com/d20Tek/Spectre.Console.Extensions/actions/workflows/spectre-console-extensions-ci.yml/badge.svg)](https://github.com/d20Tek/Spectre.Console.Extensions/actions/workflows/spectre-console-extensions-ci.yml)
[![Official Build](https://github.com/d20Tek/Spectre.Console.Extensions/actions/workflows/spectre-console-extensions-official.yml/badge.svg)](https://github.com/d20Tek/Spectre.Console.Extensions/actions/workflows/spectre-console-extensions-official.yml)
[![NuGet Release](https://github.com/d20Tek/Spectre.Console.Extensions/actions/workflows/nuget-release.yml/badge.svg)](https://github.com/d20Tek/Spectre.Console.Extensions/actions/workflows/nuget-release.yml)

## Introduction
This package provides extensions for common code and patterns when using Spectre.Console CLI app framework.
[SpectreConsole](https://github.com/spectreconsole/spectre.console)

The current releases contain implementations of ITypeRegistrar and ITypeResolver to integrate the Microsoft.Extensions.DependencyInjection, Ninject, and SimpleInjector frameworks with Spectre.Console.

We also support the CommandAppBuilder to easily create, configure, and run your instance of Spectre.Console.CommandApp.

For future releases, I will continue to investigate integration with other DI frameworks.

## Installation
This library is a NuGet package so it is easy to add to your project. To install this package into your solution, you can use the NuGet Package Manager. In PM, please use the following command:
```  
PM > Install-Package D20Tek.Spectre.Console.Extensions -Version 1.0.3
``` 

To install in the Visual Studio UI, go to the Tools menu > "Manage NuGet Packages". Then search for D20Tek.Spectre.Console.Extensions and install it from there.

Read more about the current release in our [Release Notes](ReleaseNotes.md).

## Usage
Once you've installed the NuGet package, you can start using it in your Spectre.Console projects.
If you would like basic information about how to build Spectre.Console CommandApps, please read: https://darthpedro.net/lessons-cli/.

### With CommandAppBuilder [recommended]
To add dependency injection into a CommandApp using the CommandAppBuilder, you can do the following in Program.cs:
```
using D20Tek.Samples.Common.Commands;
using D20Tek.Spectre.Console.Extensions;

namespace DependencyInjection.Cli
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await new CommandAppBuilder()
                             .WithDIContainer()
                             .WithStartup<Startup>()
                             .WithDefaultCommand<DefaultCommand>()
                             .Build()
                             .RunAsync(args);
        }
    }
}
```

And, you will need to create the following Startup.cs file:
```
using D20Tek.Samples.Common.Commands;
using D20Tek.Samples.Common.Services;
using D20Tek.Spectre.Console.Extensions;
using Spectre.Console.Cli;

namespace DependencyInjection.Cli
{
    internal class Startup : StartupBase
    {
        public override void ConfigureServices(ITypeRegistrar registrar)
        {
            // register services here...
            registrar.Register(typeof(IDisplayWriter), typeof(ConsoleDisplayWriter));
        }

        public override IConfigurator ConfigureCommands(IConfigurator config)
        {
            config.CaseSensitivity(CaseSensitivity.None);
            config.SetApplicationName("DependencyInjection.Cli");
            config.ValidateExamples();

            config.AddCommand<DefaultCommand>("default")
                .WithDescription("Default command that displays some text.")
                .WithExample(new[] { "default", "--verbose", "high" });

            return config;
        }
    }
}
```

### With Custom Code in Program
You do not need to use the CommandAppBuilder. It is still possible to write custom code your Program.Main method. And that can be simpler for small console applications. 

To add dependency injection this way, you can do the following:
```
using D20Tek.Samples.Common.Commands;
using D20Tek.Samples.Common.Services;
using D20Tek.Spectre.Console.Extensions.Injection;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace D20Tek.CountryService.Cli
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            // Create the DI container.
            var services = new ServiceCollection();

            // configure services here...
            services.AddSingleton<IDisplayWriter, ConsoleDisplayWriter>();
            var registrar = new DependencyInjectionTypeRegistrar(services);

            // Create the CommandApp with specified command type and type registrar.
            var app = new CommandApp<DefaultCommand>(registrar);

            // Configure any commands in the application.
            app.Configure(config =>
            {
                config.CaseSensitivity(CaseSensitivity.None);
                config.SetApplicationName("Basic.Cli");
                config.ValidateExamples();

                config.AddCommand<DefaultCommand>("default")
                    .WithDescription("Default command that displays some text.")
                    .WithExample(new[] { "default", "--verbose", "high" });
            });

            return await app.RunAsync(args);
        }
    }
}
```

Note: these code snippets assume using the Microsoft.Extensions.DependencyInjection framework. But similar sample code also exists for the other DI frameworks.

### Samples:
For more detailed examples on how to use D20Tek.Spectre.Console.Extensions, please review the following samples:

* [Basic Cli with DI](samples/Basic.Cli) - full listing for code in the Usage - Custom Code section above.
* [DependencyInjection.Cli](samples/DependencyInjection.Cli) - More elaborate use of Microsoft.Extensions.DependencyInjection registrar and resolver. Along with using the CommandAppBuilder to remove some of the creation complexity.
* [Ninject.Cli](samples/Ninject.Cli) - Use the Ninject DI framework to build type registrar and resolver.
* [SimpleInjector.Cli](samples/SimpleInjector.Cli) - Use the SimpleInjector DI framework to build type registrar and resolver.
* [NoDI.Cli](samples/NoDI.Cli) - Use the CommandAppBuilder to configure a console app that does not use a DI framework.

## Feedback
If you use these libraries and have any feedback, bugs, or suggestions, please file them in the Issues section of this repository.
