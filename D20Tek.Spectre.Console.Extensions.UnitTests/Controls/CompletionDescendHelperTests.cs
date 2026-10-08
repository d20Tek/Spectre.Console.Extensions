//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;
using Spectre.Console;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
[ExcludeFromCodeCoverage]
public class CompletionDescendHelperTests
{
    [TestMethod]
    public void TryDescendIntoDirectory_WithNoCompletionProvider_ReturnsStateUnchanged()
    {
        // Arrange
        var request = new ReadLineRequest(null!, Style.Plain, false, null, [], [], null);
        var buffer = new StringBuilder("subfolder" + Path.DirectorySeparatorChar);
        var state = new InputState(request, buffer, buffer.Length, true, ["original"], -1, null, false, false);

        // Act
        var result = CompletionDescendHelper.TryDescendIntoDirectory(state);

        // Assert
        Assert.AreEqual(state, result);
        CollectionAssert.AreEqual(new List<string> { "original" }, result.CompletionItems);
    }

    [TestMethod]
    public void TryDescendIntoDirectory_WithEmptyBuffer_ReturnsStateUnchanged()
    {
        // Arrange
        var request = new ReadLineRequest(null!, Style.Plain, false, null, [], [], _ => ["should-not-be-used"]);
        var buffer = new StringBuilder();
        var state = new InputState(request, buffer, 0, true, ["original"], -1, null, false, false);

        // Act
        var result = CompletionDescendHelper.TryDescendIntoDirectory(state);

        // Assert
        Assert.AreEqual(state, result);
        CollectionAssert.AreEqual(new List<string> { "original" }, result.CompletionItems);
    }

    [TestMethod]
    public void TryDescendIntoDirectory_WithBufferNotEndingInSeparator_ReturnsStateUnchanged()
    {
        // Arrange
        var request = new ReadLineRequest(null!, Style.Plain, false, null, [], [], _ => ["should-not-be-used"]);
        var buffer = new StringBuilder("notes.txt");
        var state = new InputState(request, buffer, buffer.Length, true, ["original"], -1, null, false, false);

        // Act
        var result = CompletionDescendHelper.TryDescendIntoDirectory(state);

        // Assert
        Assert.AreEqual(state, result);
        CollectionAssert.AreEqual(new List<string> { "original" }, result.CompletionItems);
    }

    [TestMethod]
    public void TryDescendIntoDirectory_WithBufferEndingInDirectorySeparator_ReturnsUpdatedCompletionItems()
    {
        // Arrange
        var typedDirectory = "subfolder" + Path.DirectorySeparatorChar;
        var request = new ReadLineRequest(
            null!,
            Style.Plain,
            false,
            null,
            [],
            [],
            typedText => typedText == typedDirectory ? ["nested.txt"] : ["should-not-be-used"]);
        var buffer = new StringBuilder(typedDirectory);
        var state = new InputState(request, buffer, buffer.Length, true, ["original"], -1, null, false, false);

        // Act
        var result = CompletionDescendHelper.TryDescendIntoDirectory(state);

        // Assert
        CollectionAssert.AreEqual(new List<string> { "nested.txt" }, result.CompletionItems);
        Assert.AreEqual(state.Buffer, result.Buffer);
        Assert.AreEqual(state.CursorIndex, result.CursorIndex);
    }

    [TestMethod]
    public void TryDescendIntoDirectory_WithBufferEndingInAltDirectorySeparator_ReturnsUpdatedCompletionItems()
    {
        // Arrange
        var typedDirectory = "subfolder" + Path.AltDirectorySeparatorChar;
        var request = new ReadLineRequest(
            null!,
            Style.Plain,
            false,
            null,
            [],
            [],
            typedText => typedText == typedDirectory ? ["nested.txt"] : ["should-not-be-used"]);
        var buffer = new StringBuilder(typedDirectory);
        var state = new InputState(request, buffer, buffer.Length, true, ["original"], -1, null, false, false);

        // Act
        var result = CompletionDescendHelper.TryDescendIntoDirectory(state);

        // Assert
        CollectionAssert.AreEqual(new List<string> { "nested.txt" }, result.CompletionItems);
    }
}
