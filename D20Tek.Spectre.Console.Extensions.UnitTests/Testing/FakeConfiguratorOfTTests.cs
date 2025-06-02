//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Injection;
using D20Tek.Spectre.Console.Extensions.Testing;
using D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing
{
    [TestClass]
    public class FakeConfiguratorOfTTests
    {
        [TestMethod]
        public void SetBranchProperties()
        {
            // arrange
            var (command, config) = CreateConfigurator();

            // act
            config.SetDescription("test description");
            config.HideBranch();
            config.AddExample(new string[] { "test", "example" });

            // assert
            Assert.AreEqual("test description", command.Description);
            Assert.IsTrue(command.IsHidden);
            Assert.AreEqual(1, command.Examples.Count);
        }

        [TestMethod]
        public void AddCommand()
        {
            // arrange
            var (command, config) = CreateConfigurator();

            // act
            var result = config.AddCommand<MockCommand>("test-command");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, command.Children.Count);
            Assert.AreEqual("test-command", command.Children.First().Name);
        }

        [TestMethod]
        public void SetDefaultCommand()
        {
            // arrange
            var (command, config) = CreateConfigurator();

            // act
            config.SetDefaultCommand<MockCommand>();

            // assert
            Assert.AreEqual(1, command.Children.Count);
            Assert.AreEqual("__default_command", command.Children.First().Name);
        }

        [TestMethod]
        public void AddChildBranch()
        {
            // arrange
            var (command, config) = CreateConfigurator();

            // act
            config.AddBranch<EmptyCommandSettings>("branch-2", TestBranchAction);

            // assert
            Assert.AreEqual(1, command.Children.Count);
            Assert.AreEqual("branch-2", command.Children.First().Name);
        }

        [TestMethod]
        public void AddDelegate()
        {
            // arrange
            var (command, config) = CreateConfigurator();

            // act
            var result = config.AddDelegate<EmptyCommandSettings>("test-delegate", TestDelegate);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, command.Children.Count);
            Assert.AreEqual("test-delegate", command.Children.First().Name);
        }

        [TestMethod]
        public void CallAddedDelegate()
        {
            // arrange
            var (command, config) = CreateConfigurator();

            // act
            var result = config.AddDelegate<EmptyCommandSettings>("test-delegate", TestDelegate);

            // assert
            var configurator = result as FakeCommandConfigurator;
            Assert.IsNotNull(configurator);
            Assert.IsNotNull(configurator.Command.Delegate);
            CommandContext context = new([], NullRemainingArguments.Instance, "test", null);
            Assert.AreEqual(0, configurator.Command.Delegate(context, new EmptyCommandSettings()));
        }

        [TestMethod]
        public void AddAsyncDelegate()
        {
            // arrange
            var (command, config) = CreateConfigurator();

            // act
            var result = config.AddAsyncDelegate<EmptyCommandSettings>("test-async-delegate", TestAsyncDelegate);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, command.Children.Count);
            Assert.AreEqual("test-async-delegate", command.Children.First().Name);
        }

        [TestMethod]
        public async Task CallAddedAsyncDelegate()
        {
            // arrange
            var (command, config) = CreateConfigurator();

            // act
            var result = config.AddAsyncDelegate<EmptyCommandSettings>("test-async-delegate", TestAsyncDelegate);

            // assert
            var configurator = result as FakeCommandConfigurator;
            Assert.IsNotNull(configurator);
            Assert.IsNotNull(configurator.Command.AsyncDelegate);
            CommandContext context = new([], NullRemainingArguments.Instance, "test", null);
            Assert.AreEqual(0, await configurator.Command.AsyncDelegate(context, new EmptyCommandSettings()));
        }

        [ExcludeFromCodeCoverage]
        private int TestDelegate(CommandContext arg1, EmptyCommandSettings arg2) => 0;

        [ExcludeFromCodeCoverage]
        private Task<int> TestAsyncDelegate(CommandContext arg1, EmptyCommandSettings arg2) => Task.FromResult(0);

        [ExcludeFromCodeCoverage]
        private void TestBranchAction(IConfigurator<EmptyCommandSettings> obj) { }

        private (CommandMetadata, IConfigurator<EmptyCommandSettings>) CreateConfigurator()
        {
            var command = CommandMetadata.FromBranch<EmptyCommandSettings>("branch");
            var container = new ServiceCollection();
            var registrar = new DependencyInjectionTypeRegistrar(container);

            return (command, new FakeConfigurator<EmptyCommandSettings>(command, registrar));
        }
    }
}
