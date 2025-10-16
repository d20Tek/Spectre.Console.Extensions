//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Autofac;
using D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;
using Lamar;
using LightInject;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace D20Tek.Spectre.Console.Extensions.UnitTests;

[TestClass]
public class CommandAppBuilderTests_PreConfiguredContainers
{
    [TestMethod]
    public void Run_DIRegistrar()
    {
        // arrange
        var builder = new CommandAppBuilder().WithDIContainer(new ServiceCollection())
                                             .WithStartup<MockStartup>()
                                             .WithDefaultCommand<MockCommand>()
                                             .Build();

        // act
        var result = builder.Run([]);

        // assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Run_DIRegistrar_WithTransient()
    {
        // arrange
        var services = new ServiceCollection();
        var builder = new CommandAppBuilder().WithDIContainer(services, ServiceLifetime.Transient)
                                             .WithStartup<MockStartup>()
                                             .WithDefaultCommand<MockCommand>()
                                             .Build();

        // act
        var result = builder.Run([]);

        // assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Run_NinjectRegistrar()
    {
        // arrange
        var container = new StandardKernel();
        var builder = new CommandAppBuilder().WithNinjectContainer(container)
                                             .WithStartup<MockStartup>()
                                             .WithDefaultCommand<MockCommand>()
                                             .Build();

        // act
        var result = builder.Run([]);

        // assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Run_AutofacRegistrar()
    {
        // arrange
        var container = new ContainerBuilder();
        var builder = new CommandAppBuilder().WithAutofacContainer(container)
                                             .WithStartup<MockStartup>()
                                             .WithDefaultCommand<MockCommand>()
                                             .Build();

        // act
        var result = builder.Run([]);

        // assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Run_LightInjectRegistrar()
    {
        // arrange
        var container = new ServiceContainer();
        var builder = new CommandAppBuilder().WithLightInjectContainer(container)
                                             .WithStartup<MockStartup>()
                                             .WithDefaultCommand<MockCommand>()
                                             .Build();

        // act
        var result = builder.Run([]);

        // assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Run_LightInjectRegistrar_WithTransient()
    {
        // arrange
        var container = new ServiceContainer();
        var builder = new CommandAppBuilder().WithLightInjectContainer(container, ServiceLifetime.Transient)
                                             .WithStartup<MockStartup>()
                                             .WithDefaultCommand<MockCommand>()
                                             .Build();

        // act
        var result = builder.Run([]);

        // assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Run_LightInjectRegistrar_WithScoped()
    {
        // arrange
        var container = new ServiceContainer();
        var builder = new CommandAppBuilder().WithLightInjectContainer(container, ServiceLifetime.Scoped)
                                             .WithStartup<MockStartup>()
                                             .WithDefaultCommand<MockCommand>()
                                             .Build();

        // act
        var result = builder.Run([]);

        // assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Run_LamarRegistrar()
    {
        // arrange
        var registry = new ServiceRegistry();
        var builder = new CommandAppBuilder().WithLamarContainer(registry)
                                             .WithStartup<MockStartup>()
                                             .WithDefaultCommand<MockCommand>()
                                             .Build();

        // act
        var result = builder.Run([]);

        // assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Run_LamarRegistrar_WithTransient()
    {
        // arrange
        var registry = new ServiceRegistry();
        var builder = new CommandAppBuilder().WithLamarContainer(registry, ServiceLifetime.Transient)
                                             .WithStartup<MockStartup>()
                                             .WithDefaultCommand<MockCommand>()
                                             .Build();

        // act
        var result = builder.Run([]);

        // assert
        Assert.AreEqual(0, result);
    }
}
