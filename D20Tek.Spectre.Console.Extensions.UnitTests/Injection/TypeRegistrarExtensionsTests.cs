using D20Tek.Spectre.Console.Extensions.Injection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Injection;

[TestClass]
public class TypeRegistrarExtensionsTests
{
    [TestMethod]
    public void WithLifetimes_WithDIContainer_ReturnsInterface()
    {
        // arrange
        var registrar = new DependencyInjectionTypeRegistrar(new ServiceCollection());

        // act
        var result = registrar.WithLifetimes();

        // assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<ISupportLifetimes>(result);
    }

    [TestMethod]
    public void WithLifetimes_WithLightInjectContainer_ThrowsException()
    {
        // arrange
        var registrar = new LightInjectTypeRegistrar(new LightInject.ServiceContainer());

        // act - assert
        Assert.Throws<InvalidOperationException>(registrar.WithLifetimes);
    }
}
