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
        public void Run_WithoutBuildFirst()
        {
            // arrange
            var builder = new CommandAppBuilder()
                              .WithStartup<MockStartup>()
                              .WithDefaultCommand<MockCommand>();

            // act
            Assert.ThrowsExactly<ArgumentNullException>(() => builder.Run(Array.Empty<string>()));
        }

        [TestMethod]
        public async Task RunAsync_WithoutBuildFirst()
        {
            // arrange
            var builder = new CommandAppBuilder()
                              .WithStartup<MockStartup>()
                              .WithDefaultCommand<MockCommand>();

            // act
            await Assert.ThrowsExactlyAsync<ArgumentNullException>(() => builder.RunAsync(Array.Empty<string>()));
        }
    }
}
