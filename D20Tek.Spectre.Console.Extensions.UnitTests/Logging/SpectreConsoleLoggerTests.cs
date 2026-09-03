//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Logging;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Logging;

[TestClass]
public class SpectreConsoleLoggerTests
{
    private static SpectreConsoleLogger CreateLogger(
        TestConsole console,
        LogLevel minLevel = LogLevel.Information,
        SpectreConsoleLoggerOptions? options = null) =>
        new(console, "TestCategory", options ?? new SpectreConsoleLoggerOptions(), () => minLevel);

    [TestMethod]
    public void Constructor_WithNullConsole_ThrowsException()
    {
        // Arrange
        var options = new SpectreConsoleLoggerOptions();

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            new SpectreConsoleLogger(null!, "cat", options, [ExcludeFromCodeCoverage]() => LogLevel.Information));
    }

    [TestMethod]
    public void Constructor_WithNullCategory_ThrowsException()
    {
        // Arrange
        var console = new TestConsole();
        var options = new SpectreConsoleLoggerOptions();

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            new SpectreConsoleLogger(console, null!, options, [ExcludeFromCodeCoverage]() => LogLevel.Information));
    }

    [TestMethod]
    public void Constructor_WithNullOptions_ThrowsException()
    {
        // Arrange
        var console = new TestConsole();

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            new SpectreConsoleLogger(console, "cat", null!, [ExcludeFromCodeCoverage]() => LogLevel.Information));
    }

    [TestMethod]
    public void Constructor_WithNullAccessor_ThrowsException()
    {
        // Arrange
        var console = new TestConsole();
        var options = new SpectreConsoleLoggerOptions();

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>(
            [ExcludeFromCodeCoverage] () => new SpectreConsoleLogger(console, "cat", options, null!));
    }

    [TestMethod]
    public void BeginScope_Always_ReturnsNull()
    {
        // Arrange
        var console = new TestConsole();
        var logger = CreateLogger(console);

        // Act
        var scope = logger.BeginScope("state");

        // Assert
        Assert.IsNull(scope);
    }

    [TestMethod]
    public void IsEnabled_WithLevelAtOrAboveMinimum_ReturnsTrue()
    {
        // Arrange
        var console = new TestConsole();
        var logger = CreateLogger(console, LogLevel.Information);

        // Act
        var result = logger.IsEnabled(LogLevel.Warning);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsEnabled_WithLevelBelowMinimum_ReturnsFalse()
    {
        // Arrange
        var console = new TestConsole();
        var logger = CreateLogger(console, LogLevel.Information);

        // Act
        var result = logger.IsEnabled(LogLevel.Debug);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsEnabled_WithNoneLevel_ReturnsFalse()
    {
        // Arrange
        var console = new TestConsole();
        var logger = CreateLogger(console, LogLevel.Trace);

        // Act
        var result = logger.IsEnabled(LogLevel.None);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Log_WithEnabledLevel_WritesMessage()
    {
        // Arrange
        var console = new TestConsole();
        var logger = CreateLogger(console, LogLevel.Information);

        // Act
        logger.LogInformation("hello world");

        // Assert
        Assert.Contains("hello world", console.Output);
        Assert.Contains("info", console.Output);
    }

    [TestMethod]
    public void Log_WithDisabledLevel_WritesNothing()
    {
        // Arrange
        var console = new TestConsole();
        var logger = CreateLogger(console, LogLevel.Warning);

        // Act
        logger.LogDebug("should not appear");

        // Assert
        Assert.AreEqual(string.Empty, console.Output);
    }

    [TestMethod]
    public void Log_WithCategoryEnabled_IncludesCategory()
    {
        // Arrange
        var console = new TestConsole();
        var options = new SpectreConsoleLoggerOptions { IncludeCategory = true };
        var logger = CreateLogger(console, LogLevel.Information, options);

        // Act
        logger.LogInformation("message");

        // Assert
        Assert.Contains("TestCategory", console.Output);
    }

    [TestMethod]
    public void Log_WithLevelLabelDisabled_OmitsLabel()
    {
        // Arrange
        var console = new TestConsole();
        var options = new SpectreConsoleLoggerOptions { IncludeLevelLabel = false };
        var logger = CreateLogger(console, LogLevel.Information, options);

        // Act
        logger.LogInformation("plain message");

        // Assert
        Assert.Contains("plain message", console.Output);
        Assert.DoesNotContain("info", console.Output);
    }

    [TestMethod]
    public void Log_WithTimestampEnabled_IncludesTimestamp()
    {
        // Arrange
        var console = new TestConsole();
        var options = new SpectreConsoleLoggerOptions { IncludeTimestamp = true };
        var logger = CreateLogger(console, LogLevel.Information, options);

        // Act
        logger.LogInformation("timed message");

        // Assert
        Assert.Contains("timed message", console.Output);
        Assert.Contains(":", console.Output);
    }

    [TestMethod]
    public void Log_WithException_RendersException()
    {
        // Arrange
        var console = new TestConsole();
        var logger = CreateLogger(console, LogLevel.Information);
        var exception = new InvalidOperationException("boom");

        // Act
        logger.LogError(exception, "operation failed");

        // Assert
        Assert.Contains("operation failed", console.Output);
        Assert.Contains("boom", console.Output);
    }

    [TestMethod]
    public void Log_WithEmptyMessageAndNoException_WritesNothing()
    {
        // Arrange
        var console = new TestConsole();
        var logger = CreateLogger(console, LogLevel.Information);

        // Act
        logger.LogInformation(string.Empty);

        // Assert
        Assert.AreEqual(string.Empty, console.Output);
    }

    [TestMethod]
    public void Log_WithNullFormatter_ThrowsException()
    {
        // Arrange
        var console = new TestConsole();
        var logger = CreateLogger(console, LogLevel.Information);

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>(
            [ExcludeFromCodeCoverage] () =>
                logger.Log<string>(LogLevel.Information, new EventId(0), "state", null, null!));
    }

    [TestMethod]
    public void Log_WithMarkupInMessage_EscapesContent()
    {
        // Arrange
        var console = new TestConsole();
        var logger = CreateLogger(console, LogLevel.Information);

        // Act
        logger.LogInformation("value is [red]not markup[/]");

        // Assert
        Assert.Contains("[red]not markup[/]", console.Output);
    }

    [TestMethod]
    public void Log_WithCriticalLevel_WritesMessage()
    {
        // Arrange
        var console = new TestConsole();
        var logger = CreateLogger(console, LogLevel.Trace);

        // Act
        logger.LogCritical("critical failure");

        // Assert
        Assert.Contains("critical failure", console.Output);
        Assert.Contains("crit", console.Output);
    }

    [TestMethod]
    public void Log_WithTraceAndDebugLevels_WritesMessages()
    {
        // Arrange
        var console = new TestConsole();
        var logger = CreateLogger(console, LogLevel.Trace);

        // Act
        logger.LogTrace("trace message");
        logger.LogDebug("debug message");

        // Assert
        Assert.Contains("trce", console.Output);
        Assert.Contains("dbug", console.Output);
    }
}
