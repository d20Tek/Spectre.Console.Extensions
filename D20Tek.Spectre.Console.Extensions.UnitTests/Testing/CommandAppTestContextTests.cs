//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console.Cli;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing
{
    [TestClass]
    public class CommandAppTestContextTests
    {
        [TestMethod]
        public void CreateDefault()
        {
            // arrange

            // act 
            var context = new CommandAppTestContext();

            // assert
            Assert.IsNotNull(context);
            Assert.IsNotNull(context.Registrar);
            Assert.IsNotNull(context.Resolver);
            Assert.IsNotNull(context.Configurator);
            Assert.IsInstanceOfType(context.Registrar, typeof(ITypeRegistrar));
            Assert.IsInstanceOfType(context.Configurator, typeof(IConfigurator));
        }

        [TestMethod]
        public void ConfigureServices()
        {
            // arrange
            var context = new CommandAppTestContext();
            var startup = new MockStartupWithConfig();

            // act
            startup.ConfigureServices(context.Registrar);

            // assert
            Assert.IsInstanceOfType(context.Resolver.Resolve(typeof(IMockService)), typeof(MockService));
        }

        [TestMethod]
        public void ConfigureCommands()
        {
            // arrange
            var context = new CommandAppTestContext();
            var startup = new MockStartupWithConfig();

            // act
            var result = startup.ConfigureCommands(context.Configurator);

            // assert
            Assert.AreEqual(context.Configurator, result);
            Assert.AreEqual("mock-command", context.Configurator.Settings.ApplicationName);
            Assert.AreEqual(1, context.Configurator.Commands.Count());
            Assert.AreEqual("mock", context.Configurator.Commands.First().Name);
            Assert.AreEqual(1, context.Configurator.Commands.First().Aliases.Count);
            Assert.AreEqual(1, context.Configurator.Commands.First().Examples.Count);
            Assert.AreEqual(0, context.Configurator.Examples.Count());
        }

        [TestMethod]
        public void Run()
        {
            // arrange
            var context = new CommandAppTestContext();
            context.Configure(config =>
            {
                config.Settings.ApplicationName = "Run Test 1";
                config.AddCommand<MockCommand>("test");
            });

            // act
            var result = context.Run(new string[] { "test" });

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "Success");
        }

        [TestMethod]
        public void Run_WithNoConfigAction()
        {
            // arrange
            var context = new CommandAppTestContext();

            // act
            var result = context.Run(new string[] { "test" });

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(-1, result.ExitCode);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Run_WithMultipleConfigActions()
        {
            // arrange
            var context = new CommandAppTestContext();
            context.Configure(config =>
            {
                config.Settings.ApplicationName = "Run Test 2";
                config.AddCommand<MockCommand>("test");
            });

            // act
            context.Configure(config =>
                config.AddCommand<MockCommand>("test-2"));
        }

        [TestMethod]
        public void RunWithException()
        {
            // arrange
            var context = new CommandAppTestContext();
            context.Configure(config =>
            {
                config.Settings.ApplicationName = "Run Test 3";
                config.AddCommand<MockCommandWithException>("fail");
            });

            // act
            var result = context.RunWithException<ArgumentOutOfRangeException>(new string[] { "fail" });

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(-1, result.ExitCode);
            StringAssert.Contains(result.Output, "out of the range");
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RunWithException_NoException()
        {
            // arrange
            var context = new CommandAppTestContext();
            context.Configure(config =>
            {
                config.Settings.ApplicationName = "Run Test 4";
                config.AddCommand<MockCommand>("succeeds");
            });

            // act
            var result = context.RunWithException<ArgumentOutOfRangeException>(new string[] { "succeeds" });
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RunWithException_UnexpectedException()
        {
            // arrange
            var context = new CommandAppTestContext();
            context.Configure(config =>
            {
                config.Settings.ApplicationName = "Run Test 5";
                config.AddCommand<MockCommandWithException>("fail");
            });

            // act
            var result = context.RunWithException<InvalidOperationException>(new string[] { "fail" });
        }

        [TestMethod]
        public void BuilderRun()
        {
            // arrange
            var context = new CommandAppTestContext();
            var builder = new CommandAppBuilder()
                .WithDIContainer()
                .WithStartup<MockStartupWithConfig>();

            // act
            var result = context.Run(builder, new string[] { "mock" });

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "Success");
        }
    }
}
