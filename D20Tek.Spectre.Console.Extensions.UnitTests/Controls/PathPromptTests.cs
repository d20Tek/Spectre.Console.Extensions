//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Controls;
using D20Tek.Spectre.Console.Extensions.Testing;
using Spectre.Console;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class PathPromptTests
{
    private string _tempDirectory = string.Empty;

    [TestInitialize]
    public void TestInitialize()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), $"pathprompttests{Guid.NewGuid():N}");
        Directory.CreateDirectory(_tempDirectory);
        Directory.CreateDirectory(Path.Combine(_tempDirectory, "subfolder"));
        Directory.CreateDirectory(Path.Combine(_tempDirectory, ".hiddenfolder"));
        File.WriteAllText(Path.Combine(_tempDirectory, "notes.txt"), "sample");
        File.WriteAllText(Path.Combine(_tempDirectory, "data.json"), "{}");
    }

    [TestCleanup]
    public void TestCleanup()
    {
        if (Directory.Exists(_tempDirectory))
        {
            Directory.Delete(_tempDirectory, recursive: true);
        }
    }

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
    public void Show_WithEmptyInputAndNoDefault_AsksAgain()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushKey(ConsoleKey.Enter);
        console.TestInput.PushTextWithEnter("notes.txt");
        var prompt = new PathPrompt("Path:").WithBaseDirectory(_tempDirectory);

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual("notes.txt", result);
        Assert.Contains("Please enter a path.", console.Output);
    }

    [TestMethod]
    public void Show_WithEmptyInputAndDefaultValue_ReturnsDefault()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushKey(ConsoleKey.Enter);
        var prompt = new PathPrompt("Path:")
            .WithBaseDirectory(_tempDirectory)
            .WithDefaultValue("notes.txt");

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual("notes.txt", result);
        Assert.Contains("(notes.txt)", console.Output);
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
    public void Show_WithTabCompletion_ReturnsClosestMatch()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushText("not");
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.Enter);
        var prompt = new PathPrompt("Path:").WithBaseDirectory(_tempDirectory);

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual("notes.txt", result);
    }

    [TestMethod]
    public void Show_WithTabCompletionIncludeHidden_IncludesHiddenEntries()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushText(".");
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.Enter);
        var prompt = new PathPrompt("Path:").WithBaseDirectory(_tempDirectory).IncludeHidden();

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual(".hiddenfolder" + Path.DirectorySeparatorChar, result);
    }

    [TestMethod]
    public void Show_WithTabCompletionExcludeHidden_SkipsHiddenEntries()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.Enter);
        var prompt = new PathPrompt("Path:").WithBaseDirectory(_tempDirectory).MustExist(false);

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreNotEqual(".hiddenfolder" + Path.DirectorySeparatorChar, result);
    }

    [TestMethod]
    public void Show_WithTabCompletionAndExtensionsFilter_OnlyCompletesMatchingFiles()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushText("d");
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.Enter);
        var prompt = new PathPrompt("Path:").WithBaseDirectory(_tempDirectory).WithExtensions(".json");

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual("data.json", result);
    }

    [TestMethod]
    public void Show_WithEmptyBaseDirectory_ReturnsNoCompletionsAndStillPrompts()
    {
        // Arrange
        var emptyDirectory = Path.Combine(_tempDirectory, "emptyfolder");
        Directory.CreateDirectory(emptyDirectory);
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("anything.txt");
        var prompt = new PathPrompt("Path:").WithBaseDirectory(emptyDirectory).MustExist(false);

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual("anything.txt", result);
    }

    [TestMethod]
    public void Show_WithMissingBaseDirectory_ReturnsNoCompletionsAndStillPrompts()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushTextWithEnter("anything.txt");
        var missingDirectory = Path.Combine(_tempDirectory, "missingbase");
        var prompt = new PathPrompt("Path:").WithBaseDirectory(missingDirectory).MustExist(false);

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual("anything.txt", result);
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

    [TestMethod]
    public void Show_WithAnyPathKindAndDefaultValue_ValidatesDefaultWithoutExistenceCheck()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushKey(ConsoleKey.Enter);
        var prompt = new PathPrompt("Path:")
            .WithBaseDirectory(_tempDirectory)
            .WithDefaultValue("doesnotexist.txt");

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual("doesnotexist.txt", result);
    }
}
