//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions
{
    /// <summary>
    /// Abstract base class for defining a startup class to configure services and commands.
    /// </summary>
    /// <typeparam name="TRegister">
    /// The type of the service registration list to use in configuration
    /// </typeparam>
    public abstract class StartupBase<TRegister>
    {
        /// <summary>
        /// Override this method to configure app services and create the type registrar. 
        /// </summary>
        /// <param name="services">Service collection to use.</param>
        /// <returns>Type registrar for this application.</returns>
        public abstract ITypeRegistrar ConfigureServices(TRegister services);

        /// <summary>
        /// Override this method to configure console commands for this application.
        /// </summary>
        /// <param name="config">Configurator to use.</param>
        /// <returns>The configurator that was used.</returns>
        public abstract IConfigurator ConfigureCommands(IConfigurator config);

    }
}
