//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Controls;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class PathValidatorTests
{
    private string _tempDirectory = string.Empty;

    [TestInitialize]
    public void TestInitialize()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), $"pathvalidatortests{Guid.NewGuid():N}");
        Directory.CreateDirectory(_tempDirectory);
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
    public void Validate_WithEmptyInputAndNoDefaultValue_ReturnsError()
    {
        // Arrange
        var validator = new PathValidator(
            mustExist: true,
            pathKind: PathKind.Any,
            baseDirectory: _tempDirectory,
            extensions: null,
            errorMessage: null,
            defaultValue: null);

        // Act
        var result = validator.Validate(string.Empty);

        // Assert
        Assert.IsFalse(result.Successful);
        Assert.AreEqual("[red]Please enter a path.[/]", result.Message);
    }

    [TestMethod]
    public void Validate_WithEmptyInputNoDefaultValueAndCustomErrorMessage_ReturnsCustomError()
    {
        // Arrange
        var validator = new PathValidator(
            mustExist: true,
            pathKind: PathKind.Any,
            baseDirectory: _tempDirectory,
            extensions: null,
            errorMessage: "Custom required message.",
            defaultValue: null);

        // Act
        var result = validator.Validate("   ");

        // Assert
        Assert.IsFalse(result.Successful);
        Assert.AreEqual("Custom required message.", result.Message);
    }

    [TestMethod]
    public void Validate_WithEmptyInputAndDefaultValue_ReturnsSuccess()
    {
        // Arrange
        var validator = new PathValidator(
            mustExist: true,
            pathKind: PathKind.Any,
            baseDirectory: _tempDirectory,
            extensions: null,
            errorMessage: null,
            defaultValue: "notes.txt");

        // Act
        var result = validator.Validate(string.Empty);

        // Assert
        Assert.IsTrue(result.Successful);
    }
}
