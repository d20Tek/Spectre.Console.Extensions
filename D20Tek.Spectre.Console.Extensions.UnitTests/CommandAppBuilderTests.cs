//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace D20Tek.Spectre.Console.Extensions.UnitTests
{
    [TestClass]
    public class CommandAppBuilderTests
    {
        [TestMethod]
        public void Constructor()
        {
            // arrange

            // act
            var builder = new CommandAppBuilder();

            // assert
            Assert.IsNotNull(builder);
            Assert.IsNull(builder.Registrar);
            Assert.IsNull(builder.Startup);
            Assert.IsNull(builder.SetDefaultCommand);
        }

        [TestMethod]
        public void WithStartup()
        {
            // arrange
            var builder = new CommandAppBuilder();

            // act
            builder.WithStartup<MockStartup>();

            // assert
            Assert.IsNotNull(builder);
            Assert.IsNull(builder.Registrar);
            Assert.IsNotNull(builder.Startup);
            Assert.IsNull(builder.SetDefaultCommand);
        }

        [TestMethod]
        public void WithDefaultCommand()
        {
            // arrange
            var builder = new CommandAppBuilder();

            // act
            builder.WithDefaultCommand<MockCommand>();

            // assert
            Assert.IsNotNull(builder);
            Assert.IsNull(builder.Registrar);
            Assert.IsNull(builder.Startup);
            Assert.IsNotNull(builder.SetDefaultCommand);
        }

        [TestMethod]
        public void Build_NoTypeRegistrar()
        {
            // arrange
            var builder = new CommandAppBuilder()
                              .WithStartup<MockStartup>();

            // act
            var result = builder.Build();

            // assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Run_NoTypeRegistrar()
        {
            // arrange
            var builder = new CommandAppBuilder()
                              .WithStartup<MockStartup>()
                              .WithDefaultCommand<MockCommand>()
                              .Build();

            // act
            var result = builder.Run(Array.Empty<string>());

            // assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task RunAsync_NoTypeRegistrar()
        {
            // arrange
            var builder = new CommandAppBuilder()
                              .WithStartup<MockStartup>()
                              .WithDefaultCommand<MockCommand>()
                              .Build();

            // act
            var result = await builder.RunAsync(Array.Empty<string>());

            // assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void WithStartupAndRegistrar()
        {
            // arrange
            var builder = new CommandAppBuilder()
                              .WithDIContainer()
                              .WithStartup<MockStartup>();
            // act
            var result = builder.Build();

            // assert
            Assert.IsNotNull(result);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var resolver = result.Registrar.Build();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.IsNotNull(resolver);
            Assert.IsNotNull(resolver.Resolve(typeof(IMockService)));
        }

        [TestMethod]
        public void Run_DIRegistrar()
        {
            // arrange
            var builder = new CommandAppBuilder()
                              .WithDIContainer()
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
            var builder = new CommandAppBuilder()
                              .WithNinjectContainer()
                              .WithStartup<MockStartup>()
                              .WithDefaultCommand<MockCommand>()
                              .Build();

            // act
            var result = builder.Run(Array.Empty<string>());

            // assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Run_SimpleInjectorRegistrar()
        {
            // arrange
            var builder = new CommandAppBuilder()
                              .WithSimpleInjectorContainer()
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
            var builder = new CommandAppBuilder()
                              .WithAutofacContainer()
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
            var builder = new CommandAppBuilder()
                              .WithLightInjectContainer()
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
