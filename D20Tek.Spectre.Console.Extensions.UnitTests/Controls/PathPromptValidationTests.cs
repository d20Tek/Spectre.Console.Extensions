//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Controls;
using D20Tek.Spectre.Console.Extensions.Testing;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class PathPromptValidationTests : PathPromptTestsBase
{
    [TestMethod]
    public void Show_WithMissingPathThenValidPath_DisplaysErrorThenReturnsValue()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("doesnotexist.txt");
        console.TestInput.PushTextWithEnter("notes.txt");
        var prompt = new PathPrompt("Path:").WithBaseDirectory(_tempDirectory);

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual("notes.txt", result);
        Assert.Contains("does not exist", console.Output);
    }

    [TestMethod]
    public void Show_WithPathKindFileAndDirectoryInput_DisplaysErrorThenReturnsValue()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("subfolder");
        console.TestInput.PushTextWithEnter("notes.txt");
        var prompt = new PathPrompt("Path:").WithBaseDirectory(_tempDirectory).WithPathKind(PathKind.File);

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual("notes.txt", result);
        Assert.Contains("File 'subfolder' does not exist.", console.Output);
    }

    [TestMethod]
    public void Show_WithPathKindDirectoryAndFileInput_DisplaysErrorThenReturnsValue()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("notes.txt");
        console.TestInput.PushTextWithEnter("subfolder");
        var prompt = new PathPrompt("Path:").WithBaseDirectory(_tempDirectory).WithPathKind(PathKind.Directory);

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual("subfolder", result);
        Assert.Contains("Directory 'notes.txt' does not exist.", console.Output);
    }

    [TestMethod]
    public void Show_WithMustExistFalse_AllowsMissingPath()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("newfile.txt");
        var prompt = new PathPrompt("Path:").WithBaseDirectory(_tempDirectory).MustExist(false);

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual("newfile.txt", result);
    }

    [TestMethod]
    public void Show_WithMultipleConsecutiveInvalidInputs_DisplaysEachErrorThenReturnsValue()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("missing1.txt");
        console.TestInput.PushTextWithEnter("missing2.txt");
        console.TestInput.PushTextWithEnter("notes.txt");
        var prompt = new PathPrompt("Path:").WithBaseDirectory(_tempDirectory);

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual("notes.txt", result);
        Assert.Contains("Path 'missing1.txt' does not exist.", console.Output);
        Assert.Contains("Path 'missing2.txt' does not exist.", console.Output);
    }

    [TestMethod]
    public void Show_WithMatchingExtension_ReturnsPath()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("data.json");
        var prompt = new PathPrompt("Path:").WithBaseDirectory(_tempDirectory).WithExtensions(".json");

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual("data.json", result);
    }

    [TestMethod]
    public void Show_WithNonMatchingExtensionThenValid_DisplaysErrorThenReturnsValue()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("notes.txt");
        console.TestInput.PushTextWithEnter("data.json");
        var prompt = new PathPrompt("Path:").WithBaseDirectory(_tempDirectory).WithExtensions(".json");

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual("data.json", result);
        Assert.Contains("must have one of the following extensions", console.Output);
    }

    [TestMethod]
    public void Show_WithCustomErrorMessage_DisplaysCustomMessage()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("doesnotexist.txt");
        console.TestInput.PushTextWithEnter("notes.txt");
        var prompt = new PathPrompt("Path:")
            .WithBaseDirectory(_tempDirectory)
            .WithErrorMessage("Custom path error.");

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual("notes.txt", result);
        Assert.Contains("Custom path error.", console.Output);
    }
}
