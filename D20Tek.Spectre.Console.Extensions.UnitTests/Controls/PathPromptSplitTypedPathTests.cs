//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Controls;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class PathPromptSplitTypedPathTests
{
    [TestMethod]
    public void SplitTypedPath_WithEmptyText_ReturnsBaseDirectoryAndEmptyParts()
    {
        // Arrange
        var baseDirectory = Path.GetTempPath();
        var prompt = new PathPrompt("Path:").WithBaseDirectory(baseDirectory);

        // Act
        var result = prompt.SplitTypedPath(string.Empty);

        // Assert
        Assert.AreEqual(baseDirectory, result.ResolvedDirectory);
        Assert.AreEqual(string.Empty, result.TypedDirectory);
        Assert.AreEqual(string.Empty, result.Prefix);
    }

    [TestMethod]
    public void SplitTypedPath_WithNoDirectoryAndNotRooted_ReturnsBaseDirectoryAsResolved()
    {
        // Arrange
        var baseDirectory = Path.GetTempPath();
        var prompt = new PathPrompt("Path:").WithBaseDirectory(baseDirectory);

        // Act
        var result = prompt.SplitTypedPath("notes");

        // Assert
        Assert.AreEqual(baseDirectory, result.ResolvedDirectory);
        Assert.AreEqual(string.Empty, result.TypedDirectory);
        Assert.AreEqual("notes", result.Prefix);
    }

    [TestMethod]
    public void SplitTypedPath_WithNoDirectoryAndRooted_ReturnsPathRootAsResolved()
    {
        // Arrange
        var baseDirectory = Path.GetTempPath();
        var prompt = new PathPrompt("Path:").WithBaseDirectory(baseDirectory);
        var root = Path.GetPathRoot(baseDirectory)!;
        var driveOnly = root.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        // "C:" (drive letter with no separator) is rooted, but Path.GetDirectoryName returns null
        // for it, so SplitTypedPath coalesces it to an empty directory part.
        Assert.IsNull(Path.GetDirectoryName(driveOnly));
        Assert.IsTrue(Path.IsPathRooted(driveOnly));

        // Act
        var result = prompt.SplitTypedPath(driveOnly);

        // Assert
        Assert.AreEqual(Path.GetPathRoot(driveOnly), result.ResolvedDirectory);
        Assert.AreEqual(string.Empty, result.TypedDirectory);
        Assert.AreEqual(string.Empty, result.Prefix);
    }

    [TestMethod]
    public void SplitTypedPath_WithRootedDirectoryEndingInSeparator_DoesNotDuplicateSeparator()
    {
        // Arrange
        var baseDirectory = Path.GetTempPath();
        var prompt = new PathPrompt("Path:").WithBaseDirectory(baseDirectory);
        var root = Path.GetPathRoot(baseDirectory)!;
        var typedText = root + "dev";

        // Act
        var result = prompt.SplitTypedPath(typedText);

        // Assert
        Assert.AreEqual(root, result.ResolvedDirectory);
        Assert.AreEqual(root, result.TypedDirectory);
        Assert.AreEqual("dev", result.Prefix);
    }

    [TestMethod]
    public void SplitTypedPath_WithRootedDirectoryPart_ResolvesDirectoryAsTyped()
    {
        // Arrange
        var baseDirectory = Path.GetTempPath();
        var prompt = new PathPrompt("Path:").WithBaseDirectory(baseDirectory);
        var root = Path.GetPathRoot(baseDirectory)!;
        var typedText = Path.Combine(root, "foldername", "sub");

        // Act
        var result = prompt.SplitTypedPath(typedText);

        // Assert
        Assert.AreEqual(Path.Combine(root, "foldername"), result.ResolvedDirectory);
        Assert.AreEqual("sub", result.Prefix);
    }

    [TestMethod]
    public void SplitTypedPath_WithRelativeDirectoryPart_CombinesWithBaseDirectory()
    {
        // Arrange
        var baseDirectory = Path.GetTempPath();
        var prompt = new PathPrompt("Path:").WithBaseDirectory(baseDirectory);
        var typedText = Path.Combine("foldername", "sub");

        // Act
        var result = prompt.SplitTypedPath(typedText);

        // Assert
        Assert.AreEqual(Path.Combine(baseDirectory, "foldername"), result.ResolvedDirectory);
        Assert.AreEqual("foldername" + Path.DirectorySeparatorChar, result.TypedDirectory);
        Assert.AreEqual("sub", result.Prefix);
    }

    [TestMethod]
    public void SplitTypedPath_WithDirectoryNotEndingInSeparator_AppendsSeparator()
    {
        // Arrange
        var baseDirectory = Path.GetTempPath();
        var prompt = new PathPrompt("Path:").WithBaseDirectory(baseDirectory);
        var typedText = Path.Combine("foldername", "sub");

        // Act
        var result = prompt.SplitTypedPath(typedText);

        // Assert
        Assert.IsTrue(result.TypedDirectory.EndsWith(Path.DirectorySeparatorChar));
        Assert.AreEqual("foldername" + Path.DirectorySeparatorChar, result.TypedDirectory);
    }
}
