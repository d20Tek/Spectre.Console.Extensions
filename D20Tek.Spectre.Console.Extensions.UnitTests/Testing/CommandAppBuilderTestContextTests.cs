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
        Assert.Contains("Success", result.Output);
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
        var result = context.RunWithException<ArgumentOutOfRangeException>(["mock"]);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(-1, result.ExitCode);
        Assert.Contains("out of the range", result.Output);
    }

    [TestMethod]
    public void RunWithException_NoException()
    {
        // arrange
        var context = new CommandAppBuilderTestContext();
        context.Builder.WithDIContainer()
                       .WithStartup<MockStartupWithConfig>();

        // act
        Assert.ThrowsExactly<InvalidOperationException>([ExcludeFromCodeCoverage] () =>
            context.RunWithException<ArgumentOutOfRangeException>(["mock"]));
    }

    [TestMethod]
    public void RunWithException_UnexpectedException()
    {
        // arrange
        var context = new CommandAppBuilderTestContext();
        context.Builder.WithDIContainer()
                       .WithStartup<MockStartupWithExceptionCommand>();

        // act
        Assert.ThrowsExactly<InvalidOperationException>([ExcludeFromCodeCoverage] () => 
            context.RunWithException<InvalidOperationException>(["mock"]));
    }

    [TestMethod]
    public async Task RunAsync()
    {
        // arrange
        var context = new CommandAppBuilderTestContext();
        context.Builder.WithDIContainer()
                       .WithStartup<MockAsyncStartupWithConfig>();

        // act
        var result = await context.RunAsync(new string[] { "mock" });

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.ExitCode);
        Assert.Contains("Success", result.Output);
        Assert.AreEqual("mock", result.Context.Name);
        Assert.IsInstanceOfType(result.Settings, typeof(EmptyCommandSettings));
    }

    [TestMethod]
    public async Task RunWithExceptionAsync()
    {
        // arrange
        var context = new CommandAppBuilderTestContext();
        context.Builder.WithDIContainer()
                       .WithStartup<MockAsyncStartupWithExceptionCommand>();

        // act
        var result = await context.RunWithExceptionAsync<ArgumentOutOfRangeException>(
            new string[] { "mock" });

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
        var context = new CommandAppBuilderTestContext();
        context.Builder.WithDIContainer()
                       .WithStartup<MockAsyncStartupWithConfig>();

        // act
        await Assert.ThrowsExactlyAsync<InvalidOperationException>(() => 
            context.RunWithExceptionAsync<ArgumentOutOfRangeException>(["mock"]));
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public async Task RunWithExceptionAsync_UnexpectedException()
    {
        // arrange
        var context = new CommandAppBuilderTestContext();
        context.Builder.WithDIContainer()
                       .WithStartup<MockAsyncStartupWithExceptionCommand>();

        // act
        await Assert.ThrowsExactlyAsync<InvalidOperationException>(() => 
            context.RunWithExceptionAsync<InvalidOperationException>(["mock"]));
    }
}
