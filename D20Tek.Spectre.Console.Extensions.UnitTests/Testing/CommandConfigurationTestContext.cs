//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console.Cli;
using System.Linq;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing
{
    [TestClass]
    public class CommandConfigurationTestContextTests
    {
        [TestMethod]
        public void CreateDefault()
        {
            // arrange

            // act 
            var context = new CommandConfigurationTestContext();

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
            var context = new CommandConfigurationTestContext();
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
            var context = new CommandConfigurationTestContext();
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
    }
}
