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
public class HostCommandAppExtensionsTests
{
    private static IHost CreateHost(IAnsiConsole console)
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddSingleton<IGreetingService, GreetingService>();
        builder.Services.AddSingleton(console);
        return builder.Build();
    }

    [TestMethod]
    public void CreateCommandApp_WithNullHost_ThrowsException()
    {
        // Arrange - Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            HostCommandAppExtensions.CreateCommandApp(null!, _ => { }));
    }

    [TestMethod]
    public void CreateCommandApp_WithNullConfigure_ThrowsException()
    {
        // Arrange
        using var host = CreateHost(new TestConsole());

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            host.CreateCommandApp(null!));
    }

    [TestMethod]
    public void CreateCommandAppBuilder_WithNullHost_ThrowsException()
    {
        // Arrange - Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            HostCommandAppExtensions.CreateCommandAppBuilder(null!));
    }

    [TestMethod]
    public void CreateCommandAppBuilder_WithHost_ReturnsBuilder()
    {
        // Arrange
        using var host = CreateHost(new TestConsole());

        // Act
        var builder = host.CreateCommandAppBuilder();

        // Assert
        Assert.IsInstanceOfType<HostCommandAppBuilder>(builder);
        Assert.AreSame(host, builder.Host);
    }

    [TestMethod]
    public async Task RunCommandAppAsync_WithNullHost_ThrowsException()
    {
        // Arrange - Act - Assert
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            [ExcludeFromCodeCoverage] () =>
                HostCommandAppExtensions.RunCommandAppAsync(null!, [], _ => { }));
    }

    [TestMethod]
    public async Task RunCommandAppAsync_WithNullArgs_ThrowsException()
    {
        // Arrange
        using var host = CreateHost(new TestConsole());

        // Act - Assert
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            [ExcludeFromCodeCoverage] () => host.RunCommandAppAsync(null!, [ExcludeFromCodeCoverage](_) => { }));
    }

    [TestMethod]
    public async Task RunCommandAppAsync_WithNullConfigure_ThrowsException()
    {
        // Arrange
        using var host = CreateHost(new TestConsole());

        // Act - Assert
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            [ExcludeFromCodeCoverage] () => host.RunCommandAppAsync([], null!));
    }

    [TestMethod]
    public async Task RunCommandAppAsync_WithDefaultCommand_ResolvesFromHostAndSucceeds()
    {
        // Arrange
        var console = new TestConsole();
        using var host = CreateHost(console);

        // Act
        var result = await host.RunCommandAppAsync(
            ["greet"],
            config => config.AddCommand<GreetCommand>("greet"));

        // Assert
        Assert.AreEqual(0, result);
        Assert.Contains("Hello, World!", console.Output);
    }

    [TestMethod]
    public void RunCommandApp_WithNullHost_ThrowsException()
    {
        // Arrange - Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            HostCommandAppExtensions.RunCommandApp(null!, [], _ => { }));
    }

    [TestMethod]
    public void RunCommandApp_WithNullArgs_ThrowsException()
    {
        // Arrange
        using var host = CreateHost(new TestConsole());

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            host.RunCommandApp(null!, [ExcludeFromCodeCoverage](_) => { }));
    }

    [TestMethod]
    public void RunCommandApp_WithNullConfigure_ThrowsException()
    {
        // Arrange
        using var host = CreateHost(new TestConsole());

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            host.RunCommandApp([], null!));
    }

    [TestMethod]
    public void RunCommandApp_WithDefaultCommand_ResolvesFromHostAndSucceeds()
    {
        // Arrange
        var console = new TestConsole();
        using var host = CreateHost(console);

        // Act
        var result = host.RunCommandApp(
            ["greet"],
            config => config.AddCommand<GreetCommand>("greet"));

        // Assert
        Assert.AreEqual(0, result);
        Assert.Contains("Hello, World!", console.Output);
    }
}
