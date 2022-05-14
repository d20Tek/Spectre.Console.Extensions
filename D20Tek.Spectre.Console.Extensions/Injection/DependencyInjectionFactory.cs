//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Injection
{
    /// <summary>
    /// Factory class for creating configured CommandApp instances that use
    /// Microsoft.Extensions.DependencyInjection framework.
    /// </summary>
    public static class DependencyInjectionFactory
    {
        /// <summary>
        /// Factory method to create a CommandApp with the registered services and 
        /// configured commands contained in Startup class.
        /// </summary>
        /// <typeparam name="TStartup">
        /// The type of the Startup class that is used for configuration.
        /// </typeparam>
        /// <returns>Fully configured CommandApp.</returns>
        public static CommandApp CreateCommandApp<TStartup>()
            where TStartup : StartupBase, new()
        {
            // Create the startup class instance from the app project.
            // Create the DI container and its services.
            var (startup, registrar) = CreateConfiguredRegistrar<TStartup>();

            // Create the CommandApp with the type registrar.
            var app = new CommandApp(registrar);

            // Configure any commands in the application.
            app.Configure(config => startup.ConfigureCommands(config));

            return app;
        }

        /// <summary>
        /// Factory method to create a CommandApp with default command, registered services, 
        /// and configured commands contained in Startup class.
        /// </summary>
        /// <typeparam name="TStartup">
        /// The type of the Startup class that is used for configuration.
        /// </typeparam>
        /// <typeparam name="TDefaultCommand">
        /// The type for the default command used in CommandApp creation.
        /// </typeparam>
        /// <returns>Fully configured CommandApp.</returns>
        public static CommandApp<TDefaultCommand> CreateCommandApp<TStartup, TDefaultCommand>()
            where TStartup : StartupBase, new()
            where TDefaultCommand : class, ICommand
        {
            // Create the DI container and its services.
            var (startup, registrar) = CreateConfiguredRegistrar<TStartup>();

            // Create the CommandApp with specified command type and type registrar.
            var app = new CommandApp<TDefaultCommand>(registrar);

            // Configure any commands in the application.
            app.Configure(config => startup.ConfigureCommands(config));

            return app;
        }

        private static (StartupBase, ITypeRegistrar) CreateConfiguredRegistrar<TStartup>()
            where TStartup : StartupBase, new()
        {
            // Create the startup class instance from the app project.
            var startup = new TStartup();
            var services = new ServiceCollection();

            // Configure all services with this DI framework.
            var registrar = startup.ConfigureServices(services);

            return (startup, registrar);
        }
    }
}
