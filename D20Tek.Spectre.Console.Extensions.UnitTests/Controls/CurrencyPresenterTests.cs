using D20Tek.Spectre.Console.Extensions.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class CurrencyPresenterTests
{
    [TestMethod]
    public void Render_WithPositiveValue_ReturnsValueAsString()
    {
        // Arrange
        decimal value = 1234.5m;

        // Act
        var result = value.Render();

        // Assert
        Assert.AreEqual("$1,234.50", result);
    }

    [TestMethod]
    public void Render_WithNegativeValue_ReturnsValueAsString()
    {
        // Arrange
        decimal value = -1234.5m;

        // Act
        var result = value.Render();

        // Assert
        Assert.AreEqual("($1,234.50)", result);
    }

    [TestMethod]
    public void Render_WithPositiveValueAndStyle_ReturnsValueAsString()
    {
        // Arrange
        decimal value = 1234.5m;

        // Act
        var result = value.Render("green", "red");

        // Assert
        Assert.AreEqual("[green]$1,234.50[/]", result);
    }

    [TestMethod]
    public void Render_WithNegativeValueAndStyle_ReturnsValueAsString()
    {
        // Arrange
        decimal value = -1234.5m;

        // Act
        var result = value.Render("green", "red");

        // Assert
        Assert.AreEqual("[red]($1,234.50)[/]", result);
    }

    [TestMethod]
    public void RenderAbbreviated_WithPositiveValue_ReturnsValueAsString()
    {
        // Arrange
        var culture = new CultureInfo("en-US");
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        decimal value = 1234.5m;

        // Act
        var result = value.RenderAbbreviated();

        // Assert
        Assert.AreEqual("$1.23K", result);
    }

    [TestMethod]
    public void RenderAbbreviated_WithNegativeValue_ReturnsValueAsString()
    {
        // Arrange
        var culture = new CultureInfo("en-US");
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        decimal value = -1234.5m;

        // Act
        var result = value.RenderAbbreviated();

        // Assert
        Assert.AreEqual("($1.23K)", result);
    }

    [TestMethod]
    public void RenderAbbreviated_WithPositiveValueAndStyle_ReturnsValueAsString()
    {
        // Arrange
        var culture = new CultureInfo("en-US");
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        decimal value = 1234.5m;

        // Act
        var result = value.RenderAbbreviated("green", "red");

        // Assert
        Assert.AreEqual("[green]$1.23K[/]", result);
    }

    [TestMethod]
    public void RenderAbbreviated_WithNegativeValueAndStyle_ReturnsValueAsString()
    {
        // Arrange
        var culture = new CultureInfo("en-US");
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        decimal value = -1234.5m;

        // Act
        var result = value.RenderAbbreviated("green", "red");

        // Assert
        Assert.AreEqual("[red]($1.23K)[/]", result);
    }
}
