//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Hosting;
using D20Tek.Spectre.Console.Extensions.UnitTests.Hosting.Fakes;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Hosting;

[TestClass]
public class HostRegistrationTests
{
    [TestMethod]
    public void ForType_WithNullType_ThrowsException()
    {
        // Arrange - Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () => HostRegistration.ForType(null!));
    }

    [TestMethod]
    public void ForInstance_WithNullInstance_ThrowsException()
    {
        // Arrange - Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () => HostRegistration.ForInstance(null!));
    }

    [TestMethod]
    public void ForFactory_WithNullFactory_ThrowsException()
    {
        // Arrange - Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () => HostRegistration.ForFactory(null!));
    }

    [TestMethod]
    public void Resolve_WithNullProvider_ThrowsException()
    {
        // Arrange
        var registration = HostRegistration.ForInstance(new GreetingService());

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () => registration.Resolve(null!));
    }

    [TestMethod]
    public void Resolve_WithInstanceRegistration_ReturnsSameInstance()
    {
        // Arrange
        var instance = new GreetingService();
        var registration = HostRegistration.ForInstance(instance);
        var provider = new ServiceCollection().BuildServiceProvider();

        // Act
        var result = registration.Resolve(provider);

        // Assert
        Assert.AreSame(instance, result);
    }

    [TestMethod]
    public void Resolve_WithFactoryRegistration_InvokesFactory()
    {
        // Arrange
        var instance = new GreetingService();
        var registration = HostRegistration.ForFactory(() => instance);
        var provider = new ServiceCollection().BuildServiceProvider();

        // Act
        var result = registration.Resolve(provider);

        // Assert
        Assert.AreSame(instance, result);
    }

    [TestMethod]
    public void Resolve_WithTypeRegistration_ConstructsUsingProviderDependencies()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<IGreetingService, GreetingService>();
        var provider = services.BuildServiceProvider();
        var registration = HostRegistration.ForType(typeof(DependentService));

        // Act
        var result = registration.Resolve(provider);

        // Assert
        Assert.IsInstanceOfType<DependentService>(result);
        Assert.IsNotNull(((DependentService)result).GreetingService);
    }

    private sealed class DependentService(IGreetingService greetingService)
    {
        public IGreetingService GreetingService { get; } = greetingService;
    }
}
