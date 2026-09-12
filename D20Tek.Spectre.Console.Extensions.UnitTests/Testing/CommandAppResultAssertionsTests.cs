//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing;

[TestClass]
public class CommandAppResultAssertionsTests
{
    [TestMethod]
    public void Constructor_WithValidResult_SetsResult()
    {
        // Arrange
        var result = new CommandAppBasicResult(0, "done");

        // Act
        var assertions = new CommandAppResultAssertions(result);

        // Assert
        Assert.AreSame(result, assertions.Result);
    }

    [TestMethod]
    public void Constructor_WithNullResult_ThrowsArgumentNullException()
    {
        // Arrange
        CommandAppBasicResult result = null!;

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(
            [ExcludeFromCodeCoverage] () => new CommandAppResultAssertions(result));
    }

    [TestMethod]
    public void AndSucceed_WithZeroExitCode_ReturnsSameInstance()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(0, "done"));

        // Act
        var actual = assertions.AndSucceed();

        // Assert
        Assert.AreSame(assertions, actual);
    }

    [TestMethod]
    public void AndSucceed_WithNonZeroExitCode_ThrowsCommandAppAssertionException()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(1, "error"));

        // Act & Assert
        Assert.ThrowsExactly<CommandAppAssertionException>(
            [ExcludeFromCodeCoverage] () => assertions.AndSucceed());
    }

    [TestMethod]
    public void AndFail_WithNonZeroExitCode_ReturnsSameInstance()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(3, "error"));

        // Act
        var actual = assertions.AndFail();

        // Assert
        Assert.AreSame(assertions, actual);
    }

    [TestMethod]
    public void AndFail_WithZeroExitCode_ThrowsCommandAppAssertionException()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(0, "done"));

        // Act & Assert
        Assert.ThrowsExactly<CommandAppAssertionException>(
            [ExcludeFromCodeCoverage] () => assertions.AndFail());
    }

    [TestMethod]
    public void AndReturnExitCode_WithMatchingExitCode_ReturnsSameInstance()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(5, "done"));

        // Act
        var actual = assertions.AndReturnExitCode(5);

        // Assert
        Assert.AreSame(assertions, actual);
    }

    [TestMethod]
    public void AndReturnExitCode_WithNonMatchingExitCode_ThrowsCommandAppAssertionException()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(5, "done"));

        // Act & Assert
        Assert.ThrowsExactly<CommandAppAssertionException>(
            [ExcludeFromCodeCoverage] () => assertions.AndReturnExitCode(6));
    }

    [TestMethod]
    public void AndOutputContains_WhenPresent_ReturnsSameInstance()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(0, "hello world"));

        // Act
        var actual = assertions.AndOutputContains("world");

        // Assert
        Assert.AreSame(assertions, actual);
    }

    [TestMethod]
    public void AndOutputContains_WithComparison_MatchesIgnoringCase()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(0, "Hello World"));

        // Act
        var actual = assertions.AndOutputContains("world", StringComparison.OrdinalIgnoreCase);

        // Assert
        Assert.AreSame(assertions, actual);
    }

    [TestMethod]
    public void AndOutputContains_WhenMissing_ThrowsCommandAppAssertionException()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(0, "hello"));

        // Act & Assert
        Assert.ThrowsExactly<CommandAppAssertionException>(
            [ExcludeFromCodeCoverage] () => assertions.AndOutputContains("world"));
    }

    [TestMethod]
    public void AndOutputContains_WithNullExpected_ThrowsArgumentNullException()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(0, "hello"));

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(
            [ExcludeFromCodeCoverage] () => assertions.AndOutputContains(null!));
    }

    [TestMethod]
    public void AndOutputDoesNotContain_WhenAbsent_ReturnsSameInstance()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(0, "hello"));

        // Act
        var actual = assertions.AndOutputDoesNotContain("world");

        // Assert
        Assert.AreSame(assertions, actual);
    }

    [TestMethod]
    public void AndOutputDoesNotContain_WhenPresent_ThrowsCommandAppAssertionException()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(0, "hello world"));

        // Act & Assert
        Assert.ThrowsExactly<CommandAppAssertionException>(
            [ExcludeFromCodeCoverage] () => assertions.AndOutputDoesNotContain("world"));
    }

    [TestMethod]
    public void AndOutputDoesNotContain_WithNullUnexpected_ThrowsArgumentNullException()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(0, "hello"));

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(
            [ExcludeFromCodeCoverage] () => assertions.AndOutputDoesNotContain(null!));
    }

    [TestMethod]
    public void AndOutputMatches_WhenMatching_ReturnsSameInstance()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(0, "item count: 42"));

        // Act
        var actual = assertions.AndOutputMatches(@"count: \d+");

        // Assert
        Assert.AreSame(assertions, actual);
    }

    [TestMethod]
    public void AndOutputMatches_WhenNotMatching_ThrowsCommandAppAssertionException()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(0, "no numbers here"));

        // Act & Assert
        Assert.ThrowsExactly<CommandAppAssertionException>(
            [ExcludeFromCodeCoverage] () => assertions.AndOutputMatches(@"\d+"));
    }

    [TestMethod]
    public void AndOutputMatches_WithNullPattern_ThrowsArgumentNullException()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(0, "hello"));

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(
            [ExcludeFromCodeCoverage] () => assertions.AndOutputMatches(null!));
    }

    [TestMethod]
    public void AndOutputIsEmpty_WhenEmpty_ReturnsSameInstance()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(0, string.Empty));

        // Act
        var actual = assertions.AndOutputIsEmpty();

        // Assert
        Assert.AreSame(assertions, actual);
    }

    [TestMethod]
    public void AndOutputIsEmpty_WhenNotEmpty_ThrowsCommandAppAssertionException()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(0, "content"));

        // Act & Assert
        Assert.ThrowsExactly<CommandAppAssertionException>(
            [ExcludeFromCodeCoverage] () => assertions.AndOutputIsEmpty());
    }

    [TestMethod]
    public void AndOutputIsNotEmpty_WhenNotEmpty_ReturnsSameInstance()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(0, "content"));

        // Act
        var actual = assertions.AndOutputIsNotEmpty();

        // Assert
        Assert.AreSame(assertions, actual);
    }

    [TestMethod]
    public void AndOutputIsNotEmpty_WhenEmpty_ThrowsCommandAppAssertionException()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(0, string.Empty));

        // Act & Assert
        Assert.ThrowsExactly<CommandAppAssertionException>(
            [ExcludeFromCodeCoverage] () => assertions.AndOutputIsNotEmpty());
    }

    [TestMethod]
    public void ShouldSucceed_WithZeroExitCode_ReturnsSameInstance()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(0, "done"));

        // Act
        var actual = assertions.ShouldSucceed();

        // Assert
        Assert.AreSame(assertions, actual);
    }

    [TestMethod]
    public void ShouldFail_WithNonZeroExitCode_ReturnsSameInstance()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(1, "error"));

        // Act
        var actual = assertions.ShouldFail();

        // Assert
        Assert.AreSame(assertions, actual);
    }

    [TestMethod]
    public void ShouldReturnExitCode_WithMatchingExitCode_ReturnsSameInstance()
    {
        // Arrange
        var assertions = new CommandAppResultAssertions(new CommandAppBasicResult(7, "done"));

        // Act
        var actual = assertions.ShouldReturnExitCode(7);

        // Assert
        Assert.AreSame(assertions, actual);
    }
}
