using D20Tek.Spectre.Console.Extensions.Injection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Injection;

[TestClass]
public class LifetimeExtensionsTests
{
    public interface ITestService { };

    public class TestService : ITestService { };

    [TestMethod]
    public void RegisterSingleton_WithType_RegistersInContainer()
    {
        // arrange
        var container = new ServiceCollection();
        var registrar = new DependencyInjectionTypeRegistrar(container);

        // act
        var result = registrar.WithLifetimes().RegisterSingleton<ITestService, TestService>();

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, container.Count);
        Assert.IsTrue(container.Any(x => x.Lifetime == ServiceLifetime.Singleton));
        Assert.IsTrue(container.Any(x => x.ServiceType == typeof(ITestService)));
        Assert.IsTrue(container.Any(x => x.ImplementationType == typeof(TestService)));
    }

    [TestMethod]
    public void RegisterSingleton_WithInstance_RegistersInContainer()
    {
        // arrange
        var container = new ServiceCollection();
        var registrar = new DependencyInjectionTypeRegistrar(container);
        var instance = new TestService();

        // act
        var result = registrar.WithLifetimes().RegisterSingleton<ITestService>(instance);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, container.Count);
        Assert.IsTrue(container.Any(x => x.Lifetime == ServiceLifetime.Singleton));
        Assert.IsTrue(container.Any(x => x.ServiceType == typeof(ITestService)));
        Assert.IsTrue(container.Any(x => x.ImplementationInstance == instance));
    }

    [TestMethod]
    public void RegisterSingleton_WithFactoryMethod_RegistersInContainer()
    {
        // arrange
        var container = new ServiceCollection();
        var registrar = new DependencyInjectionTypeRegistrar(container);

        // act
        registrar.WithLifetimes().RegisterSingleton<ITestService, TestService>(
            [ExcludeFromCodeCoverage] (sp) => new TestService());

        // assert
        Assert.AreEqual(1, container.Count);
        Assert.IsTrue(container.Any(x => x.Lifetime == ServiceLifetime.Singleton));
        Assert.IsTrue(container.Any(x => x.ServiceType == typeof(ITestService)));
        Assert.IsFalse(container.Any(x => x.ImplementationType == typeof(TestService)));
        Assert.IsNotNull(container.First().ImplementationFactory);
    }

    [TestMethod]
    public void RegisterScoped_WithType_RegistersInContainer()
    {
        // arrange
        var container = new ServiceCollection();
        var registrar = new DependencyInjectionTypeRegistrar(container);

        // act
        var result = registrar.WithLifetimes().RegisterScoped<ITestService, TestService>();

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, container.Count);
        Assert.IsTrue(container.Any(x => x.Lifetime == ServiceLifetime.Scoped));
        Assert.IsTrue(container.Any(x => x.ServiceType == typeof(ITestService)));
        Assert.IsTrue(container.Any(x => x.ImplementationType == typeof(TestService)));
    }

    [TestMethod]
    public void RegisterScoped_WithFactoryMethod_RegistersInContainer()
    {
        // arrange
        var container = new ServiceCollection();
        var registrar = new DependencyInjectionTypeRegistrar(container);

        // act
        registrar.WithLifetimes().RegisterScoped<ITestService, TestService>(
            [ExcludeFromCodeCoverage] (sp) => new TestService());

        // assert
        Assert.AreEqual(1, container.Count);
        Assert.IsTrue(container.Any(x => x.Lifetime == ServiceLifetime.Scoped));
        Assert.IsTrue(container.Any(x => x.ServiceType == typeof(ITestService)));
        Assert.IsFalse(container.Any(x => x.ImplementationType == typeof(TestService)));
        Assert.IsNotNull(container.First().ImplementationFactory);
    }

    [TestMethod]
    public void RegisterTransient_WithType_RegistersInContainer()
    {
        // arrange
        var container = new ServiceCollection();
        var registrar = new DependencyInjectionTypeRegistrar(container);

        // act
        var result = registrar.WithLifetimes().RegisterTransient<ITestService, TestService>();

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, container.Count);
        Assert.IsTrue(container.Any(x => x.Lifetime == ServiceLifetime.Transient));
        Assert.IsTrue(container.Any(x => x.ServiceType == typeof(ITestService)));
        Assert.IsTrue(container.Any(x => x.ImplementationType == typeof(TestService)));
    }

    [TestMethod]
    public void RegisterTransient_WithFactoryMethod_RegistersInContainer()
    {
        // arrange
        var container = new ServiceCollection();
        var registrar = new DependencyInjectionTypeRegistrar(container);

        // act
        registrar.WithLifetimes().RegisterTransient<ITestService, TestService>(
            [ExcludeFromCodeCoverage] (sp) => new TestService());

        // assert
        Assert.AreEqual(1, container.Count);
        Assert.IsTrue(container.Any(x => x.Lifetime == ServiceLifetime.Transient));
        Assert.IsTrue(container.Any(x => x.ServiceType == typeof(ITestService)));
        Assert.IsFalse(container.Any(x => x.ImplementationType == typeof(TestService)));
        Assert.IsNotNull(container.First().ImplementationFactory);
    }
}
