//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Controls;
using D20Tek.Spectre.Console.Extensions.Testing;
using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class PathPromptMiscTests : PathPromptTestsBase
{
    [TestMethod]
    public void Show_WithExistingFile_ReturnsPath()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("notes.txt");
        var prompt = new PathPrompt("Path:").WithBaseDirectory(_tempDirectory);

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual("notes.txt", result);
        Assert.Contains("Path:", console.Output);
    }

    [TestMethod]
    public void Show_WithExistingDirectory_ReturnsPath()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("subfolder");
        var prompt = new PathPrompt("Path:").WithBaseDirectory(_tempDirectory);

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual("subfolder", result);
    }

    [TestMethod]
    public void Show_WithRootedPath_ReturnsPath()
    {
        // Arrange
        var filePath = Path.Combine(_tempDirectory, "notes.txt");
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter(filePath);
        var prompt = new PathPrompt("Path:").WithBaseDirectory(_tempDirectory);

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual(filePath, result);
    }

    [TestMethod]
    public void Show_WithPromptStyle_ReturnsPath()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("notes.txt");
        var prompt = new PathPrompt("Path:")
            .WithBaseDirectory(_tempDirectory)
            .WithPromptStyle(new Style(decoration: Decoration.Italic));

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual("notes.txt", result);
    }

    [TestMethod]
    public async Task ShowAsync_WithExistingFile_ReturnsPath()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("notes.txt");
        var prompt = new PathPrompt("Path:").WithBaseDirectory(_tempDirectory);

        // Act
        var result = await prompt.ShowAsync(console, CancellationToken.None);

        // Assert
        Assert.AreEqual("notes.txt", result);
    }
}
