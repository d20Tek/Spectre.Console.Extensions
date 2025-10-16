using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console.Cli;
using System.Threading.Tasks;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Commands;

[TestClass]
public class InteractiveCommandTests
{
    [TestMethod]
    public async Task Execute_WithExitSubcommand_ExitsApp()
    {
        // arrange
        var app = new FakeCommandApp();
        var console = new TestConsole();
        var input = new TestConsoleInput();
        input.PushTextWithEnter("exit");
        console.TestInput = input;

        var command = new TestInteractivePrompt(app, console);
        var context = new CommandContext([], NullRemainingArguments.Instance, "test", null);

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        Assert.AreEqual(0, result);
        Assert.StartsWith("Test Starting...", console.Output);
        Assert.EndsWith("Test Ending...\n", console.Output);
        Assert.Contains("test>", console.Output);
    }

    [TestMethod]
    public async Task Execute_WithSubcommandThenExit_RunsCommand()
    {
        // arrange
        var app = new FakeCommandApp();
        var console = new TestConsole();
        var input = new TestConsoleInput();
        input.PushTextWithEnter("other-command --foo bar -x");
        input.PushTextWithEnter("exit");
        console.TestInput = input;

        var command = new TestInteractivePrompt(app, console);
        var context = new CommandContext([], NullRemainingArguments.Instance, "test", null);

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        Assert.AreEqual(0, result);
        StringAssert.StartsWith(console.Output, "Test Starting...");
        StringAssert.EndsWith(console.Output, "Test Ending...\n");
    }

    [TestMethod]
    public async Task Execute_WithSubcommandError_Exits()
    { 
        // arrange
        var app = new FakeCommandApp(-1);
        var console = new TestConsole();
        var input = new TestConsoleInput();
        input.PushTextWithEnter("fail-command --foo \"bar\" -x true");
        input.PushTextWithEnter("exit");
        console.TestInput = input;

        var command = new TestInteractivePrompt(app, console);
        var context = new CommandContext([], NullRemainingArguments.Instance, "test", null);

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        Assert.AreEqual(-1, result);
        StringAssert.StartsWith(console.Output, "Test Starting...");
        Assert.DoesNotContain("Test Ending...\n", console.Output);
    }

    [TestMethod]
    public async Task Execute_WithStartSubcommand_DoesNotProcess()
    {
        // arrange
        var app = new FakeCommandApp(-1);
        var console = new TestConsole();
        var input = new TestConsoleInput();
        input.PushTextWithEnter("start");
        input.PushTextWithEnter("exit");
        console.TestInput = input;

        var command = new TestInteractivePrompt(app, console);
        var context = new CommandContext([], NullRemainingArguments.Instance, "test", null);

        // act
        var result = await command.ExecuteAsync(context);

        // assert
        Assert.AreEqual(0, result);
        StringAssert.StartsWith(console.Output, "Test Starting...");
        StringAssert.EndsWith(console.Output, "Test Ending...\n");
    }
}
