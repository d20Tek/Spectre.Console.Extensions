//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;
using DependencyInjection.Cli;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console.Cli;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing
{
    [TestClass]
    public class CommandAppE2ERunnerTests
    {
        [TestMethod]
        public async Task RunAsync_DefaultOperation()
        {
            // arrange

            // act
            var result = await CommandAppE2ERunner.RunAsync(Program.Main, String.Empty);

            // assert
            Assert.AreEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "DependencyInjection.Cli");
            StringAssert.Contains(result.Output, "completed successfully");
        }

        [TestMethod]
        [TestCategory("Console")]
        public async Task RunAsync_DefaultOperation_WithParameters()
        {
            // arrange

            // act
            var result = await CommandAppE2ERunner.RunAsync(Program.Main, "--help");


            // assert
            Assert.AreEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "DependencyInjection.Cli");
        }

        [TestMethod]
        public void Run_DefaultOperation()
        {
            // arrange

            // act
            var result = CommandAppE2ERunner.Run(TestRunMethod, String.Empty);

            // assert
            Assert.AreEqual(0, result.ExitCode);
        }

        private int TestRunMethod(string[] args)
        {
            return 0;
        }
    }
}
