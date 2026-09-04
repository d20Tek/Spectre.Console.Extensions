//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Hosting;
using D20Tek.Spectre.Console.Extensions.Testing;
using D20Tek.Spectre.Console.Extensions.UnitTests.Hosting.Fakes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Hosting;

[TestClass]
public class HostStartupIntegrationTests
{
    private static IHost CreateHostWithStartup(IAnsiConsole console)
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddSingleton(console);
        builder.WithStartup<GreetStartup>();
        return builder.Build();
    }

    [TestMethod]
    public void HostCommandAppBuilder_WithStartup_AppliesConfiguredCommands()
    {
        // Arrange
        var console = new TestConsole();
        using var host = CreateHostWithStartup(console);

        // Act - the "greet" command was registered by the startup's ConfigureCommands.
        var result = host.CreateCommandAppBuilder().Run(["greet"]);

        // Assert
        Assert.AreEqual(0, result);
        Assert.Contains("Hello, World!", console.Output);
    }

    [TestMethod]
    public async Task HostCommandAppBuilder_WithStartup_AppliesConfiguredCommandsAsync()
    {
        // Arrange
        var console = new TestConsole();
        using var host = CreateHostWithStartup(console);

        // Act
        var result = await host.CreateCommandAppBuilder().RunAsync(["greet"]);

        // Assert
        Assert.AreEqual(0, result);
        Assert.Contains("Hello, World!", console.Output);
    }

    [TestMethod]
    public void HostCommandAppBuilder_WithStartupAndAdditionalCommands_AppliesBoth()
    {
        // Arrange
        var console = new TestConsole();
        using var host = CreateHostWithStartup(console);

        // Act - startup registers "greet"; ConfigureCommands adds another alias.
        var result = host.CreateCommandAppBuilder()
            .ConfigureCommands(config => config.AddCommand<GreetCommand>("hello"))
            .Run(["hello"]);

        // Assert
        Assert.AreEqual(0, result);
        Assert.Contains("Hello, World!", console.Output);
    }

    [TestMethod]
    public void CreateCommandApp_WithStartup_AppliesConfiguredCommands()
    {
        // Arrange
        var console = new TestConsole();
        using var host = CreateHostWithStartup(console);

        // Act
        var app = host.CreateCommandApp(config => config.AddCommand<GreetCommand>("hello"));
        var result = app.Run(["greet"], CancellationToken.None);

        // Assert
        Assert.AreEqual(0, result);
        Assert.Contains("Hello, World!", console.Output);
    }

    [TestMethod]
    public void RunCommandApp_WithStartup_AppliesConfiguredCommands()
    {
        // Arrange
        var console = new TestConsole();
        using var host = CreateHostWithStartup(console);

        // Act
        var result = host.RunCommandApp(
            ["greet"],
            config => config.AddCommand<GreetCommand>("hello"));

        // Assert
        Assert.AreEqual(0, result);
        Assert.Contains("Hello, World!", console.Output);
    }

    [TestMethod]
    public void WithStartup_InvokesBothStartupPhases()
    {
        // Arrange
        var console = new TestConsole();
        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddSingleton<IAnsiConsole>(console);
        builder.WithStartup<GreetStartup>();
        using var host = builder.Build();

        // Act
        host.CreateCommandAppBuilder().Run(["greet"]);

        // Assert - both phases ran on the same registered startup instance.
        var startup = (GreetStartup)host.Services.GetRequiredService<HostStartupBase>();
        Assert.IsTrue(startup.ConfigureServicesCalled);
        Assert.IsTrue(startup.ConfigureCommandsCalled);
    }
}
