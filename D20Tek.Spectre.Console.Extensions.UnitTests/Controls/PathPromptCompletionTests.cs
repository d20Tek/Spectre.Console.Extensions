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
}
