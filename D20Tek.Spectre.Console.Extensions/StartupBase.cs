//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions
{
    /// <summary>
    /// Abstract base class for defining a startup class to configure services and commands.
    /// </summary>
    public abstract class StartupBase
    {
        /// <summary>
        /// Override this method to configure app services in the type registrar. 
        /// </summary>
        /// <param name="registrar">Type registrar to use.</param>
        public abstract void ConfigureServices(ITypeRegistrar registrar);

        /// <summary>
        /// Override this method to configure console commands for this application.
        /// </summary>
        /// <param name="config">Configurator to use.</param>
        /// <returns>The configurator that was used.</returns>
        public abstract IConfigurator ConfigureCommands(IConfigurator config);

    }
}
