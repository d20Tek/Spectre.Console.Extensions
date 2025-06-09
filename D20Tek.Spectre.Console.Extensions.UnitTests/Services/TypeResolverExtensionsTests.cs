//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Injection;
using D20Tek.Spectre.Console.Extensions.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Services;

[TestClass]
public class TypeResolverExtensionsTests
{
    public interface ITestService { };

    public class TestService : ITestService { };

    [TestMethod]
    public void GetService_WithRegisteredType_ReturnsServiceInstance()
    {
        // arrange
        var services = new ServiceCollection();
        var registrar = new DependencyInjectionTypeRegistrar(services);

        registrar.Register(typeof(ITestService), typeof(TestService));
        var resolver = registrar.Build();

        // act
        var service = resolver.GetService<ITestService>();

        // assert
        Assert.IsNotNull(service);
        Assert.IsInstanceOfType<ITestService>(service);
        Assert.IsInstanceOfType<TestService>(service);
    }

    [TestMethod]
    public void GetService_WithUnregisteredType_ReturnsNull()
    {
        // arrange
        var services = new ServiceCollection();
        using var resolver = new DependencyInjectionTypeResolver(services.BuildServiceProvider());

        // act
        var service = resolver.GetService<ITestService>();

        // assert
        Assert.IsNull(service);
    }

    [TestMethod]
    public void GetRequiredService_WithRegisteredType_ReturnsServiceInstance()
    {
        // arrange
        var services = new ServiceCollection();
        var registrar = new DependencyInjectionTypeRegistrar(services);

        registrar.Register(typeof(ITestService), typeof(TestService));
        var resolver = registrar.Build();

        // act
        var service = resolver.GetRequiredService<ITestService>();

        // assert
        Assert.IsNotNull(service);
        Assert.IsInstanceOfType<ITestService>(service);
        Assert.IsInstanceOfType<TestService>(service);
    }

    [TestMethod]
    public void GetRequiredService_WithUnregisteredType_ThrowsException()
    {
        // arrange
        var services = new ServiceCollection();
        using var resolver = new DependencyInjectionTypeResolver(services.BuildServiceProvider());

        // act - assert
        Assert.Throws<InvalidOperationException>(resolver.GetRequiredService<ITestService>);
    }
}
