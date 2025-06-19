using D20Tek.Spectre.Console.Extensions.Controls;
using D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console;

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
}
