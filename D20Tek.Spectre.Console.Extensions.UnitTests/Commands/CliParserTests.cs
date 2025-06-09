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

        // act
        var args = CliParser.SplitCommandLine("foo bar baz").ToArray();

        // assert
        Assert.IsNotNull(args);
        CollectionAssert.AreEqual(new[] { "foo", "bar", "baz" }, args);
    }

    [TestMethod]
    public void SplitCommandLine_WithMultipleArgs_ReturnsArgsList()
    {
        // arrange

        // act
        var args = CliParser.SplitCommandLine("command --foo bar -x -y \"not\"").ToArray();

        // assert
        Assert.IsNotNull(args);
        CollectionAssert.AreEqual(new[] { "command", "--foo", "bar", "-x", "-y", "not" }, args);
    }

    [TestMethod]
    public void SplitCommandLine_WithQuotedArgumentWithSpaces_ReturnsSingleToken()
    {
        // arrange
        var input = "foo \"bar baz\"";

        // act
        var result = CliParser.SplitCommandLine(input).ToArray();

        // assert
        CollectionAssert.AreEqual(new[] { "foo", "bar baz" }, result);
    }

    [TestMethod]
    public void SplitCommandLine_WithMultipleQuotedArguments_ReturnsTokens()
    {
        // arrange
        var input = "\"foo bar\" \"baz qux\"";

        // act
        var result = CliParser.SplitCommandLine(input).ToArray();

        // assert
        CollectionAssert.AreEqual(new[] { "foo bar", "baz qux" }, result);
    }

    [TestMethod]
    public void SplitCommandLine_WithEmptyString_ReturnsEmpty()
    {
        // arrange

        // act
        var result = CliParser.SplitCommandLine("").ToArray();

        // assert.
        Assert.AreEqual(0, result.Length);
    }

    [TestMethod]
    public void SplitCommandLine_WithWhitespaceOnly_ReturnsEmpty()
    {
        // arrange
        var input = "    ";

        // act
        var result = CliParser.SplitCommandLine(input).ToArray();

        // assert
        Assert.AreEqual(0, result.Length);
    }

    [TestMethod]
    public void SplitCommandLine_WithUnclosedQuote_ReturnsTokenUpToEnd()
    {
        // arrange
        var input = "cmd \"unterminated value";

        // act
        var result = CliParser.SplitCommandLine(input).ToArray();

        // assert
        CollectionAssert.AreEqual(new[] { "cmd" }, result);
    }

    [TestMethod]
    public void SplitCommandLine_WithMultipleSpacesBetweenTokens_IgnoresExtraSpaces()
    {
        // arrange
        var input = "one   two    \"three   four\"";

        // act
        var result = CliParser.SplitCommandLine(input).ToArray();

        // assert
        CollectionAssert.AreEqual(new[] { "one", "two", "three   four" }, result);
    }

    [TestMethod]
    public void SplitCommandLine_WithMixedQuotesAndPlain_ReturnsCorrectTokens()
    {
        // arrange
        var input = "do something \"quoted arg\" again";

        // act
        var result = CliParser.SplitCommandLine(input).ToArray();

        // assert
        CollectionAssert.AreEqual(new[] { "do", "something", "quoted arg", "again" }, result);
    }

    [TestMethod]
    public void SplitCommandLine_WithQuoteInsideUnquotedToken_ReturnsSingleArg()
    {
        // arrange
        var input = "arg\"middle\"end";

        // act
        var result = CliParser.SplitCommandLine(input).ToArray();

        // assert
        CollectionAssert.AreEqual(new[] { "argmiddleend" }, result);
    }
}
