using D20Tek.Spectre.Console.Extensions.Controls;
using D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console;
using System;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class HistoryTextPromptChoiceTests
{
    [TestMethod]
    public void ShowPrompt_WithChoicesAndItem_ReturnsString()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("Second");

        // act
        var result = console.Prompt(
            new HistoryTextPrompt<string>("enter choice:")
                .AddChoice("First")
                .AddChoice("Second")
                .AddChoice("Third"));

        // assert
        Assert.AreEqual("Second", result);
    }

    [TestMethod]
    public void ShowPrompt_WithChoicesAndMissingItem_ShowsError()
    {
        // arrange
        var console = new TestConsole();
        string[] choices = [ "First", "Second", "Third" ];
        console.TestInput.PushTextWithEnter("Fourth");
        console.TestInput.PushTextWithEnter("First");

        // act
        var result = console.Prompt(
            new HistoryTextPrompt<string>("enter choice:")
                .AddChoices(choices));

        // assert
        StringAssert.Contains(console.Output, "Invalid choice");
    }

    [TestMethod]
    public void ShowPrompt_WithDontShowChoices_ReturnsString()
    {
        // arrange
        var console = new TestConsole();
        string[] choices = ["First", "Second", "Third"];
        console.TestInput.PushTextWithEnter("First");

        // act
        var result = console.Prompt(
            new HistoryTextPrompt<string>("enter choice:")
                .AddChoices(choices)
                .HideChoices());

        // assert
        Assert.IsFalse(console.Output.Contains("First/Second/Third"));
    }

    [TestMethod]
    public void ShowPrompt_WithShowChoices_ReturnsString()
    {
        // arrange
        var console = new TestConsole();
        string[] choices = ["First", "Second", "Third"];
        console.TestInput.PushTextWithEnter("First");

        // act
        var result = console.Prompt(
            new HistoryTextPrompt<string>("enter choice:")
                .AddChoices(choices)
                .ShowChoices());

        // assert
        StringAssert.Contains(console.Output, "First/Second/Third");
    }

    [TestMethod]
    public void ShowPrompt_WithAutocompleteChoices_ReturnsString()
    {
        // arrange
        var console = new TestConsole();
        string[] choices = ["First", "Second", "Third"];
        console.TestInput.PushText("Fir");
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.Enter);

        // act
        var result = console.Prompt(
            new HistoryTextPrompt<string>("enter choice:")
                .AddChoices(choices));

        // assert
        Assert.AreEqual("First", result);
    }

    [TestMethod]
    public void ShowPrompt_WithAutocompleteChoices_ReturnsClosest()
    {
        // arrange
        var console = new TestConsole();
        string[] choices = ["First", "Second", "Third"];
        console.TestInput.PushText("Se");
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.Enter);

        // act
        var result = console.Prompt(
            new HistoryTextPrompt<string>("enter choice:")
                .AddChoices(choices));

        // assert
        Assert.AreEqual("Second", result);
    }

    [TestMethod]
    public void ShowPrompt_WithAutocompleteChoicesJustTab_ReturnsFirstString()
    {
        // arrange
        var console = new TestConsole();
        string[] choices = ["First", "Second", "Third"];
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.Enter);

        // act
        var result = console.Prompt(
            new HistoryTextPrompt<string>("enter choice:")
                .AddChoices(choices));

        // assert
        Assert.AreEqual("First", result);
    }

    [TestMethod]
    public void ShowPrompt_WithNextAutocompleteChoices_ReturnsString()
    {
        // arrange
        var console = new TestConsole();
        string[] choices = ["First", "Second", "Third"];
        console.TestInput.PushText("Second");
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.Enter);

        // act
        var result = console.Prompt(
            new HistoryTextPrompt<string>("enter choice:")
                .AddChoices(choices));

        // assert
        Assert.AreEqual("Third", result);
    }

    [TestMethod]
    public void ShowPrompt_WithPreviousAutocompleteChoices_ReturnsString()
    {
        // arrange
        var shiftTab = new ConsoleKeyInfo((char)ConsoleKey.Tab, ConsoleKey.Tab, true, false, false);
        var console = new TestConsole();
        string[] choices = ["First", "Second", "Third"];
        console.TestInput.PushText("Third");
        console.TestInput.PushKey(shiftTab);
        console.TestInput.PushKey(ConsoleKey.Enter);

        // act
        var result = console.Prompt(
            new HistoryTextPrompt<string>("enter choice:")
                .AddChoices(choices));

        // assert
        Assert.AreEqual("Second", result);
    }

    [TestMethod]
    public void ShowPrompt_WithChoicesAndDefault_ReturnsString()
    {
        // arrange
        var console = new TestConsole();
        string[] choices = ["First", "Second", "Third"];
        console.TestInput.PushKey(ConsoleKey.Enter);

        // act
        var result = console.Prompt(
            new HistoryTextPrompt<string>("enter choice:")
                .AddChoices(choices)
                .DefaultValue("Third"));

        // assert
        Assert.AreEqual("Third", result);
    }

    [TestMethod]
    public void ShowPrompt_WithAutocompleteChoicesAndNoMatch_ReturnsFirstString()
    {
        // arrange
        var console = new TestConsole();
        string[] choices = ["First", "Second", "Third"];
        console.TestInput.PushText("Thrd");
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.Enter);

        // act
        var result = console.Prompt(
            new HistoryTextPrompt<string>("enter choice:")
                .AddChoices(choices));

        // assert
        Assert.AreEqual("First", result);
    }
}
