//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Ninject;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Injection;

/// <summary>
/// Type registry for Spectre.Console that uses the Ninject
/// framework to register types with the DI engine.
/// </summary>
public sealed class NinjectTypeRegistrar : ITypeRegistrar
{
    private readonly StandardKernel _kernel;

    /// <summary>
    /// Constructor that takes a standard kernel instance.
    /// </summary>
    /// <param name="kernel">Service builder to use for registering types.</param>
    public NinjectTypeRegistrar(StandardKernel kernel)
    {
        ArgumentNullException.ThrowIfNull(kernel);
        _kernel = kernel;
    }

    /// <summary>
    /// Builds the type resolver representing the registrations
    /// specified in the current instance.
    /// </summary>
    /// <returns>A type resolver.</returns>
    public ITypeResolver Build() => new NinjectTypeResolver(_kernel);

    /// <summary>
    /// Registers the specified service.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="implementation">The implementation type.</param>
    public void Register(Type service, Type implementation)
    {
        ArgumentNullException.ThrowIfNull(service, nameof(service));
        ArgumentNullException.ThrowIfNull(implementation, nameof(implementation));

        _kernel.Bind(service).To(implementation);
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

        _kernel.Bind(service).ToConstant(implementation);
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

        _kernel.Bind(service).ToMethod((context) => factoryMethod());
    }
}
