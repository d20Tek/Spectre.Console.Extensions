//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Injection
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TStartup">
    /// The type of the Startup class that is used for configuration.
    /// </typeparam>
    /// <typeparam name="TContainer"></typeparam>
    internal static class CommonDIFactory<TStartup, TContainer>
        where TStartup : StartupBase<TContainer>, new()
    {
        /// <summary>
        /// Factory method to create a CommandApp with the registered services and 
        /// configured commands contained in Startup class.
        /// </summary>
        /// <returns>Fully configured CommandApp.</returns>
        public static CommandApp CreateCommandApp(TContainer container)
        {
            // Create the DI container and its services.
            var (startup, registrar) = CreateConfiguredRegistrar(container);

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
        /// <typeparam name="TDefaultCommand">
        /// The type for the default command used in CommandApp creation.
        /// </typeparam>
        /// <returns>Fully configured CommandApp.</returns>
        public static CommandApp<TDefaultCommand> CreateCommandApp<TDefaultCommand>(TContainer container)
            where TDefaultCommand : class, ICommand
        {
            // Create the DI container and its services.
            var (startup, registrar) = CreateConfiguredRegistrar(container);

            // Create the CommandApp with specified command type and type registrar.
            var app = new CommandApp<TDefaultCommand>(registrar);

            // Configure any commands in the application.
            app.Configure(config => startup.ConfigureCommands(config));

            return app;
        }

        private static (StartupBase<TContainer>, ITypeRegistrar) CreateConfiguredRegistrar(TContainer container)
        {
            ArgumentNullException.ThrowIfNull(container, nameof(container));

            // Create the startup class instance from the app project.
            var startup = new TStartup();

            // Configure all services with this DI framework.
            var registrar = startup.ConfigureServices(container);

            return (startup, registrar);
        }
    }
}
