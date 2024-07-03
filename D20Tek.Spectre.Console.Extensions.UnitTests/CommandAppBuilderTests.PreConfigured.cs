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
using System;

namespace D20Tek.Spectre.Console.Extensions.UnitTests
{
    [TestClass]
    public class CommandAppBuilderTests_PreConfiguredContainers
    {
        [TestMethod]
        public void Run_DIRegistrar()
        {
            // arrange
            var services = new ServiceCollection();
            var builder = new CommandAppBuilder()
                              .WithDIContainer(services)
                              .WithStartup<MockStartup>()
                              .WithDefaultCommand<MockCommand>()
                              .Build();

            // act
            var result = builder.Run(Array.Empty<string>());

            // assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Run_NinjectRegistrar()
        {
            // arrange
            var container = new StandardKernel();
            var builder = new CommandAppBuilder()
                              .WithNinjectContainer(container)
                              .WithStartup<MockStartup>()
                              .WithDefaultCommand<MockCommand>()
                              .Build();

            // act
            var result = builder.Run(Array.Empty<string>());

            // assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Run_AutofacRegistrar()
        {
            // arrange
            var container = new ContainerBuilder();
            var builder = new CommandAppBuilder()
                              .WithAutofacContainer(container)
                              .WithStartup<MockStartup>()
                              .WithDefaultCommand<MockCommand>()
                              .Build();

            // act
            var result = builder.Run(Array.Empty<string>());

            // assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Run_LightInjectRegistrar()
        {
            // arrange
            var container = new ServiceContainer();
            var builder = new CommandAppBuilder()
                              .WithLightInjectContainer(container)
                              .WithStartup<MockStartup>()
                              .WithDefaultCommand<MockCommand>()
                              .Build();

            // act
            var result = builder.Run(Array.Empty<string>());

            // assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Run_LamarRegistrar()
        {
            // arrange
            var registry = new ServiceRegistry();
            var builder = new CommandAppBuilder()
                              .WithLamarContainer(registry)
                              .WithStartup<MockStartup>()
                              .WithDefaultCommand<MockCommand>()
                              .Build();

            // act
            var result = builder.Run(Array.Empty<string>());

            // assert
            Assert.AreEqual(0, result);
        }
    }
}
