//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Injection;
using D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Injection
{
    [TestClass]
    public class DependencyInjectionFactoryTests
    {
        public class TestCommand : Command
        {
            [ExcludeFromCodeCoverage]
            public override int Execute(CommandContext context)
            {
                return 42;
            }
        }

        [TestMethod]
        public void CreateCommandApp()
        {
            // arrange

            // act
            var app = DependencyInjectionFactory.CreateCommandApp<MockStartup>();

            // assert
            Assert.IsNotNull(app);
        }

        [TestMethod]
        public void CreateCommandApp_WithDefaultCommand()
        {
            // arrange

            // act
            var app = DependencyInjectionFactory.CreateCommandApp<MockStartup, TestCommand>();

            // assert
            Assert.IsNotNull(app);
        }
    }
}
