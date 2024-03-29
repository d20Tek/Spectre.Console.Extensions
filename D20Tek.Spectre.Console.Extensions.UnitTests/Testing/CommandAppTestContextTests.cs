﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console.Cli;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing
{
    [TestClass]
    public class CommandAppTestContextTests
    {
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
            Assert.AreEqual("test", result.Context.Name);
            Assert.IsInstanceOfType(result.Settings, typeof(EmptyCommandSettings));
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
            var result = context.RunWithException<ArgumentOutOfRangeException>(
                new string[] { "fail" });

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
            var result = context.RunWithException<ArgumentOutOfRangeException>(
                new string[] { "succeeds" });
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
            var result = context.RunWithException<InvalidOperationException>(
                new string[] { "fail" });
        }

        [TestMethod]
        public async Task RunAsync()
        {
            // arrange
            var context = new CommandAppTestContext();
            context.Configure(config =>
            {
                config.Settings.ApplicationName = "Run Test 1";
                config.AddCommand<MockAsyncCommand>("test");
            });

            // act
            var result = await context.RunAsync(new string[] { "test" });

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ExitCode);
            StringAssert.Contains(result.Output, "Success");
            Assert.AreEqual("test", result.Context.Name);
            Assert.IsInstanceOfType(result.Settings, typeof(EmptyCommandSettings));
        }

        [TestMethod]
        public async Task RunAsync_WithNoConfigAction()
        {
            // arrange
            var context = new CommandAppTestContext();

            // act
            var result = await context.RunAsync(new string[] { "test" });

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(-1, result.ExitCode);
        }

        [TestMethod]
        public async Task RunWithExceptionAsync()
        {
            // arrange
            var context = new CommandAppTestContext();
            context.Configure(config =>
            {
                config.Settings.ApplicationName = "Run Test 3";
                config.AddCommand<MockAsyncCommandWithException>("fail");
            });

            // act
            var result = await context.RunWithExceptionAsync<ArgumentOutOfRangeException>(
                new string[] { "fail" });

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(-1, result.ExitCode);
            StringAssert.Contains(result.Output, "out of the range");
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task RunWithExceptionAsync_NoException()
        {
            // arrange
            var context = new CommandAppTestContext();
            context.Configure(config =>
            {
                config.Settings.ApplicationName = "Run Test 4";
                config.AddCommand<MockAsyncCommand>("succeeds");
            });

            // act
            var result = await context.RunWithExceptionAsync<ArgumentOutOfRangeException>(
                new string[] { "succeeds" });
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task RunWithExceptionAsync_UnexpectedException()
        {
            // arrange
            var context = new CommandAppTestContext();
            context.Configure(config =>
            {
                config.Settings.ApplicationName = "Run Test 5";
                config.AddCommand<MockAsyncCommandWithException>("fail");
            });

            // act
            var result = await context.RunWithExceptionAsync<InvalidOperationException>(
                new string[] { "fail" });
        }
    }
}
