//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Injection;
using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.Spectre.Console.Extensions
{
    /// <summary>
    /// Extension methods to supply additional DI containers that CommandAppBuilder as use.
    /// </summary>
    public static class CommandAppBuilderExtensions
    {
        /// <summary>
        /// Creates the type registrar based on the DependencyInjection ServiceCollection in CommandAppBuilder
        /// with optional pre-registered services.
        /// </summary>
        /// <param name="builder">CommandAppBuilder to extend.</param>
        /// <param name="services">
        ///     [Optional] Provide pre-registered services, or create new instance when null.
        /// </param>
        /// <param name="lifetime">ServiceLifetime to use for all Register calls, defaults to Singleton</param>
        /// <returns>Returns the CommandAppBuilder</returns>
        public static CommandAppBuilder WithDIContainer(
            this CommandAppBuilder builder,
            IServiceCollection? services = null,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            // if no pre-registerd IServiceCollecion specified, create a new empty instance.
            services ??= new ServiceCollection();

            builder.SetRegistrar(new DependencyInjectionTypeRegistrar(services, lifetime));
            return builder;
        }
    }
}
