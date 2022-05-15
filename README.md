# d20Tek Spectre.Console Extensions
[![CI Build](https://github.com/d20Tek/Spectre.Console.Extensions/actions/workflows/spectre-console-extensions-ci.yml/badge.svg)](https://github.com/d20Tek/Spectre.Console.Extensions/actions/workflows/spectre-console-extensions-ci.yml)
[![Spectre.Console.Extensions Official](https://github.com/d20Tek/Spectre.Console.Extensions/actions/workflows/spectre-console-extensions-official.yml/badge.svg)](https://github.com/d20Tek/Spectre.Console.Extensions/actions/workflows/spectre-console-extensions-official.yml)
[![NuGet Release](https://github.com/d20Tek/Spectre.Console.Extensions/actions/workflows/nuget-release.yml/badge.svg)](https://github.com/d20Tek/Spectre.Console.Extensions/actions/workflows/nuget-release.yml)

## Introduction
This package provides extensions for common code and patterns when using Spectre.Console CLI app framework.
[SpectreConsole](https://github.com/spectreconsole/spectre.console)

The initial release contains implementations of ITypeRegistrar and ITypeResolver to integrate the Microsoft.Extensions.DependencyInjection framework with Spectre.Console.
For future releases, I will investigate integration with other DI frameworks.

## Installation
This library is in NuGet package so it is easy to add to your project. To install this package into your solution, you can use the NuGet Package Manager. In PM, please use the following command:
```  
PM > Install-Package D20Tek.Spectre.Console.Extensions -Version 1.0.1
``` 

To install in the Visual Studio UI, go to the Tools menu > "Manage NuGet Packages". Then search for D20Tek.Spectre.Console.Extensions and install it from there.

Read more about the current release in our [Release Notes](ReleaseNotes.md).

## Usage
Once you've installed the NuGet package, you can start using it in your Spectre.Console projects.
If you would like basic information about how to build Spectre.Console CommandApps, please read: https://darthpedro.net/lessons-cli/.

To add dependency injection into a CommandApp, you can do the following:
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

### Samples:
For more detailed examples on how to use D20Tek.Spectre.Console.Extensions, please review the following samples:

* [Basic Cli with DI](samples/Basic.Cli) - full listing for code in the Usage section above.
* [DependencyInjection.Cli](samples/DependencyInjection.Cli) - More elaborate use of Microsoft.Extensions.DependencyInjection registrar and resolver. Along with using the DependencyInjectionFactory to remove some of the creation complexity.

## Feedback
If you use these libraries and have any feedback, bugs, or suggestions, please file them in the Issues section of this repository.
