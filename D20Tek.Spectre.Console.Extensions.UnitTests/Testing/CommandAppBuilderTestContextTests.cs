﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console.Cli;
using System;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing
{
    [TestClass]
    public class CommandAppBuilderTestContextTests
    {
        [TestMethod]
        public void Run()
        {
            // arrange
            var context = new CommandAppBuilderTestContext();
            context.Builder.WithDIContainer()
                           .WithStartup<MockStartupWithConfig>();

            // act
            var result = context.Run(new string[] { "mock" });

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "Success");
            Assert.AreEqual("mock", result.Context.Name);
            Assert.IsInstanceOfType(result.Settings, typeof(EmptyCommandSettings));
        }

        [TestMethod]
        public void RunWithException()
        {
            // arrange
            var context = new CommandAppBuilderTestContext();
            context.Builder.WithDIContainer()
                           .WithStartup<MockStartupWithExceptionCommand>();

            // act
            var result = context.RunWithException<ArgumentOutOfRangeException>(new string[] { "mock" });

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
            var context = new CommandAppBuilderTestContext();
            context.Builder.WithDIContainer()
                           .WithStartup<MockStartupWithConfig>();

            // act
            _ = context.RunWithException<ArgumentOutOfRangeException>(new string[] { "mock" });
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RunWithException_UnexpectedException()
        {
            // arrange
            var context = new CommandAppBuilderTestContext();
            context.Builder.WithDIContainer()
                           .WithStartup<MockStartupWithExceptionCommand>();

            // act
            _ = context.RunWithException<InvalidOperationException>(new string[] { "mock" });
        }
    }
}
