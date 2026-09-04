//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Hosting;
using D20Tek.Spectre.Console.Extensions.UnitTests.Hosting.Fakes;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Hosting;

[TestClass]
public class HostTypeResolverTests
{
    [TestMethod]
    public void Constructor_WithNullProvider_ThrowsException()
    {
        // Arrange
        var registrations = new Dictionary<Type, HostRegistration>();

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            new HostTypeResolver(null!, registrations));
    }

    [TestMethod]
    public void Constructor_WithNullRegistrations_ThrowsException()
    {
        // Arrange
        var provider = new ServiceCollection().BuildServiceProvider();

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            new HostTypeResolver(provider, null!));
    }

    [TestMethod]
    public void Resolve_WithNullType_ReturnsNull()
    {
        // Arrange
        var provider = new ServiceCollection().BuildServiceProvider();
        var resolver = new HostTypeResolver(provider, new Dictionary<Type, HostRegistration>());

        // Act
        var result = resolver.Resolve(null);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void Resolve_WithHostRegisteredService_ResolvesFromProvider()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<IGreetingService, GreetingService>();
        var provider = services.BuildServiceProvider();
        var resolver = new HostTypeResolver(provider, new Dictionary<Type, HostRegistration>());

        // Act
        var result = resolver.Resolve(typeof(IGreetingService));

        // Assert
        Assert.IsInstanceOfType<GreetingService>(result);
    }

    [TestMethod]
    public void Resolve_WithMappedType_ConstructsFromHostProvider()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<IGreetingService, GreetingService>();
        services.AddSingleton<global::Spectre.Console.IAnsiConsole>(new Extensions.Testing.TestConsole());
        var provider = services.BuildServiceProvider();
        var registrations = new Dictionary<Type, HostRegistration>
        {
            [typeof(GreetCommand)] = HostRegistration.ForType(typeof(GreetCommand)),
        };
        var resolver = new HostTypeResolver(provider, registrations);

        // Act
        var result = resolver.Resolve(typeof(GreetCommand));

        // Assert - GreetCommand depends on IGreetingService (host) and IAnsiConsole.
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void Resolve_WithUnknownType_ReturnsNull()
    {
        // Arrange
        var provider = new ServiceCollection().BuildServiceProvider();
        var resolver = new HostTypeResolver(provider, new Dictionary<Type, HostRegistration>());

        // Act
        var result = resolver.Resolve(typeof(IGreetingService));

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void Resolve_PrefersHostProviderOverRegistrationMap()
    {
        // Arrange
        var hostInstance = new GreetingService();
        var mapInstance = new GreetingService();
        var services = new ServiceCollection();
        services.AddSingleton<IGreetingService>(hostInstance);
        var provider = services.BuildServiceProvider();
        var registrations = new Dictionary<Type, HostRegistration>
        {
            [typeof(IGreetingService)] = HostRegistration.ForInstance(mapInstance),
        };
        var resolver = new HostTypeResolver(provider, registrations);

        // Act
        var result = resolver.Resolve(typeof(IGreetingService));

        // Assert
        Assert.AreSame(hostInstance, result);
    }

    [TestMethod]
    public void Resolve_WithMappedTypeDependingOnAnotherMappedType_ResolvesFromComposite()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<IGreetingService, GreetingService>();
        var provider = services.BuildServiceProvider();
        var registrations = new Dictionary<Type, HostRegistration>
        {
            // DependentService depends on IGreetingService (host), and IDependentService is
            // itself only in the registration map; the consumer type below depends on it.
            [typeof(IDependentService)] = HostRegistration.ForType(typeof(DependentService)),
        };
        var resolver = new HostTypeResolver(provider, registrations);

        // Act
        var result = resolver.Resolve(typeof(IDependentService));

        // Assert
        Assert.IsInstanceOfType<DependentService>(result);
        Assert.AreEqual("Hello, dependency!", ((IDependentService)result!).Describe());
    }

    [TestMethod]
    public void Resolve_WithChainedMappedRegistrations_SatisfiesNestedMapDependency()
    {
        // Arrange
        var provider = new ServiceCollection().BuildServiceProvider();
        var greeting = new GreetingService();
        var registrations = new Dictionary<Type, HostRegistration>
        {
            // IGreetingService lives ONLY in the map (not in the host provider), and
            // DependentService depends on it. This fails when ActivatorUtilities uses the raw
            // host provider, and succeeds when it uses the composite provider.
            [typeof(IGreetingService)] = HostRegistration.ForInstance(greeting),
            [typeof(IDependentService)] = HostRegistration.ForType(typeof(DependentService)),
        };
        var resolver = new HostTypeResolver(provider, registrations);

        // Act
        var result = resolver.Resolve(typeof(IDependentService));

        // Assert
        Assert.IsInstanceOfType<DependentService>(result);
        Assert.AreEqual("Hello, dependency!", ((IDependentService)result!).Describe());
    }
}
