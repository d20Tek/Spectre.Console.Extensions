//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console.Cli;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing;

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
        var result = context.Run(["test"]);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.ExitCode);
        Assert.Contains("Success", result.Output);
        Assert.AreEqual("test", result.Context.Name);
        Assert.IsInstanceOfType(result.Settings, typeof(EmptyCommandSettings));
    }

    [TestMethod]
    public void Run_WithNoConfigAction()
    {
        // arrange
        var context = new CommandAppTestContext();

        // act
        var result = context.Run(["test"]);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(-1, result.ExitCode);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void Run_WithMultipleConfigActions()
    {
        // arrange
        var context = new CommandAppTestContext();
        context.Configure([ExcludeFromCodeCoverage] (config) =>
        {
            config.Settings.ApplicationName = "Run Test 2";
            config.AddCommand<MockCommand>("test");
        });

        // act
        Assert.ThrowsExactly<InvalidOperationException>([ExcludeFromCodeCoverage] () =>
            context.Configure([ExcludeFromCodeCoverage] (config) => config.AddCommand<MockCommand>("test-2")));
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
        var result = context.RunWithException<ArgumentOutOfRangeException>(["fail"]);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(-1, result.ExitCode);
        Assert.Contains("out of the range", result.Output);
    }

    [TestMethod]
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
        Assert.ThrowsExactly<InvalidOperationException>([ExcludeFromCodeCoverage] () =>
            context.RunWithException<ArgumentOutOfRangeException>(["succeeds"]));
    }

    [TestMethod]
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
        Assert.ThrowsExactly<InvalidOperationException>([ExcludeFromCodeCoverage] () =>
            context.RunWithException<InvalidOperationException>(["fail"]));
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
        var result = await context.RunAsync(["test"]);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.ExitCode);
        Assert.Contains("Success", result.Output);
        Assert.AreEqual("test", result.Context.Name);
        Assert.IsInstanceOfType(result.Settings, typeof(EmptyCommandSettings));
    }

    [TestMethod]
    public async Task RunAsync_WithNoConfigAction()
    {
        // arrange
        var context = new CommandAppTestContext();

        // act
        var result = await context.RunAsync(["test"]);

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
        var result = await context.RunWithExceptionAsync<ArgumentOutOfRangeException>(["fail"]);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(-1, result.ExitCode);
        Assert.Contains("out of the range", result.Output);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
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
        await Assert.ThrowsExactlyAsync<InvalidOperationException>(() =>
            context.RunWithExceptionAsync<ArgumentOutOfRangeException>(["succeeds"]));
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
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
        await Assert.ThrowsExactlyAsync<InvalidOperationException>(() =>
            context.RunWithExceptionAsync<InvalidOperationException>(["fail"]));
    }
}
