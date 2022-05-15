//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Ninject;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Injection
{
    /// <summary>
    /// Factory class for creating configured CommandApp instances that use
    /// Microsoft.Extensions.DependencyInjection framework.
    /// </summary>
    public static class NinjectFactory
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
            where TStartup : StartupBase<StandardKernel>, new()
        {
            return CommonDIFactory<TStartup, StandardKernel>.CreateCommandApp(new StandardKernel());
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
            where TStartup : StartupBase<StandardKernel>, new()
            where TDefaultCommand : class, ICommand
        {
            return CommonDIFactory<TStartup, StandardKernel>.CreateCommandApp<TDefaultCommand>(new StandardKernel());
        }
    }
}
