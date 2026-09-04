//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Hosting;
using D20Tek.Spectre.Console.Extensions.UnitTests.Hosting.Fakes;
using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Hosting;

[TestClass]
public class CompositeServiceProviderTests
{
    [TestMethod]
    public void GetService_WithHostRegisteredService_ResolvesFromHostProvider()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<IGreetingService, GreetingService>();
        var provider = services.BuildServiceProvider();
        var composite = new CompositeServiceProvider(provider, new Dictionary<Type, HostRegistration>());

        // Act
        var result = composite.GetService(typeof(IGreetingService));

        // Assert
        Assert.IsInstanceOfType<GreetingService>(result);
    }

    [TestMethod]
    public void GetService_WithMappedService_ResolvesFromRegistrationMap()
    {
        // Arrange
        var provider = new ServiceCollection().BuildServiceProvider();
        var instance = new GreetingService();
        var registrations = new Dictionary<Type, HostRegistration>
        {
            [typeof(IGreetingService)] = HostRegistration.ForInstance(instance),
        };
        var composite = new CompositeServiceProvider(provider, registrations);

        // Act
        var result = composite.GetService(typeof(IGreetingService));

        // Assert
        Assert.AreSame(instance, result);
    }

    [TestMethod]
    public void GetService_WithUnknownService_ReturnsNull()
    {
        // Arrange
        var provider = new ServiceCollection().BuildServiceProvider();
        var composite = new CompositeServiceProvider(provider, new Dictionary<Type, HostRegistration>());

        // Act
        var result = composite.GetService(typeof(IGreetingService));

        // Assert
        Assert.IsNull(result);
    }
}
