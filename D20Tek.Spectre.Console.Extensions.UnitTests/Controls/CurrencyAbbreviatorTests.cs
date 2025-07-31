using D20Tek.Spectre.Console.Extensions.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class CurrencyAbbreviatorTests
{
    [DataTestMethod]
    [DataRow(1234.5, "$1.23K", "en-US")]
    [DataRow(1234567.9, "$1.23M", "en-US")]
    [DataRow(1234567890, "$1.23B", "en-US")]
    [DataRow(123, "$123", "en-US")]
    [DataRow(1234.5, "£1.23K", "en-GB")]
    [DataRow(1234.5, "1.23K €", "fr-FR")]
    [DataRow(1234.5, "€ 1.23K", "nl-NL")]
    [DataRow(1234.5, "1.23K¤", "twq")]
    public void Render_WithPositiveValue_ReturnsFormattedCurrency(double value, string expected, string cultureName)
    {
        // Arrange
        var culture = new CultureInfo(cultureName);

        // Act
        var result = CurrencyAbbreviator.Format((decimal)value, culture);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [DataTestMethod]
    [DataRow(-1234.5, "($1.23K)", "en-US")]
    [DataRow(-1234.5, "-£1.23K", "en-GB")]
    [DataRow(-1234.5, "-1.23K €", "fr-FR")]
    [DataRow(-1234.5, "(€ 1.23K)", "nl-NL")]
    [DataRow(-1234.5, "-1.23K¤", "twq")]
    public void Render_WithNegativeValue_ReturnsFormattedCurrency(double value, string expected, string cultureName)
    {
        // Arrange
        var culture = new CultureInfo(cultureName);

        // Act
        var result = CurrencyAbbreviator.Format((decimal)value, culture);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [DataTestMethod]
    [DataRow(2, "$-1.23K")]
    [DataRow(3, "$1.23K-")]
    [DataRow(4, "(1.23K$)")]
    [DataRow(6, "1.23K-$")]
    [DataRow(7, "1.23K$-")]
    [DataRow(9, "-$ 1.23K")]
    [DataRow(10, "1.23K $-")]
    [DataRow(11, "$ 1.23K-")]
    [DataRow(13, "(1.23K $)")]
    [DataRow(14, "($1.23K)")]
    [DataRow(15, "(1.23K$)")]
    public void Render_NegativeCurrencyPatterns(int pattern, string expected)
    {
        // Arrange
        var baseCulture = CultureInfo.GetCultureInfo("en-US");
        var testCulture = (CultureInfo)baseCulture.Clone();
        testCulture.NumberFormat.CurrencyNegativePattern = pattern;

        decimal value = -1234.5m;

        // Act
        var result = CurrencyAbbreviator.Format(value, testCulture);

        // Assert
        Assert.AreEqual(expected, result);
    }
}
