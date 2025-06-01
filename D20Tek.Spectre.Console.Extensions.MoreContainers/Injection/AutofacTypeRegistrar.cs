//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Injection
{
    /// <summary>
    /// Type registry for Spectre.Console that uses the Autofac
    /// framework to register types with the DI engine.
    /// </summary>
    public sealed class AutofacTypeRegistrar : ITypeRegistrar
    {
        private readonly ContainerBuilder _container;

        /// <summary>
        /// Constructor that takes a container builder instance.
        /// </summary>
        /// <param name="container">Container builder to use for registering types.</param>
        public AutofacTypeRegistrar(ContainerBuilder container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <summary>
        /// Builds the type resolver representing the registrations
        /// specified in the current instance.
        /// </summary>
        /// <returns>A type resolver.</returns>
        public ITypeResolver Build()
        {
            var provider = _container.Build();
            return new AutofacTypeResolver(provider);
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

            _container.RegisterType(implementation).As(service);
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

            _container.RegisterInstance(implementation).As(service);
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

            _container.Register((context) => factoryMethod()).As(service);
        }
    }
}
