//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Injection;
using DependencyInjection.Cli.Commands;
using DependencyInjection.Cli.Services;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace D20Tek.CountryService.Cli
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var registrar = ConfigureServices(new ServiceCollection());

            var app = new CommandApp<DefaultCommand>(registrar);
            app.Configure(config => ConfigureCommands(config));

            return await app.RunAsync(args);
        }

        internal static ITypeRegistrar ConfigureServices(IServiceCollection services)
        {
            // Create a type registrar and register any dependencies.
            // A type registrar is an adapter for a DI framework.

            // register services here...
            services.AddSingleton<IDisplayWriter, ConsoleDisplayWriter>();

            return new DependencyInjectionTypeRegistrar(services);
        }

        private static IConfigurator ConfigureCommands(IConfigurator config)
        {
            config.CaseSensitivity(CaseSensitivity.None);
            config.SetApplicationName("Country.Cli");
            config.ValidateExamples();

            config.AddCommand<DefaultCommand>("default")
                .WithDescription("Default command that displays some text.")
                .WithExample(new[] { "default", "--verbose", "high" });

            return config;
        }
    }
}
