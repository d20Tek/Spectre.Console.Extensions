//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing;

[TestClass]
public class CommandAppResultAssertionExtensionsTests
{
    [TestMethod]
    public void Should_WithValidResult_ReturnsAssertions()
    {
        // Arrange
        var result = new CommandAppBasicResult(0, "done");

        // Act
        var assertions = result.Should();

        // Assert
        Assert.IsNotNull(assertions);
        Assert.AreSame(result, assertions.Result);
    }

    [TestMethod]
    public void Should_WithNullResult_ThrowsArgumentNullException()
    {
        // Arrange
        CommandAppBasicResult result = null!;

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () => result.Should());
    }

    [TestMethod]
    public void ShouldSucceed_WithZeroExitCode_ReturnsAssertions()
    {
        // Arrange
        var result = new CommandAppBasicResult(0, "done");

        // Act
        var assertions = result.ShouldSucceed();

        // Assert
        Assert.IsNotNull(assertions);
    }

    [TestMethod]
    public void ShouldSucceed_WithNonZeroExitCode_ThrowsCommandAppAssertionException()
    {
        // Arrange
        var result = new CommandAppBasicResult(1, "error");

        // Act & Assert
        Assert.ThrowsExactly<CommandAppAssertionException>(
            [ExcludeFromCodeCoverage] () => result.ShouldSucceed());
    }

    [TestMethod]
    public void ShouldFail_WithNonZeroExitCode_ReturnsAssertions()
    {
        // Arrange
        var result = new CommandAppBasicResult(2, "error");

        // Act
        var assertions = result.ShouldFail();

        // Assert
        Assert.IsNotNull(assertions);
    }

    [TestMethod]
    public void ShouldFail_WithZeroExitCode_ThrowsCommandAppAssertionException()
    {
        // Arrange
        var result = new CommandAppBasicResult(0, "done");

        // Act & Assert
        Assert.ThrowsExactly<CommandAppAssertionException>(
            [ExcludeFromCodeCoverage] () => result.ShouldFail());
    }

    [TestMethod]
    public void ShouldReturnExitCode_WithMatchingExitCode_ReturnsAssertions()
    {
        // Arrange
        var result = new CommandAppBasicResult(42, "done");

        // Act
        var assertions = result.ShouldReturnExitCode(42);

        // Assert
        Assert.IsNotNull(assertions);
    }

    [TestMethod]
    public void ShouldReturnExitCode_WithNonMatchingExitCode_ThrowsCommandAppAssertionException()
    {
        // Arrange
        var result = new CommandAppBasicResult(1, "done");

        // Act & Assert
        Assert.ThrowsExactly<CommandAppAssertionException>(
            [ExcludeFromCodeCoverage] () => result.ShouldReturnExitCode(2));
    }

    [TestMethod]
    public void ShouldSucceed_ThenChainOutputContains_ChainsFluently()
    {
        // Arrange
        var result = new CommandAppBasicResult(0, "operation done");

        // Act
        var assertions = result.ShouldSucceed().AndOutputContains("done");

        // Assert
        Assert.IsNotNull(assertions);
    }
}
