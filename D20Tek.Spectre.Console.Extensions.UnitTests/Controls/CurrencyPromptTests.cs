using D20Tek.Spectre.Console.Extensions.Controls;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class CurrencyPromptTests
{
    [TestMethod]
    public void Show_WithValidCurrencyInput_ReturnsParsedValue()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("123.45");
        var prompt = new CurrencyPrompt("Enter amount:");

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual(123.45m, result);
        StringAssert.Contains(console.Output, "Enter amount:");
        StringAssert.Contains(console.Output, "123.45");
    }

    [TestMethod]
    public void Show_WithDefaultValueAndEmptyInput_ReturnsDefault()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushKey(System.ConsoleKey.Enter);
        var prompt = new CurrencyPrompt("Enter amount:").WithDefaultValue(50.00m);

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual(50.00m, result);
        StringAssert.Contains(console.Output, "Enter amount:");
        StringAssert.Contains(console.Output, "50.00");
    }

    [TestMethod]
    public async Task ShowAsync_WithValidCurrencyInput_ReturnsParsedValue()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("789.10");
        var prompt = new CurrencyPrompt("Enter amount:").WithDefaultValue(100M);

        // Act
        var result = await prompt.ShowAsync(console, new CancellationToken());

        // Assert
        Assert.AreEqual(789.10m, result);
        StringAssert.Contains(console.Output, "Enter amount:");
        StringAssert.Contains(console.Output, "789.10");
    }

    [TestMethod]
    public void Show_WithInvalidThenValidInput_DisplaysErrorThenReturnsValue()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("oops");
        console.TestInput.PushTextWithEnter("123.45");
        var prompt = new CurrencyPrompt("Enter amount:").WithErrorMessage("Please enter a valid test currency.");

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual(123.45m, result);
        StringAssert.Contains(console.Output, "Please enter a valid test currency.");
    }

    [TestMethod]
    public void Show_WithCultureAndCurrencyInput_ReturnsParsedValue()
    {
        // Arrange
        var culture = new CultureInfo("fr-FR");
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("123,45");
        var prompt = new CurrencyPrompt("Enter amount:")
            .WithCulture(new CultureInfo("fr-FR"))
            .WithExampleHint(1000M);

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual(123.45m, result);
        StringAssert.Contains(console.Output, "Enter amount:");
        StringAssert.Contains(console.Output, "1 000,00");
        StringAssert.Contains(console.Output, "123,45");
        Assert.AreEqual(culture, prompt.Culture);
    }

    [TestMethod]
    public void Show_WithNullCultureAndCurrencyInput_ReturnsParsedValue()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("123.45");
        var prompt = new CurrencyPrompt("Enter amount:")
        {
            Culture = null
        };

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual(123.45m, result);
        StringAssert.Contains(console.Output, "Enter amount:");
        StringAssert.Contains(console.Output, "123.45");
        Assert.IsNotNull(prompt.Culture);
    }

    [TestMethod]
    public void Show_WithPromptStyle_ReturnsParsedValue()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("123.45");
        var prompt = new CurrencyPrompt("Enter amount:").WithPromptStyle(new Style(decoration: Decoration.Italic));

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual(123.45m, result);
    }

    [TestMethod]
    public void Show_WithCustomValidatorAndValidInput_ReturnsParsedValue()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("123.45");
        var prompt = new CurrencyPrompt("Enter amount:").WithValidator([ExcludeFromCodeCoverage] (input) =>
            input == "123.45" ? ValidationResult.Success() : ValidationResult.Error("Unexpected value"));

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual(123.45m, result);
    }

    [TestMethod]
    public void Show_WithCustomValidatorAndInvalidInput_ReturnsError()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("0.45");
        console.TestInput.PushKey(System.ConsoleKey.Enter);
        var prompt = new CurrencyPrompt("Enter amount:")
            .WithDefaultValue(0)
            .WithValidator([ExcludeFromCodeCoverage] (input) =>
            input == "123.45" ? ValidationResult.Success() : ValidationResult.Error("Unexpected value"));

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual(0m, result);
        StringAssert.Contains(console.Output, "Unexpected value");
    }
}
