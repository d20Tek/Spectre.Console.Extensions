using D20Tek.Spectre.Console.Extensions.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class CurrencyPromptValidationTests
{
    [TestMethod]
    public void ValidateCurrency_ValidInput_ReturnsSuccess()
    {
        // Arrange
        var prompt = new CurrencyPrompt("Enter amount:");

        // Act
        var result = prompt.ValidateCurrency("123.45");

        // Assert
        Assert.IsTrue(result.Successful);
    }

    [TestMethod]
    public void ValidateCurrency_InvalidInput_ReturnsError()
    {
        // Arrange
        var prompt = new CurrencyPrompt("Enter amount:");

        // Act
        var result = prompt.ValidateCurrency("abc");

        // Assert
        Assert.IsFalse(result.Successful);
    }

    [TestMethod]
    public void ValidateCurrency_UsesCultureSuccessfully()
    {
        // Arrange
        var prompt = new CurrencyPrompt("Enter amount:")
        {
            Culture = new CultureInfo("fr-FR")
        };

        // Act
        var result = prompt.ValidateCurrency("123,45");

        // Assert
        Assert.IsTrue(result.Successful);
    }

    [TestMethod]
    public void ValidateCurrency_EmptyInputWithDefault_ReturnsSuccess()
    {
        // Arrange
        var prompt = new CurrencyPrompt("Enter amount:").WithDefaultValue(50.00m);

        // Act
        var result = prompt.ValidateCurrency(string.Empty);

        // Assert
        Assert.IsTrue(result.Successful);
    }

    [TestMethod]
    public void ValidateCurrency_InputBelowMin_ReturnsError()
    {
        // Arrange
        var prompt = new CurrencyPrompt("Enter amount:").WithMinValue(100.00m);

        // Act
        var result = prompt.ValidateCurrency("50");

        // Assert
        Assert.IsFalse(result.Successful);
    }

    [TestMethod]
    public void ValidateCurrency_InputAboveMax_ReturnsError()
    {
        // Arrange
        var prompt = new CurrencyPrompt("Enter amount:").WithMaxValue(100.00m);

        // Act
        var result = prompt.ValidateCurrency("200");

        // Assert
        Assert.IsFalse(result.Successful);
    }
}
