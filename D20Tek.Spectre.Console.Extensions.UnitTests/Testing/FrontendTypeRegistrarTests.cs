//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Injection;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing;

[TestClass]
public class FrontendTypeRegistrarTests
{
    [TestMethod]
    public void Register()
    {
        // arrange
        var collection = new ServiceCollection();
        var container = new DependencyInjectionTypeRegistrar(collection);
        var registrar = new FrontendTypeRegistrar(container);

        // act
        registrar.Register<IAnsiConsole, TestConsole>();

        // assert
        Assert.HasCount(1, collection);
        Assert.AreEqual(typeof(IAnsiConsole), collection[0].ServiceType);
        Assert.AreEqual(typeof(TestConsole), collection[0].ImplementationType);
    }

    [TestMethod]
    public void RegisterInstance()
    {
        // arrange
        var collection = new ServiceCollection();
        var container = new DependencyInjectionTypeRegistrar(collection);
        var registrar = new FrontendTypeRegistrar(container);
        var console = new TestConsole();

        // act
        registrar.RegisterInstance<IAnsiConsole, TestConsole>(console);

        // assert
        Assert.HasCount(1, collection);
        Assert.AreEqual(typeof(IAnsiConsole), collection[0].ServiceType);
        Assert.AreEqual(console, collection[0].ImplementationInstance);
    }

    [TestMethod]
    public void RegisterInstance2()
    {
        // arrange
        var collection = new ServiceCollection();
        var container = new DependencyInjectionTypeRegistrar(collection);
        var registrar = new FrontendTypeRegistrar(container);
        var console = new TestConsole();

        // act
        registrar.RegisterInstance<IAnsiConsole>(console);

        // assert
        Assert.HasCount(1, collection);
        Assert.AreEqual(typeof(IAnsiConsole), collection[0].ServiceType);
        Assert.AreEqual(console, collection[0].ImplementationInstance);
    }
}
