//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Controls;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class PathPromptDefaultValueTests : PathPromptTestsBase
{
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
