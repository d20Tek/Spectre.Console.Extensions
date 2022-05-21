//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace D20Tek.Spectre.Console.Extensions.UnitTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CommandAppBuilderExceptionTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Run_WithoutBuildFirst()
        {
            // arrange
            var builder = new CommandAppBuilder()
                              .WithStartup<MockStartup>()
                              .WithDefaultCommand<MockCommand>();

            // act
            _ = builder.Run(Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RunAsync_WithoutBuildFirst()
        {
            // arrange
            var builder = new CommandAppBuilder()
                              .WithStartup<MockStartup>()
                              .WithDefaultCommand<MockCommand>();

            // act
            _ = await builder.RunAsync(Array.Empty<string>());
        }
    }
}
