//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Logging;
using D20Tek.Spectre.Console.Extensions.Settings;
using Microsoft.Extensions.Logging;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Logging;

[TestClass]
public class VerbosityLevelExtensionsTests
{
    [TestMethod]
    [DataRow(VerbosityLevel.Quiet, LogLevel.Error)]
    [DataRow(VerbosityLevel.Minimal, LogLevel.Warning)]
    [DataRow(VerbosityLevel.Normal, LogLevel.Information)]
    [DataRow(VerbosityLevel.Detailed, LogLevel.Debug)]
    [DataRow(VerbosityLevel.Diagnostic, LogLevel.Trace)]
    public void ToLogLevel_WithKnownVerbosity_ReturnsMappedLevel(
        VerbosityLevel verbosity, LogLevel expected)
    {
        // Arrange

        // Act
        var result = verbosity.ToLogLevel();

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ToLogLevel_WithUndefinedVerbosity_ReturnsInformation()
    {
        // Arrange
        var verbosity = (VerbosityLevel)999;

        // Act
        var result = verbosity.ToLogLevel();

        // Assert
        Assert.AreEqual(LogLevel.Information, result);
    }

    [TestMethod]
    [DataRow(LogLevel.Trace, VerbosityLevel.Diagnostic)]
    [DataRow(LogLevel.Debug, VerbosityLevel.Detailed)]
    [DataRow(LogLevel.Information, VerbosityLevel.Normal)]
    [DataRow(LogLevel.Warning, VerbosityLevel.Minimal)]
    [DataRow(LogLevel.Error, VerbosityLevel.Quiet)]
    [DataRow(LogLevel.Critical, VerbosityLevel.Quiet)]
    [DataRow(LogLevel.None, VerbosityLevel.Quiet)]
    public void ToVerbosityLevel_WithKnownLogLevel_ReturnsMappedVerbosity(
        LogLevel logLevel, VerbosityLevel expected)
    {
        // Arrange

        // Act
        var result = logLevel.ToVerbosityLevel();

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ToVerbosityLevel_WithUndefinedLogLevel_ReturnsNormal()
    {
        // Arrange
        var logLevel = (LogLevel)999;

        // Act
        var result = logLevel.ToVerbosityLevel();

        // Assert
        Assert.AreEqual(VerbosityLevel.Normal, result);
    }
}
