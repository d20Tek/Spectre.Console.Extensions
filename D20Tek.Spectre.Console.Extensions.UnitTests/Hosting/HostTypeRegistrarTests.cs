//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Hosting;
using D20Tek.Spectre.Console.Extensions.UnitTests.Hosting.Fakes;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Hosting;

[TestClass]
public class HostTypeRegistrarTests
{
    [TestMethod]
    public void Constructor_WithNullProvider_ThrowsException()
    {
        // Arrange - Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () => new HostTypeRegistrar(null!));
    }

    [TestMethod]
    public void Build_ReturnsHostTypeResolver()
    {
        // Arrange
        var provider = new ServiceCollection().BuildServiceProvider();
        var registrar = new HostTypeRegistrar(provider);

        // Act
        var resolver = registrar.Build();

        // Assert
        Assert.IsInstanceOfType<HostTypeResolver>(resolver);
    }

    [TestMethod]
    public void Register_WithNullService_ThrowsException()
    {
        // Arrange
        var provider = new ServiceCollection().BuildServiceProvider();
        var registrar = new HostTypeRegistrar(provider);

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            registrar.Register(null!, typeof(GreetingService)));
    }

    [TestMethod]
    public void Register_WithNullImplementation_ThrowsException()
    {
        // Arrange
        var provider = new ServiceCollection().BuildServiceProvider();
        var registrar = new HostTypeRegistrar(provider);

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            registrar.Register(typeof(IGreetingService), null!));
    }

    [TestMethod]
    public void RegisterInstance_WithNullService_ThrowsException()
    {
        // Arrange
        var provider = new ServiceCollection().BuildServiceProvider();
        var registrar = new HostTypeRegistrar(provider);

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            registrar.RegisterInstance(null!, new GreetingService()));
    }

    [TestMethod]
    public void RegisterInstance_WithNullImplementation_ThrowsException()
    {
        // Arrange
        var provider = new ServiceCollection().BuildServiceProvider();
        var registrar = new HostTypeRegistrar(provider);

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            registrar.RegisterInstance(typeof(IGreetingService), null!));
    }

    [TestMethod]
    public void RegisterLazy_WithNullService_ThrowsException()
    {
        // Arrange
        var provider = new ServiceCollection().BuildServiceProvider();
        var registrar = new HostTypeRegistrar(provider);

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            registrar.RegisterLazy(null!, [ExcludeFromCodeCoverage]() => new GreetingService()));
    }

    [TestMethod]
    public void RegisterLazy_WithNullFactory_ThrowsException()
    {
        // Arrange
        var provider = new ServiceCollection().BuildServiceProvider();
        var registrar = new HostTypeRegistrar(provider);

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            registrar.RegisterLazy(typeof(IGreetingService), null!));
    }

    [TestMethod]
    public void Register_ThenResolve_ResolvesRegisteredType()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<IGreetingService, GreetingService>();
        var provider = services.BuildServiceProvider();
        var registrar = new HostTypeRegistrar(provider);
        registrar.Register(typeof(IGreetingService), typeof(GreetingService));

        // Act
        var resolver = registrar.Build();
        var result = resolver.Resolve(typeof(IGreetingService));

        // Assert
        Assert.IsInstanceOfType<GreetingService>(result);
    }

    [TestMethod]
    public void RegisterInstance_ThenResolve_ReturnsInstance()
    {
        // Arrange
        var instance = new GreetingService();
        var provider = new ServiceCollection().BuildServiceProvider();
        var registrar = new HostTypeRegistrar(provider);
        registrar.RegisterInstance(typeof(IGreetingService), instance);

        // Act
        var resolver = registrar.Build();
        var result = resolver.Resolve(typeof(IGreetingService));

        // Assert
        Assert.AreSame(instance, result);
    }

    [TestMethod]
    public void RegisterLazy_ThenResolve_InvokesFactory()
    {
        // Arrange
        var instance = new GreetingService();
        var provider = new ServiceCollection().BuildServiceProvider();
        var registrar = new HostTypeRegistrar(provider);
        registrar.RegisterLazy(typeof(IGreetingService), () => instance);

        // Act
        var resolver = registrar.Build();
        var result = resolver.Resolve(typeof(IGreetingService));

        // Assert
        Assert.AreSame(instance, result);
    }

    [TestMethod]
    public void ImplementsITypeRegistrar()
    {
        // Arrange
        var provider = new ServiceCollection().BuildServiceProvider();

        // Act
        var registrar = new HostTypeRegistrar(provider);

        // Assert
        Assert.IsInstanceOfType<ITypeRegistrar>(registrar);
    }
}
