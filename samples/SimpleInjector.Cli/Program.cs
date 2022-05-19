//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Samples.Common.Commands;
using D20Tek.Samples.Common.Services;
using D20Tek.Spectre.Console.Extensions.Injection;
using Spectre.Console.Cli;

namespace SimpleInjector.Cli
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            // Create the DI container.
            var services = new Container();

            // configure services here...
            services.Register<IDisplayWriter, ConsoleDisplayWriter>();
            var registrar = new SimpleInjectorTypeRegistrar(services);

            // Create the CommandApp with specified command type and type registrar.
            var app = new CommandApp<DefaultCommand>(registrar);

            // Configure any commands in the application.
            app.Configure(config =>
            {
                config.CaseSensitivity(CaseSensitivity.None);
                config.SetApplicationName("SimpleInjector.Cli");
                config.ValidateExamples();

                config.AddCommand<DefaultCommand>("default")
                    .WithDescription("Default command that displays some text.")
                    .WithExample(new[] { "default", "--verbose", "high" });
            });

            return await app.RunAsync(args);
        }
    }
}
