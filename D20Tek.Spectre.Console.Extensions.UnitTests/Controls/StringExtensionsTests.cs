using D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class StringExtensionsTests
{

    [TestMethod]
    public void Repeat_WithTextAndCount_ReturnsString()
    {
        // arrange
        var text = "x";

        // act
        var result = text.Repeat(5);

        // assert
        Assert.AreEqual("xxxxx", result);
    }

    [TestMethod]
    public void Repeat_WithMultipleTextAndCount_ReturnsString()
    {
        // arrange
        var text = "foo";

        // act
        var result = text.Repeat(3);

        // assert
        Assert.AreEqual("foofoofoo", result);
    }

    [TestMethod]
    public void Repeat_WithTextAndSingleCount_ReturnsString()
    {
        // arrange
        var text = "?";

        // act
        var result = text.Repeat(1);

        // assert
        Assert.AreEqual("?", result);
    }

    [TestMethod]
    public void Repeat_WithEmptyTextAndCount_ReturnsEmptyString()
    {
        // arrange
        var text = string.Empty;

        // act
        var result = text.Repeat(10);

        // assert
        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void Repeat_WithNullText_ThrowsException()
    {
        // arrange
        string text = null;

        // act - assert
        Assert.Throws<ArgumentNullException>([ExcludeFromCodeCoverage]() => text.Repeat(5));
    }
}
