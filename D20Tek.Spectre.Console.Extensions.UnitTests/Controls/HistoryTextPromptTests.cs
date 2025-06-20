using D20Tek.Spectre.Console.Extensions.Controls;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class HistoryTextPromptTests
{
    [TestMethod]
    public void ShowPrompt_WithEnteredText_ReturnsString()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("Test Text!");

        // act
        var result = console.Prompt(new HistoryTextPrompt<string>("enter text:"));

        // assert
        Assert.AreEqual("Test Text!", result);
    }

    [TestMethod]
    public async Task ShowPromptAsync_WithEnteredText_ReturnsString()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("Test Text!");

        // act
        var result = await console.PromptAsync(new HistoryTextPrompt<string>("enter text:"));

        // assert
        Assert.AreEqual("Test Text!", result);
    }

    [TestMethod]
    public void ShowPrompt_WithEnteredInt_ReturnsInt()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("42");

        // act
        var result = console.Prompt(new HistoryTextPrompt<int>("enter text:"));

        // assert
        Assert.AreEqual(42, result);
    }

    [TestMethod]
    public void ShowPrompt_WithEnteredDecimal_ReturnsDecimal()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("12.34");

        // act
        var result = console.Prompt(new HistoryTextPrompt<decimal>("enter text:"));

        // assert
        Assert.AreEqual(12.34M, result);
    }

    [TestMethod]
    public void ShowPrompt_WithInvalidText_ShowsValidationError()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("ninety-nine");
        console.TestInput.PushTextWithEnter("0");

        // act
        var result = console.Prompt(new HistoryTextPrompt<int>("age:"));

        // assert
        StringAssert.Contains(console.Output, "Invalid input");
    }

    [TestMethod]
    public void ShowPrompt_WithDefaultValue_ReturnsString()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushKey(ConsoleKey.Enter);

        // act
        var prompt = new HistoryTextPrompt<string>("enter text:");
        var result = console.Prompt(prompt.DefaultValue("default").ShowDefaultValue());

        // assert
        Assert.AreEqual("default", result);
    }

    [TestMethod]
    public void ShowPrompt_WithSecretMask_ReturnsString()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("secret text");

        // act
        var result = console.Prompt(new HistoryTextPrompt<string>("enter text:").Secret('*'));

        // assert
        Assert.AreEqual("secret text", result);
        StringAssert.Contains(console.Output, "***********");
    }

    [TestMethod]
    public void ShowPrompt_WithSecretAndDefaultValue_ReturnsString()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushKey(ConsoleKey.Enter);

        // act
        var result = console.Prompt(new HistoryTextPrompt<string>("enter text:")
                            .Secret()
                            .DefaultValue("default"));

        // assert
        Assert.AreEqual("default", result);
        StringAssert.Contains(console.Output, "*******");
    }

    [TestMethod]
    public void ShowPrompt_WithAllowEmpty_ReturnsString()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushKey(ConsoleKey.Enter);

        // act
        var result = console.Prompt(new HistoryTextPrompt<string>("enter text:").AllowEmpty());

        // assert
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void ShowPrompt_WithPromptStyle_ReturnsString()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("new style");

        // act
        var result = console.Prompt(new HistoryTextPrompt<string>("enter text:")
                            .PromptStyle(new Style(Color.Black, Color.White)));

        // assert
        Assert.AreEqual("new style", result);
    }

    [TestMethod]
    public void ShowPrompt_WithCustomValidationBool_ShowsErrorMessage()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("10");
        console.TestInput.PushTextWithEnter("42");

        // act
        var result = console.Prompt(
            new HistoryTextPrompt<int>("enter:")
                .ValidationErrorMessage("test error")
                .Validate(x => x >= 18 && x < 100));

        // assert
        StringAssert.Contains(console.Output, "test error");
    }

    [TestMethod]
    public void ShowPrompt_WithCustomValidationSuccess_ReturnsValue()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("42");

        // act
        var result = console.Prompt(
            new HistoryTextPrompt<int>("enter:")
                .ValidationErrorMessage("test error")
                .Validate([ExcludeFromCodeCoverage] (x) => x >= 18 && x < 100));

        // assert
        Assert.AreEqual(42, result);
    }

    [TestMethod]
    public void ShowPrompt_EnterNotAllowedAsksAgain_ReturnsValue()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushKey(ConsoleKey.Enter);
        console.TestInput.PushTextWithEnter("42");

        // act
        var result = console.Prompt(new HistoryTextPrompt<int>("enter:"));

        // assert
        Assert.AreEqual(42, result);
    }

    [TestMethod]
    public void ShowPrompt_WithSetLocale_ReturnsString()
    {
        // arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("Test Text!");

        // act
        var prompt = new HistoryTextPrompt<string>("enter text:")
        {
            Culture = CultureInfo.GetCultureInfo("en-US")
        };
        var result = console.Prompt(prompt);

        // assert
        Assert.AreEqual("Test Text!", result);
    }
}
