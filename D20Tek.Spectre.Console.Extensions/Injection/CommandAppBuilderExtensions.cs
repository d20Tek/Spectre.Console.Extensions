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
        /// Creates the type registrar based on the DependencyInjection ServiceCollection in CommandAppBuilder
        /// with optional pre-registered services.
        /// </summary>
        /// <param name="builder">CommandAppBuilder to extend.</param>
        /// <param name="services">
        ///     [Optional] Provide pre-registered services, or create new instance when null.
        /// </param>
        /// <returns>Returns the CommandAppBuilder</returns>
        public static CommandAppBuilder WithDIContainer(
            this CommandAppBuilder builder,
            IServiceCollection? services = null)
        {
            // if no pre-registerd IServiceCollecion specified, create a new empty instance.
            services ??= new ServiceCollection();

            builder.Registrar = new DependencyInjectionTypeRegistrar(services);
            return builder;
        }

        /// <summary>
        /// Creates the type registrar based on the Ninject Standard Kernel in CommandAppBuilder
        /// with optional pre-registered container.
        /// </summary>
        /// <param name="builder">CommandAppBuilder to extend.</param>
        /// <param name="container">
        ///     [Optional] Provide pre-registered services kernel. Creates new instance when not specified.
        /// </param>
        /// <returns>Returns the CommandAppBuilder</returns>
        public static CommandAppBuilder WithNinjectContainer(
            this CommandAppBuilder builder,
            StandardKernel? container = null)
        {
            // if no pre-registerd StandardKernel specified, create a new empty instance.
            container ??= new StandardKernel();

            builder.Registrar = new NinjectTypeRegistrar(container);
            return builder;
        }

        /// <summary>
        /// Creates the type registrar based on the SimpleInjector Container in CommandAppBuilder
        /// with optional pre-registered container.
        /// </summary>
        /// <param name="builder">CommandAppBuilder to extend.</param>
        /// <param name="container">
        ///     [Optional] Provide pre-registered services container. Creates new instance when not specified.
        /// </param>
        /// <returns>Returns the CommandAppBuilder</returns>
        public static CommandAppBuilder WithSimpleInjectorContainer(
            this CommandAppBuilder builder,
            SimpleInjector.Container? container = null)
        {
            // if no pre-registerd Container specified, create a new empty instance.
            container ??= new SimpleInjector.Container();

            builder.Registrar = new SimpleInjectorTypeRegistrar(container);
            return builder;
        }

        /// <summary>
        /// Creates the type registrar based on the AutoFac ContainerBuilder in CommandAppBuilder
        /// with optional pre-registered container.
        /// </summary>
        /// <param name="builder">CommandAppBuilder to extend.</param>
        /// <param name="container">
        ///     [Optional] Provide pre-registered services container. Creates new instance when not specified.
        /// </param>
        /// <returns>Returns the CommandAppBuilder</returns>
        public static CommandAppBuilder WithAutofacContainer(
            this CommandAppBuilder builder,
            ContainerBuilder? container = null)
        {
            // if no pre-registerd ContainerBuilder specified, create a new empty instance.
            container ??= new ContainerBuilder();

            builder.Registrar = new AutofacTypeRegistrar(container);
            return builder;
        }

        /// <summary>
        /// Creates the type registrar based on the LightInject ServiceContainer in CommandAppBuilder
        /// with optional pre-registered container.
        /// </summary>
        /// <param name="builder">CommandAppBuilder to extend.</param>
        /// <param name="container">
        ///     [Optional] Provide pre-registered services container. Creates new instance when not specified.
        /// </param>
        /// <returns>Returns the CommandAppBuilder</returns>
        public static CommandAppBuilder WithLightInjectContainer(
            this CommandAppBuilder builder,
            ServiceContainer? container = null)
        {
            // if no pre-registerd ServiceContainer specified, create a new empty instance.
            container ??= new ServiceContainer();

            builder.Registrar = new LightInjectTypeRegistrar(container);
            return builder;
        }

        /// <summary>
        /// Creates the type registrar based on the Lamar ServiceRegistry in CommandAppBuilder
        /// with optional pre-registered service registry.
        /// </summary>
        /// <param name="builder">CommandAppBuilder to extend.</param>
        /// <param name="serviceRegistry">
        ///     [Optional] Provide pre-registered services registry. Creates new instance when not specified.
        /// </param>
        /// <returns>Returns the CommandAppBuilder</returns>
        public static CommandAppBuilder WithLamarContainer(
            this CommandAppBuilder builder,
            ServiceRegistry? serviceRegistry = null)
        {
            // if no pre-registerd ServiceRegistry specified, create a new empty instance.
            serviceRegistry ??= new ServiceRegistry();

            builder.Registrar = new LamarTypeRegistrar(serviceRegistry);
            return builder;
        }
    }
}
