//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Injection
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
        /// Creates the type registrar based on the DependencyInjector ServiceCollection
        /// and sets it in the CommandAppBuilder.
        /// </summary>
        /// <returns>Returns the CommandAppBuilder.</returns>
        public CommandAppBuilder WithDIContainer()
        {
            var container = new ServiceCollection();
            this.Registrar = new DependencyInjectionTypeRegistrar(container);
            return this;
        }

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
            this._startup = new TStartup();
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
            ArgumentNullException.ThrowIfNull(this._startup, nameof(this._startup));

            if (this.Registrar != null)
            {
                // Configure all services with this DI framework.
                this._startup.ConfigureServices(this.Registrar);

                // Create the CommandApp with the type registrar.
                this._app = new CommandApp(this.Registrar);
            }
            else
            {
                this._app = new CommandApp();
            }

            // Configure any commands in the application.
            this._app.Configure(config => this._startup.ConfigureCommands(config));

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
            ArgumentNullException.ThrowIfNull(this._startup, nameof(this._startup));

            if (this.Registrar != null)
            {
                // Configure all services with this DI framework.
                this._startup.ConfigureServices(this.Registrar);

                // Create the CommandApp with the type registrar.
                this._app = new CommandApp(this.Registrar);
                this._app.SetDefaultCommand<TDefault>();
            }
            else
            {
                this._app = new CommandApp();
                this._app.SetDefaultCommand<TDefault>();
            }
            // Configure any commands in the application.
            this._app.Configure(config => this._startup.ConfigureCommands(config));

            return this;
        }

        /// <summary>
        /// Runs the CommandApp asynchronously.
        /// </summary>
        /// <param name="args">Command line arguments run with.</param>
        /// <returns>Return value from the application.</returns>
        public async Task<int> RunAsync(string[] args)
        {
            _ = this._app ?? throw new ArgumentNullException(
                nameof(this._app), "Build was not called prior to calling RunAsync.");

            return await this._app.RunAsync(args);
        }

        /// <summary>
        /// Runs the CommandApp.
        /// </summary>
        /// <param name="args">Command line arguments run with.</param>
        /// <returns>Return value from the application.</returns>
        public int Run(string[] args)
        {
            _ = this._app ?? throw new ArgumentNullException(
                nameof(this._app), "Build was not called prior to calling Run.");

            return this._app.Run(args);
        }
    }
}
