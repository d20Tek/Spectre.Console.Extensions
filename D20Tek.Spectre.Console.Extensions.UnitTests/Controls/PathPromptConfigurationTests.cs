//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class PathPromptConfigurationTests : PathPromptTestsBase
{
    [TestMethod]
    public void Constructor_WithNullLabel_ThrowsArgumentNullException()
    {
        // Arrange
        string label = null!;

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () => new PathPrompt(label));
    }

    [TestMethod]
    public void Constructor_WithEmptyLabel_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>([ExcludeFromCodeCoverage] () => new PathPrompt(string.Empty));
    }

    [TestMethod]
    public void WithBaseDirectory_WithNull_ThrowsArgumentNullException()
    {
        // Arrange
        var prompt = new PathPrompt("Path:");

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(
            [ExcludeFromCodeCoverage] () => prompt.WithBaseDirectory(null!));
    }

    [TestMethod]
    public void WithExtensions_WithNull_ThrowsArgumentNullException()
    {
        // Arrange
        var prompt = new PathPrompt("Path:");

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(
            [ExcludeFromCodeCoverage] () => prompt.WithExtensions(null!));
    }

    [TestMethod]
    public void WithDefaultValue_WithNull_ThrowsArgumentNullException()
    {
        // Arrange
        var prompt = new PathPrompt("Path:");

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(
            [ExcludeFromCodeCoverage] () => prompt.WithDefaultValue(null!));
    }

    [TestMethod]
    public void Show_WithNullConsole_ThrowsArgumentNullException()
    {
        // Arrange
        var prompt = new PathPrompt("Path:");

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () => prompt.Show(null!));
    }
}
