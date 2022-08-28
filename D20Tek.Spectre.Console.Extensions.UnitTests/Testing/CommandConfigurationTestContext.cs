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

        [TestMethod]
        public void Configurator_WithFullMetadata()
        {
            // arrange
            var context = new CommandConfigurationTestContext();

            // act
            context.Configurator.AddCommand<MockCommand>("test")
                                .WithAlias("1")
                                .WithData("data")
                                .WithExample(new string[] { "test" })
                                .WithDescription("description");

            // assert
            Assert.AreEqual(1, context.Configurator.Commands.Count());
            var command = context.Configurator.Commands.First();
            Assert.AreEqual("test", command.Name);
            Assert.AreEqual("description", command.Description);
            Assert.AreEqual("data", command.Data);
            Assert.AreEqual(1, command.Aliases.Count);
            Assert.AreEqual(1, command.Examples.Count);
            Assert.AreEqual(typeof(MockCommand), command.CommandType);
            Assert.AreEqual(typeof(EmptyCommandSettings), command.SettingsType);
            Assert.IsFalse(command.IsHidden);
            Assert.IsFalse(command.IsDefaultCommand);
            Assert.IsNull(command.Delegate);
        }

        [TestMethod]
        public void Configurator_WithBranch()
        {
            // arrange
            var context = new CommandConfigurationTestContext();

            // act
            context.Configurator.AddBranch<EmptyCommandSettings>("branch1", c => c.AddCommand<MockCommand>("test"));

            // assert
            Assert.AreEqual(1, context.Configurator.Commands.Count());
            Assert.AreEqual("branch1", context.Configurator.Commands.First().Name);
            Assert.AreEqual(0, context.Configurator.Commands.First().Aliases.Count);
            Assert.AreEqual(0, context.Configurator.Commands.First().Examples.Count);
            Assert.AreEqual(0, context.Configurator.Examples.Count());
            Assert.AreEqual("test", context.Configurator.Commands.First().Children.First().Name);
        }
    }
}
