//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Controls;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class PathPromptCompletionTests : PathPromptTestsBase
{
    [TestMethod]
    public void Show_WithRepeatedTab_CyclesThroughSiblingEntries()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.Enter);
        var prompt = new PathPrompt("Path:").WithBaseDirectory(_tempDirectory);

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual("subfolder" + Path.DirectorySeparatorChar, result);
    }

    [TestMethod]
    public void Show_WithRightArrowAtEndOfDirectory_DescendsAndCyclesNestedEntries()
    {
        // Arrange
        var nestedFile = Path.Combine(_tempDirectory, "subfolder", "nested.txt");
        File.WriteAllText(nestedFile, "sample");

        var console = new TestConsole();
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.RightArrow);
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.Enter);
        var prompt = new PathPrompt("Path:").WithBaseDirectory(_tempDirectory);

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual(
            "subfolder" + Path.DirectorySeparatorChar + "nested.txt",
            result);
    }

    [TestMethod]
    public void Show_WithDownArrowAtEndOfDirectory_DescendsAndCyclesNestedEntries()
    {
        // Arrange
        var nestedFile = Path.Combine(_tempDirectory, "subfolder", "nested.txt");
        File.WriteAllText(nestedFile, "sample");

        var console = new TestConsole();
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.DownArrow);
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.Enter);
        var prompt = new PathPrompt("Path:").WithBaseDirectory(_tempDirectory);

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual(
            "subfolder" + Path.DirectorySeparatorChar + "nested.txt",
            result);
    }

    [TestMethod]
    public void Show_WithRootedDirectoryPathCompletion_DoesNotDuplicateSeparator()
    {
        // Arrange
        var root = Path.GetPathRoot(_tempDirectory)!;
        var firstRootEntry = Directory.EnumerateFileSystemEntries(root).First();
        var firstRootEntryName = Path.GetFileName(firstRootEntry);
        var typedPrefix = root + firstRootEntryName[..1];

        var console = new TestConsole();
        console.TestInput.PushText(typedPrefix);
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.Enter);
        var prompt = new PathPrompt("Path:").WithBaseDirectory(Directory.GetCurrentDirectory());

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.IsFalse(
            result.Contains(root[..^1] + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar),
            $"Completed path '{result}' should not contain a duplicated path separator after the root.");
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
    public void Show_WithAbsolutePathCompletion_CompletesWithinTypedParentDirectory()
    {
        // Arrange
        var console = new TestConsole();
        var typedPrefix = Path.Combine(_tempDirectory, "sub");
        console.TestInput.PushText(typedPrefix);
        console.TestInput.PushKey(ConsoleKey.Tab);
        console.TestInput.PushKey(ConsoleKey.Enter);
        var prompt = new PathPrompt("Path:").WithBaseDirectory(Directory.GetCurrentDirectory());

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual(Path.Combine(_tempDirectory, "subfolder") + Path.DirectorySeparatorChar, result);
    }

    [TestMethod]
    public void Show_WithBackspace_RemovesCharacterFromPathInput()
    {
        // Arrange
        var console = new TestConsole();
        console.TestInput.PushText("notes.txtxx");
        console.TestInput.PushKey(ConsoleKey.Backspace);
        console.TestInput.PushKey(ConsoleKey.Backspace);
        console.TestInput.PushKey(ConsoleKey.Enter);
        var prompt = new PathPrompt("Path:").WithBaseDirectory(_tempDirectory);

        // Act
        var result = prompt.Show(console);

        // Assert
        Assert.AreEqual("notes.txt", result);
    }
}
