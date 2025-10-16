//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Injection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Injection;

[TestClass]
public class NinjectTypeResolverTests
{
    public interface ITestService { };

    public class TestService : ITestService { };

    [TestMethod]
    public void Resolve_WithTypes()
    {
        // arrange
        var services = new StandardKernel();
        var registrar = new NinjectTypeRegistrar(services);

        registrar.Register(typeof(ITestService), typeof(TestService));
        var resolver = registrar.Build();

        // act
        var service = resolver.Resolve(typeof(ITestService));

        // assert
        Assert.IsNotNull(service);
        Assert.IsInstanceOfType<ITestService>(service);
        Assert.IsInstanceOfType<TestService>(service);
    }

    [TestMethod]
    public void Resolve_WithUnregisteredType()
    {
        // arrange
        var services = new StandardKernel();
        using var resolver = new NinjectTypeResolver(services);

        // act
        var service = resolver.Resolve(typeof(NinjectTypeResolver));

        // assert
        Assert.IsNull(service);
    }

    [TestMethod]
    public void Resolve_WithNullType()
    {
        // arrange
        var services = new StandardKernel();
        using var resolver = new NinjectTypeResolver(services);

        // act
        var service = resolver.Resolve(null);

        // assert
        Assert.IsNull(service);
    }

    [TestMethod]
    public void Resolve_WithFactory()
    {
        var services = new StandardKernel();
        var registrar = new NinjectTypeRegistrar(services);

        registrar.RegisterLazy(typeof(ITestService), FactoryMethod);
        var resolver = registrar.Build();

        // act
        var service = resolver.Resolve(typeof(ITestService));

        // assert
        Assert.IsNotNull(service);
        Assert.IsInstanceOfType<ITestService>(service);
        Assert.IsInstanceOfType<TestService>(service);
    }

    private TestService FactoryMethod() => new();

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void Constructor_WithNullServiceCollection()
    {
        // arrange

        // act
        Assert.ThrowsExactly<ArgumentNullException>(() => new NinjectTypeResolver(null));
    }
}
