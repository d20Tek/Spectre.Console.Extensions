using D20Tek.Spectre.Console.Extensions.Controls;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class HistoryTextPromptMiscTests
{
    [TestMethod]
    public void ShowPrompt_WithLeftArrow_MovesCursor()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushText("123");
        console.TestInput.PushKey(System.ConsoleKey.LeftArrow);
        console.TestInput.PushKey(System.ConsoleKey.LeftArrow);
        console.TestInput.PushTextWithEnter("x");

        // act
        var result = console.Prompt(new HistoryTextPrompt<string>("x:"));

        // assert
        Assert.AreEqual("1x23", result);
    }

    [TestMethod]
    public void ShowPrompt_WithLeftArrowPastStart_MovesCursorToStart()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushText("123");
        console.TestInput.PushKey(System.ConsoleKey.LeftArrow);
        console.TestInput.PushKey(System.ConsoleKey.LeftArrow);
        console.TestInput.PushKey(System.ConsoleKey.LeftArrow);
        console.TestInput.PushKey(System.ConsoleKey.LeftArrow);
        console.TestInput.PushTextWithEnter("x");

        // act
        var result = console.Prompt(new HistoryTextPrompt<string>("x:"));

        // assert
        Assert.AreEqual("x123", result);
    }

    [TestMethod]
    public void ShowPrompt_WithRightArrow_MovesCursor()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushText("1234");
        console.TestInput.PushKey(System.ConsoleKey.LeftArrow);
        console.TestInput.PushKey(System.ConsoleKey.LeftArrow);
        console.TestInput.PushKey(System.ConsoleKey.RightArrow);
        console.TestInput.PushTextWithEnter("x");

        // act
        var result = console.Prompt(new HistoryTextPrompt<string>("x:"));

        // assert
        Assert.AreEqual("123x4", result);
    }

    [TestMethod]
    public void ShowPrompt_WithRightArrowPastEnd_MovesCursor()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushText("1234");
        console.TestInput.PushKey(System.ConsoleKey.LeftArrow);
        console.TestInput.PushKey(System.ConsoleKey.RightArrow);
        console.TestInput.PushKey(System.ConsoleKey.RightArrow);
        console.TestInput.PushKey(System.ConsoleKey.RightArrow);
        console.TestInput.PushTextWithEnter("x");

        // act
        var result = console.Prompt(new HistoryTextPrompt<string>("x:"));

        // assert
        Assert.AreEqual("1234x", result);
    }

    [TestMethod]
    public void ShowPrompt_WithBackspace_MovesCursorAndErasesChars()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushText("123");
        console.TestInput.PushKey(System.ConsoleKey.Backspace);
        console.TestInput.PushKey(System.ConsoleKey.Backspace);
        console.TestInput.PushTextWithEnter("x");

        // act
        var result = console.Prompt(new HistoryTextPrompt<string>("x:"));

        // assert
        Assert.AreEqual("1x", result);
    }

    [TestMethod]
    public void ShowPrompt_WithBackspacePastStart_MovesCursorAndErasesChars()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushText("123");
        console.TestInput.PushKey(System.ConsoleKey.Backspace);
        console.TestInput.PushKey(System.ConsoleKey.Backspace);
        console.TestInput.PushKey(System.ConsoleKey.Backspace);
        console.TestInput.PushKey(System.ConsoleKey.Backspace);
        console.TestInput.PushTextWithEnter("x");

        // act
        var result = console.Prompt(new HistoryTextPrompt<string>("x:"));

        // assert
        Assert.AreEqual("x", result);
    }

    [TestMethod]
    public void ShowPrompt_WithMoveLeftAndBackspace_MovesCursorAndErasesChars()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushText("123");
        console.TestInput.PushKey(System.ConsoleKey.LeftArrow);
        console.TestInput.PushKey(System.ConsoleKey.Backspace);
        console.TestInput.PushTextWithEnter("x");

        // act
        var result = console.Prompt(new HistoryTextPrompt<string>("x:"));

        // assert
        Assert.AreEqual("1x3", result);
    }

    [TestMethod]
    public void ShowPrompt_WithMoveLeftAndBackspaceAndSecret_MovesCursorAndErasesChars()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushText("123");
        console.TestInput.PushKey(System.ConsoleKey.LeftArrow);
        console.TestInput.PushKey(System.ConsoleKey.Backspace);
        console.TestInput.PushTextWithEnter("x");

        // act
        var result = console.Prompt(new HistoryTextPrompt<string>("x:").Secret());

        // assert
        Assert.AreEqual("1x3", result);
    }

    [TestMethod]
    public void ShowPrompt_WithTextOverwrite_MovesAndOverwriteText()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushText("123");
        console.TestInput.PushKey(System.ConsoleKey.Insert);
        console.TestInput.PushKey(System.ConsoleKey.F20);
        console.TestInput.PushKey(new System.ConsoleKeyInfo(' ', System.ConsoleKey.F12, true, false, false));
        console.TestInput.PushKey(System.ConsoleKey.LeftArrow);
        console.TestInput.PushTextWithEnter("xyz");

        // act
        var result = console.Prompt(new HistoryTextPrompt<string>("x:"));

        // assert
        Assert.AreEqual("12xyz", result);
    }

    [TestMethod]
    public void ShowPrompt_WithTextOverwriteAtStart_ReturnsText()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushKey(System.ConsoleKey.Insert);
        console.TestInput.PushText("12");
        console.TestInput.PushKey(System.ConsoleKey.Backspace);
        console.TestInput.PushKey(System.ConsoleKey.Backspace);
        console.TestInput.PushTextWithEnter("xyz");

        // act
        var result = console.Prompt(new HistoryTextPrompt<string>("x:"));

        // assert
        Assert.AreEqual("xyz", result);
    }

    [TestMethod]
    public void ShowPrompt_WithControlKey_SkipsKey()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushText("12");
        console.TestInput.PushKey(System.ConsoleKey.Escape);
        console.TestInput.PushTextWithEnter("3");

        // act
        var result = console.Prompt(new HistoryTextPrompt<string>("x:"));

        // assert
        Assert.AreEqual("123", result);
    }
}
