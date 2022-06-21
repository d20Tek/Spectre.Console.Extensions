﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Lamar;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Injection
{
    /// <summary>
    /// Type registry for Spectre.Console that uses the Lamar DI
    /// framework to register types with the DI engine.
    /// </summary>
    public sealed class LamarTypeRegistrar : ITypeRegistrar
    {
        private readonly ServiceRegistry _registry;

        /// <summary>
        /// Constructor that takes a service registry instance.
        /// </summary>
        /// <param name="registry">Service registry to use for registering types.</param>
        public LamarTypeRegistrar(ServiceRegistry registry)
        {
            ArgumentNullException.ThrowIfNull(registry, nameof(registry));

            _registry = registry;
        }

        /// <summary>
        /// Builds the type resolver representing the registrations
        /// specified in the current instance.
        /// </summary>
        /// <returns>A type resolver.</returns>
        public ITypeResolver Build()
        {
            var container = new Container(_registry);
            return new LamarTypeResolver(container);
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

            _registry.AddSingleton(service, implementation);
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

            _registry.AddSingleton(service, implementation);
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

            _registry.AddSingleton(service, (provider) => factoryMethod());
        }
    }
}
