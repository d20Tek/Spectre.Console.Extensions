//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandAppBuilder
    {
        private CommandApp? _app;
        private StartupBase? _startup;

        internal ITypeRegistrar? Registrar { get; set; }

        /// <summary>
        /// Sets up the Startup class to use in this builder.
        /// </summary>
        /// <typeparam name="TStartup">Type of the Startup class in this project,
        /// which must derive from StartupBase.</typeparam>
        /// <returns>Returns the CommandAppBuilder.</returns>
        public CommandAppBuilder WithStartup<TStartup>()
            where TStartup : StartupBase, new()
        {
            // Create the startup class instance from the app project.
            _startup = new TStartup();
            return this;
        }

        /// <summary>
        /// Builds the CommandApp encapsulated by this builder. It ensures that the
        /// application services are configured, the CommandApp is created with the
        /// type registrar, and the app Commands are configured.
        /// </summary>
        /// <returns>Returns the CommandAppBuilder.</returns>
        public CommandAppBuilder Build()
        {
            ArgumentNullException.ThrowIfNull(_startup, nameof(_startup));

            if (Registrar != null)
            {
                // Configure all services with this DI framework.
                _startup.ConfigureServices(Registrar);

                // Create the CommandApp with the type registrar.
                _app = new CommandApp(Registrar);
            }
            else
            {
                _app = new CommandApp();
            }

            // Configure any commands in the application.
            _app.Configure(config => _startup.ConfigureCommands(config));

            return this;
        }

        /// <summary>
        /// Builds the CommandApp encapsulated by this builder. It ensures that the
        /// application services are configured, the CommandApp is created with the
        /// type registrar, and the app Commands are configured.
        /// </summary>
        /// <typeparam name="TDefault">Default command to create the CommandApp with.</typeparam>
        /// <returns>Returns the CommandAppBuilder.</returns>
        public CommandAppBuilder Build<TDefault>()
            where TDefault : class, ICommand
        {
            ArgumentNullException.ThrowIfNull(_startup, nameof(_startup));

            if (Registrar != null)
            {
                // Configure all services with this DI framework.
                _startup.ConfigureServices(Registrar);

                // Create the CommandApp with the type registrar.
                _app = new CommandApp(Registrar);
                _app.SetDefaultCommand<TDefault>();
            }
            else
            {
                _app = new CommandApp();
                _app.SetDefaultCommand<TDefault>();
            }
            // Configure any commands in the application.
            _app.Configure(config => _startup.ConfigureCommands(config));

            return this;
        }

        /// <summary>
        /// Runs the CommandApp asynchronously.
        /// </summary>
        /// <param name="args">Command line arguments run with.</param>
        /// <returns>Return value from the application.</returns>
        public async Task<int> RunAsync(string[] args)
        {
            _ = _app ?? throw new ArgumentNullException(
                nameof(_app), "Build was not called prior to calling RunAsync.");

            return await _app.RunAsync(args);
        }

        /// <summary>
        /// Runs the CommandApp.
        /// </summary>
        /// <param name="args">Command line arguments run with.</param>
        /// <returns>Return value from the application.</returns>
        public int Run(string[] args)
        {
            _ = _app ?? throw new ArgumentNullException(
                nameof(_app), "Build was not called prior to calling Run.");

            return _app.Run(args);
        }
    }
}
