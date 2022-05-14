//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions
{
    /// <summary>
    /// Abstract base class for defining a startup class to configure services and commands.
    /// </summary>
    public abstract class StartupBase
    {
        /// <summary>
        /// Override this method to configure app services and create the type registrar. 
        /// </summary>
        /// <param name="services">Service collection to use.</param>
        /// <returns>Type registrar for this application.</returns>
        public abstract ITypeRegistrar ConfigureServices(IServiceCollection services);

        /// <summary>
        /// Override this method to configure console commands for this application.
        /// </summary>
        /// <param name="config">Configurator to use.</param>
        /// <returns>The configurator that was used.</returns>
        public abstract IConfigurator ConfigureCommands(IConfigurator config);

    }
}
