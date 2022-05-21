//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using SimpleInjector;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Injection
{
    /// <summary>
    /// Type registry for Spectre.Console that uses the SimpleInjector
    /// framework to register types with the DI engine.
    /// </summary>
    public sealed class SimpleInjectorTypeRegistrar : ITypeRegistrar
    {
        private readonly Container _container;

        /// <summary>
        /// Constructor that takes a standard kernel instance.
        /// </summary>
        /// <param name="container">Service builder to use for registering types.</param>
        public SimpleInjectorTypeRegistrar(Container container)
        {
            ArgumentNullException.ThrowIfNull(container, nameof(container));

            _container = container;
            _container.Options.AllowOverridingRegistrations = true;
        }

        /// <summary>
        /// Builds the type resolver representing the registrations
        /// specified in the current instance.
        /// </summary>
        /// <returns>A type resolver.</returns>
        public ITypeResolver Build()
        {
            return new SimpleInjectorTypeResolver(_container);
        }

        /// <summary>
        /// Registers the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="implementation">The implementation type.</param>
        public void Register(Type service, Type implementation)
        {
            ArgumentNullException.ThrowIfNull(service, nameof(service));
            ArgumentNullException.ThrowIfNull(implementation, nameof(implementation));

            _container.Register(service, implementation);
        }

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="implementation">The implementation.</param>
        public void RegisterInstance(Type service, object implementation)
        {
            ArgumentNullException.ThrowIfNull(service, nameof(service));
            ArgumentNullException.ThrowIfNull(implementation, nameof(implementation));

            _container.RegisterInstance(service, implementation);
        }

        /// <summary>
        /// Registers the specified instance lazily.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="factoryMethod">The factory that creates the implementation.</param>
        public void RegisterLazy(Type service, Func<object> factoryMethod)
        {
            ArgumentNullException.ThrowIfNull(service, nameof(service));
            ArgumentNullException.ThrowIfNull(factoryMethod, nameof(factoryMethod));

            _container.Register(service, factoryMethod, Lifestyle.Singleton);
        }
    }
}
