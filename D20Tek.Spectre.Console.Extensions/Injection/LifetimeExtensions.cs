using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.Spectre.Console.Extensions.Injection;

/// <summary>
/// Extension methods to support registering types with specific lifetimes into the
/// Microsoft DI container (or any container that supports IServiceCollection).
/// </summary>
public static class LifetimeExtensions
{
    /// <summary>
    /// Adds a singleton service of the type specified in <typeparamref name="TService"/> with an
    /// implementation type specified in <typeparamref name="TImplementation"/> to the
    /// specified registrar.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="registrar">The TypeRegistrar to add the service to.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Singleton"/>
    public static ISupportLifetimes RegisterSingleton<TService, TImplementation>(this ISupportLifetimes registrar)
        where TService : class
        where TImplementation : class, TService
    {
        registrar.Services.AddSingleton<TService, TImplementation>();
        return registrar;
    }

    /// <summary>
    /// Adds a singleton service of the type specified in <typeparamref name="TService"/> with an
    /// implementation instance as a parameter to the specified registrar.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="registrar">The TypeRegistrar to add the service to.</param>
    /// <param name="instance">Instance of service to use.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Singleton"/>
    public static ISupportLifetimes RegisterSingleton<TService>(this ISupportLifetimes registrar, TService instance)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(instance);
        registrar.Services.AddSingleton<TService>(instance);
        return registrar;
    }

    /// <summary>
    /// Adds a singleton service of the type specified in <typeparamref name="TService"/> using the
    /// factory specified in <paramref name="implementationFactory"/> to the specified registrar.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="registrar">The TypeRegistrar to add the service to.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Singleton"/>
    public static ISupportLifetimes RegisterSingleton<TService, TImplementation>(
        this ISupportLifetimes registrar,
        Func<IServiceProvider, TImplementation> implementationFactory)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(implementationFactory);
        registrar.Services.AddSingleton<TService>(implementationFactory);
        return registrar;
    }

    /// <summary>
    /// Adds a scoped service of the type specified in <typeparamref name="TService"/> with an
    /// implementation type specified in <typeparamref name="TImplementation"/> to the
    /// specified registrar.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="registrar">The TypeRegistrar to add the service to.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Singleton"/>
    public static ISupportLifetimes RegisterScoped<TService, TImplementation>(this ISupportLifetimes registrar)
        where TService : class
        where TImplementation : class, TService
    {
        registrar.Services.AddScoped<TService, TImplementation>();
        return registrar;
    }

    /// <summary>
    /// Adds a scoped service of the type specified in <typeparamref name="TService"/> using the
    /// factory specified in <paramref name="implementationFactory"/> to the specified registrar.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="registrar">The TypeRegistrar to add the service to.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Singleton"/>
    public static ISupportLifetimes RegisterScoped<TService, TImplementation>(
        this ISupportLifetimes registrar,
        Func<IServiceProvider, TImplementation> implementationFactory)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(implementationFactory);
        registrar.Services.AddScoped<TService>(implementationFactory);
        return registrar;
    }

    /// <summary>
    /// Adds a transient service of the type specified in <typeparamref name="TService"/> with an
    /// implementation type specified in <typeparamref name="TImplementation"/> to the
    /// specified registrar.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="registrar">The TypeRegistrar to add the service to.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Singleton"/>
    public static ISupportLifetimes RegisterTransient<TService, TImplementation>(this ISupportLifetimes registrar)
        where TService : class
        where TImplementation : class, TService
    {
        registrar.Services.AddTransient<TService, TImplementation>();
        return registrar;
    }

    /// <summary>
    /// Adds a transient service of the type specified in <typeparamref name="TService"/> using the
    /// factory specified in <paramref name="implementationFactory"/> to the specified registrar.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="registrar">The TypeRegistrar to add the service to.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="ServiceLifetime.Singleton"/>
    public static ISupportLifetimes RegisterTransient<TService, TImplementation>(
        this ISupportLifetimes registrar,
        Func<IServiceProvider, TImplementation> implementationFactory)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(implementationFactory);
        registrar.Services.AddTransient<TService>(implementationFactory);
        return registrar;
    }
}
