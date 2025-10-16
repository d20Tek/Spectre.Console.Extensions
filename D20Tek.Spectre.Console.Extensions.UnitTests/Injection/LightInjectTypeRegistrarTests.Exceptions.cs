//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Injection;
using LightInject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Injection;

[TestClass]
[ExcludeFromCodeCoverage]
public class LightInjectTypeRegistrarExceptionTests
{
    public interface ITestService { };

    public class TestService : ITestService { };

    [TestMethod]
    public void Create_WithNullContainer()
    {
        // arrange

        // act
        Assert.ThrowsExactly<ArgumentNullException>(() => new LightInjectTypeRegistrar(null));
    }

    [TestMethod]
    public void Register_WithNullServiceType()
    {
        // arrange
        var services = new ServiceContainer();
        var registrar = new LightInjectTypeRegistrar(services);

        // act
        Assert.ThrowsExactly<ArgumentNullException>(() => registrar.Register(null, typeof(TestService)));
    }

    [TestMethod]
    public void Register_WithNullImplementationType()
    {
        // arrange
        var services = new ServiceContainer();
        var registrar = new LightInjectTypeRegistrar(services);

        // act
        Assert.ThrowsExactly<ArgumentNullException>(() => registrar.Register(typeof(ITestService), null));
    }

    [TestMethod]
    public void RegisterInstance_WithNullType()
    {
        // arrange
        var services = new ServiceContainer();
        var registrar = new LightInjectTypeRegistrar(services);

        // act
        Assert.ThrowsExactly<ArgumentNullException>(() => registrar.RegisterInstance(null, new TestService()));
    }

    [TestMethod]
    public void RegisterInstance_WithNullImplementation()
    {
        // arrange
        var services = new ServiceContainer();
        var registrar = new LightInjectTypeRegistrar(services);

        // act
        Assert.ThrowsExactly<ArgumentNullException>(() => registrar.RegisterInstance(typeof(ITestService), null));
    }

    [TestMethod]
    public void RegisterLazy_WithNullType()
    {
        // arrange
        var services = new ServiceContainer();
        var registrar = new LightInjectTypeRegistrar(services);

        // act
        Assert.ThrowsExactly<ArgumentNullException>(() => registrar.RegisterLazy(null, null));
    }

    [TestMethod]
    public void RegisterLazy_WithNullFactory()
    {
        // arrange
        var services = new ServiceContainer();
        var registrar = new LightInjectTypeRegistrar(services);

        // act
        Assert.ThrowsExactly<ArgumentNullException>(() => registrar.RegisterLazy(typeof(ITestService), null));
    }
}
