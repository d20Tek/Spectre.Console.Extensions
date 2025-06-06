using D20Tek.Spectre.Console.Extensions.Injection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Injection;

[TestClass]
public class LifetimeExtensionsExceptionTests
{
    public interface ITestService { };

    public class TestService : ITestService { };

    [TestMethod]
    public void RegisterSingleton_WithNullInstance_ThrowsException()
    {
        // arrange
        var container = new ServiceCollection();
        var registrar = new DependencyInjectionTypeRegistrar(container);
        TestService instance = null;

        // act - assert
        Assert.Throws<ArgumentNullException>([ExcludeFromCodeCoverage]() => 
            registrar.WithLifetimes().RegisterSingleton<ITestService>(instance));
    }

    [TestMethod]
    public void RegisterSingleton_WithNullFactoryMethod_ThrowsException()
    {
        // arrange
        var container = new ServiceCollection();
        var registrar = new DependencyInjectionTypeRegistrar(container);

        // act - assert
        Assert.Throws<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            registrar.WithLifetimes().RegisterSingleton<ITestService, TestService>(null));
    }

    [TestMethod]
    public void RegisterScoped_WithNullFactoryMethod_ThrowsException()
    {
        // arrange
        var container = new ServiceCollection();
        var registrar = new DependencyInjectionTypeRegistrar(container);

        // act - assert
        Assert.Throws<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            registrar.WithLifetimes().RegisterScoped<ITestService, TestService>(null));
    }

    [TestMethod]
    public void RegisterTransient_WithNullFactoryMethod_ThrowsException()
    {
        // arrange
        var container = new ServiceCollection();
        var registrar = new DependencyInjectionTypeRegistrar(container);

        // act - assert
        Assert.Throws<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            registrar.WithLifetimes().RegisterTransient<ITestService, TestService>(null));
    }
}
