//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Injection
{
    /// <summary>
    /// Type registry for Spectre.Console that uses the Microsoft.Extensions.DependencyInjection
    /// framework to register types with the DI engine.
    /// </summary>
    public sealed class DependencyInjectionTypeRegistrar : ITypeRegistrar, ISupportLifetimes
    {
        private readonly ServiceLifetime _defaultLifetime;

        /// <inheritdoc />
        public IServiceCollection Services { get; }

        /// <summary>
        /// Constructor that takes a service collection instance.
        /// </summary>
        /// <param name="builder">Service collection builder to use for registering types.</param>
        /// <param name="lifetime">Default lifetime to use in all Register functions. Singleton if unspecified.</param>
        public DependencyInjectionTypeRegistrar(
            IServiceCollection builder,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            Services = builder;
            _defaultLifetime = lifetime;
        }

        /// <summary>
        /// Builds the type resolver representing the registrations
        /// specified in the current instance.
        /// </summary>
        /// <returns>A type resolver.</returns>
        public ITypeResolver Build()
        {
            var provider = Services.BuildServiceProvider();
            return new DependencyInjectionTypeResolver(provider);
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

            Services.Add(new ServiceDescriptor(service, implementation, _defaultLifetime));
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

            Services.AddSingleton(service, implementation);
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

            Services.Add(new ServiceDescriptor(service, (provider) => factoryMethod(), _defaultLifetime));
        }
    }
}
