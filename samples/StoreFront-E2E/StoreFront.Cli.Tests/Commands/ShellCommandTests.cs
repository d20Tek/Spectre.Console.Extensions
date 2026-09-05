//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.Extensions.Options;
using Spectre.Console.Cli;
using StoreFront.Cli.Commands;
using StoreFront.Cli.Configuration;
using StoreFront.Cli.Tests.Fakes;

namespace StoreFront.Cli.Tests.Commands;

[TestClass]
public sealed class ShellCommandTests
{
    private static (ShellCommand Command, TestConsole Console, FakeCommandApp App) CreateShell(
        string[] inputLines, StoreOptions? options = null, int appResult = 0)
    {
        var app = new FakeCommandApp(appResult);
        var console = new TestConsole();
        var input = new TestConsoleInput();
        foreach (var line in inputLines)
        {
            input.PushTextWithEnter(line);
        }

        console.TestInput = input;

        var command = new ShellCommand(app, Options.Create(options ?? new StoreOptions()), console);
        return (command, console, app);
    }

    private static async Task<int> RunAsync(ShellCommand command)
    {
        var context = new CommandContext([], NullRemainingArguments.Instance, "shell", null);
        return await ((ICommand)command).ExecuteAsync(context, new EmptyCommandSettings(), CancellationToken.None);
    }

    [TestMethod]
    public async Task Execute_ImmediateExit_ReturnsSuccess()
    {
        // Arrange
        var (command, _, _) = CreateShell(["exit"]);

        // Act
        var result = await RunAsync(command);

        // Assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public async Task Execute_ShowsWelcomeMessageWithStoreName()
    {
        // Arrange
        var (command, console, _) = CreateShell(["exit"], new StoreOptions { Name = "Test Shop" });

        // Act
        await RunAsync(command);

        // Assert
        StringAssert.Contains(console.Output, "Welcome to the StoreFront shell.");
    }

    [TestMethod]
    public async Task Execute_ShowsExitMessageOnExit()
    {
        // Arrange
        var (command, console, _) = CreateShell(["exit"]);

        // Act
        await RunAsync(command);

        // Assert
        StringAssert.Contains(console.Output, "Thanks for visiting StoreFront!");
    }

    [TestMethod]
    public async Task Execute_UsesStorePromptPrefix()
    {
        // Arrange
        var (command, console, _) = CreateShell(["exit"]);

        // Act
        await RunAsync(command);

        // Assert
        StringAssert.Contains(console.Output, "store>");
    }

    [TestMethod]
    public async Task Execute_WithCommandThenExit_ForwardsCommandToApp()
    {
        // Arrange
        var (command, _, app) = CreateShell(["catalog", "exit"]);

        // Act
        var result = await RunAsync(command);

        // Assert
        Assert.AreEqual(0, result);
        Assert.AreEqual(1, app.Invocations.Count);
        var forwarded = app.Invocations[0].Where(a => !string.IsNullOrWhiteSpace(a)).ToArray();
        CollectionAssert.AreEqual(new[] { "catalog" }, forwarded);
    }

    [TestMethod]
    public async Task Execute_WithMultiArgumentCommand_SplitsArguments()
    {
        // Arrange
        var (command, _, app) = CreateShell(["checkout item COF001:2", "exit"]);

        // Act
        await RunAsync(command);

        // Assert
        var forwarded = app.Invocations[0].Where(a => !string.IsNullOrWhiteSpace(a)).ToArray();
        CollectionAssert.AreEqual(
            new[] { "checkout", "item", "COF001:2" }, forwarded);
    }
}
