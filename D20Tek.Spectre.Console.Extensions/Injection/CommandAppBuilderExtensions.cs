//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Autofac;
using D20Tek.Spectre.Console.Extensions.Injection;
using Lamar;
using LightInject;
using Microsoft.Extensions.DependencyInjection;
using Ninject;

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
            var container = new SimpleInjector.Container();
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

        /// <summary>
        /// Creates the type registrar based on the LightInject ServiceContainer
        /// and sets it in the CommandAppBuilder.
        /// </summary>
        /// <param name="builder">Builder instance to extend.</param>
        /// <returns>Returns the CommandAppBuilder.</returns>
        public static CommandAppBuilder WithLightInjectContainer(this CommandAppBuilder builder)
        {
            var container = new ServiceContainer();
            builder.Registrar = new LightInjectTypeRegistrar(container);
            return builder;
        }

        /// <summary>
        /// Creates the type registrar based on the Lamar ServiceRegistry
        /// and sets it in the CommandAppBuilder.
        /// </summary>
        /// <param name="builder">Builder instance to extend.</param>
        /// <returns>Returns the CommandAppBuilder.</returns>
        public static CommandAppBuilder WithLamarContainer(this CommandAppBuilder builder)
        {
            var registry = new ServiceRegistry();
            builder.Registrar = new LamarTypeRegistrar(registry);
            return builder;
        }
    }
}
