//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions;
using D20Tek.Spectre.Console.Extensions.Injection;
using DependencyInjection.Cli.Commands;
using DependencyInjection.Cli.Services;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace DependencyInjection.Cli
{
    internal class Startup : StartupBase
    {
        public override ITypeRegistrar ConfigureServices(IServiceCollection services)
        {
            // Create a type registrar and register any dependencies.
            // A type registrar is an adapter for a DI framework.

            // register services here...
            services.AddSingleton<IDisplayWriter, ConsoleDisplayWriter>();

            return new DependencyInjectionTypeRegistrar(services);
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
