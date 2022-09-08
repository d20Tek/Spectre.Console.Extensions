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

        [ExcludeFromCodeCoverage]
        private int TestDelegate(CommandContext arg1, EmptyCommandSettings arg2) => 0;

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
