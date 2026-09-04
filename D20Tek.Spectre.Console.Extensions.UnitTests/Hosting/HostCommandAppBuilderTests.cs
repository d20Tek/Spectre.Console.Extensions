//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Hosting;
using D20Tek.Spectre.Console.Extensions.Testing;
using D20Tek.Spectre.Console.Extensions.UnitTests.Hosting.Fakes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Hosting;

[TestClass]
public class HostCommandAppBuilderTests
{
    private static IHost CreateHost(IAnsiConsole console)
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddSingleton<IGreetingService, GreetingService>();
        builder.Services.AddSingleton(console);
        return builder.Build();
    }

    [TestMethod]
    public void Constructor_WithNullHost_ThrowsException()
    {
        // Arrange - Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            new HostCommandAppBuilder(null!));
    }

    [TestMethod]
    public void Host_ReturnsProvidedHost()
    {
        // Arrange
        using var host = CreateHost(new TestConsole());

        // Act
        var builder = new HostCommandAppBuilder(host);

        // Assert
        Assert.AreSame(host, builder.Host);
    }

    [TestMethod]
    public void ConfigureCommands_WithNullConfigure_ThrowsException()
    {
        // Arrange
        using var host = CreateHost(new TestConsole());
        var builder = new HostCommandAppBuilder(host);

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            builder.ConfigureCommands(null!));
    }

    [TestMethod]
    public async Task RunAsync_WithNullArgs_ThrowsException()
    {
        // Arrange
        using var host = CreateHost(new TestConsole());
        var builder = new HostCommandAppBuilder(host);

        // Act - Assert
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            [ExcludeFromCodeCoverage] () => builder.RunAsync(null!));
    }

    [TestMethod]
    public void Run_WithNullArgs_ThrowsException()
    {
        // Arrange
        using var host = CreateHost(new TestConsole());
        var builder = new HostCommandAppBuilder(host);

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            builder.Run(null!));
    }

    [TestMethod]
    public void Build_ReturnsSameBuilderInstance()
    {
        // Arrange
        using var host = CreateHost(new TestConsole());
        var builder = new HostCommandAppBuilder(host);

        // Act
        var result = builder.WithDefaultCommand<GreetCommand>().Build();

        // Assert
        Assert.AreSame(builder, result);
    }

    [TestMethod]
    public async Task RunAsync_WithDefaultCommand_ResolvesFromHostAndSucceeds()
    {
        // Arrange
        var console = new TestConsole();
        using var host = CreateHost(console);
        var builder = new HostCommandAppBuilder(host).WithDefaultCommand<GreetCommand>();

        // Act
        var result = await builder.RunAsync([]);

        // Assert
        Assert.AreEqual(0, result);
        Assert.Contains("Hello, World!", console.Output);
    }

    [TestMethod]
    public void Run_WithConfigureCommands_ResolvesFromHostAndSucceeds()
    {
        // Arrange
        var console = new TestConsole();
        using var host = CreateHost(console);
        var builder = new HostCommandAppBuilder(host)
            .ConfigureCommands(config => config.AddCommand<GreetCommand>("greet"));

        // Act
        var result = builder.Run(["greet"]);

        // Assert
        Assert.AreEqual(0, result);
        Assert.Contains("Hello, World!", console.Output);
    }

    [TestMethod]
    public async Task RunAsync_WithoutExplicitBuild_BuildsAutomatically()
    {
        // Arrange
        var console = new TestConsole();
        using var host = CreateHost(console);
        var builder = new HostCommandAppBuilder(host).WithDefaultCommand<GreetCommand>();

        // Act
        var result = await builder.RunAsync([]);

        // Assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Run_AfterExplicitBuild_ReusesBuiltApp()
    {
        // Arrange
        var console = new TestConsole();
        using var host = CreateHost(console);
        var builder = new HostCommandAppBuilder(host)
            .WithDefaultCommand<GreetCommand>()
            .Build();

        // Act
        var result = builder.Run([]);

        // Assert
        Assert.AreEqual(0, result);
        Assert.Contains("Hello, World!", console.Output);
    }
}
