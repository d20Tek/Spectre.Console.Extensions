using D20Tek.Spectre.Console.Extensions.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Commands;

[TestClass]
public class CliParserTests
{
    [TestMethod]
    public void SplitCommandLine_WithSimpleInput_ReturnsArgsList()
    {
        // arrange
        string[] expected = ["foo", "bar", "baz"];

        // act
        var args = CliParser.SplitCommandLine("foo bar baz").ToArray();

        // assert
        Assert.IsNotNull(args);
        CollectionAssert.AreEqual(expected, args);
    }

    [TestMethod]
    public void SplitCommandLine_WithMultipleArgs_ReturnsArgsList()
    {
        // arrange
        string[] expected = ["command", "--foo", "bar", "-x", "-y", "not"];

        // act
        var args = CliParser.SplitCommandLine("command --foo bar -x -y \"not\"").ToArray();

        // assert
        Assert.IsNotNull(args);
        CollectionAssert.AreEqual(expected, args);
    }

    [TestMethod]
    public void SplitCommandLine_WithQuotedArgumentWithSpaces_ReturnsSingleToken()
    {
        // arrange
        var input = "foo \"bar baz\"";
        string[] expected = ["foo", "bar baz"];

        // act
        var result = CliParser.SplitCommandLine(input).ToArray();

        // assert
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SplitCommandLine_WithMultipleQuotedArguments_ReturnsTokens()
    {
        // arrange
        var input = "\"foo bar\" \"baz qux\"";
        string[] expected = ["foo bar", "baz qux"];

        // act
        var result = CliParser.SplitCommandLine(input).ToArray();

        // assert
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SplitCommandLine_WithEmptyString_ReturnsEmpty()
    {
        // arrange

        // act
        var result = CliParser.SplitCommandLine(string.Empty).ToArray();

        // assert.
        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void SplitCommandLine_WithWhitespaceOnly_ReturnsEmpty()
    {
        // arrange
        var input = "    ";

        // act
        var result = CliParser.SplitCommandLine(input).ToArray();

        // assert
        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void SplitCommandLine_WithUnclosedQuote_ReturnsTokenUpToEnd()
    {
        // arrange
        var input = "cmd \"unterminated value";
        string[] expected = ["cmd"];

        // act
        var result = CliParser.SplitCommandLine(input).ToArray();

        // assert
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SplitCommandLine_WithMultipleSpacesBetweenTokens_IgnoresExtraSpaces()
    {
        // arrange
        var input = "one   two    \"three   four\"";
        string[] expected = ["one", "two", "three   four"];

        // act
        var result = CliParser.SplitCommandLine(input).ToArray();

        // assert
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SplitCommandLine_WithMixedQuotesAndPlain_ReturnsCorrectTokens()
    {
        // arrange
        var input = "do something \"quoted arg\" again";
        string[] expected = ["do", "something", "quoted arg", "again"];

        // act
        var result = CliParser.SplitCommandLine(input).ToArray();

        // assert
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SplitCommandLine_WithQuoteInsideUnquotedToken_ReturnsSingleArg()
    {
        // arrange
        var input = "arg\"middle\"end";
        string[] expected = ["argmiddleend"];

        // act
        var result = CliParser.SplitCommandLine(input).ToArray();

        // assert
        CollectionAssert.AreEqual(expected, result);
    }
}
