//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Autofac;
using D20Tek.Spectre.Console.Extensions.Injection;
using Microsoft.Extensions.DependencyInjection;
using Ninject;
using SimpleInjector;

namespace D20Tek.Spectre.Console.Extensions
{
    /// <summary>
    /// Extension methods to supply additional DI containers that CommandAppBuilder as use.
    /// </summary>
    public static class CommandAppBuilderExtensions
    {
        /// <summary>
        /// Creates the type registrar based on the DependencyInjector ServiceCollection
        /// and sets it in the CommandAppBuilder.
        /// </summary>
        /// <returns>Returns the CommandAppBuilder.</returns>
        public static CommandAppBuilder WithDIContainer(this CommandAppBuilder builder)
        {
            var container = new ServiceCollection();
            builder.Registrar = new DependencyInjectionTypeRegistrar(container);
            return builder;
        }

        /// <summary>
        /// Creates the type registrar based on the Ninject StandardKernel
        /// and sets it in the CommandAppBuilder.
        /// </summary>
        /// <param name="builder">Builder instance to extend.</param>
        /// <returns>Returns the CommandAppBuilder.</returns>
        public static CommandAppBuilder WithNinjectContainer(this CommandAppBuilder builder)
        {
            var container = new StandardKernel();
            builder.Registrar = new NinjectTypeRegistrar(container);
            return builder;
        }

        /// <summary>
        /// Creates the type registrar based on the SimpleInjector Container
        /// and sets it in the CommandAppBuilder.
        /// </summary>
        /// <param name="builder">Builder instance to extend.</param>
        /// <returns>Returns the CommandAppBuilder.</returns>
        public static CommandAppBuilder WithSimpleInjectorContainer(this CommandAppBuilder builder)
        {
            var container = new Container();
            builder.Registrar = new SimpleInjectorTypeRegistrar(container);
            return builder;
        }

        /// <summary>
        /// Creates the type registrar based on the Autofac ContainerBuilder
        /// and sets it in the CommandAppBuilder.
        /// </summary>
        /// <param name="builder">Builder instance to extend.</param>
        /// <returns>Returns the CommandAppBuilder.</returns>
        public static CommandAppBuilder WithAutofacContainer(this CommandAppBuilder builder)
        {
            var container = new ContainerBuilder();
            builder.Registrar = new AutofacTypeRegistrar(container);
            return builder;
        }
    }
}
