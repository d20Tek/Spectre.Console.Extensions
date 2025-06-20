using D20Tek.Spectre.Console.Extensions.Controls;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class HistoryTextPromptHistoryTests
{
    [TestMethod]
    public void ShowPrompt_WithHistoryPrevious_ReturnsSecondString()
    {
        // arrange
        string[] history = ["Command1", "Command2", "Command3"];
        var console = new TestConsole();
        console.TestInput.PushKey(System.ConsoleKey.UpArrow);
        console.TestInput.PushKey(System.ConsoleKey.UpArrow);
        console.TestInput.PushKey(System.ConsoleKey.Enter);

        // act
        var result = console.Prompt(
            new HistoryTextPrompt<string>("enter:")
                .AddHistory(history));

        // assert
        Assert.AreEqual("Command2", result);
    }

    [TestMethod]
    public void ShowPrompt_WithHistoryPreviousPastStart_ReturnsFirstString()
    {
        // arrange
        string[] history = ["Command1", "Command2"];
        var console = new TestConsole();
        console.TestInput.PushKey(System.ConsoleKey.UpArrow);
        console.TestInput.PushKey(System.ConsoleKey.UpArrow);
        console.TestInput.PushKey(System.ConsoleKey.UpArrow);
        console.TestInput.PushKey(System.ConsoleKey.Enter);

        // act
        var result = console.Prompt(
            new HistoryTextPrompt<string>("enter:")
                .AddHistory(history));

        // assert
        Assert.AreEqual("Command1", result);
    }

    [TestMethod]
    public void ShowPrompt_WithHistoryNext_ReturnsThirdString()
    {
        // arrange
        string[] history = ["Command1", "Command2", "Command3"];
        var console = new TestConsole();
        console.TestInput.PushKey(System.ConsoleKey.UpArrow);
        console.TestInput.PushKey(System.ConsoleKey.UpArrow);
        console.TestInput.PushKey(System.ConsoleKey.DownArrow);
        console.TestInput.PushKey(System.ConsoleKey.Enter);

        // act
        var result = console.Prompt(
            new HistoryTextPrompt<string>("enter:")
                .AddHistory(history));

        // assert
        Assert.AreEqual("Command3", result);
    }

    [TestMethod]
    public void ShowPrompt_WithHistoryNextPastEnd_MovesToEmptyString()
    {
        // arrange
        string[] history = ["Command1", "Command2", "Command3"];
        var console = new TestConsole();
        console.TestInput.PushKey(System.ConsoleKey.UpArrow);
        console.TestInput.PushKey(System.ConsoleKey.UpArrow);
        console.TestInput.PushKey(System.ConsoleKey.DownArrow);
        console.TestInput.PushKey(System.ConsoleKey.DownArrow);
        console.TestInput.PushKey(System.ConsoleKey.Enter);

        // act
        var result = console.Prompt(
            new HistoryTextPrompt<string>("enter:")
                .AddHistory(history)
                .AllowEmpty());

        // assert
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void ShowPrompt_WithHistoryNextAndText_MovesToEnd()
    {
        // arrange
        string[] history = ["Command1", "Command2", "Command3"];
        var console = new TestConsole();
        console.TestInput.PushText("foo");
        console.TestInput.PushKey(System.ConsoleKey.UpArrow);
        console.TestInput.PushKey(System.ConsoleKey.DownArrow);
        console.TestInput.PushKey(System.ConsoleKey.Enter);

        // act
        var result = console.Prompt(
            new HistoryTextPrompt<string>("enter:")
                .AddHistory(history)
                .AllowEmpty());

        // assert
        Assert.AreEqual("foo", result);
    }
}
