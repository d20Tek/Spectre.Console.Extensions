//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Logging;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Logging;

[TestClass]
public class SpectreConsoleLoggerProviderTests
{
    [TestMethod]
    public void Constructor_WithNullConsole_ThrowsException()
    {
        // Arrange

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () => new SpectreConsoleLoggerProvider(null!));
    }

    [TestMethod]
    public void Constructor_WithDefaults_SetsInformationMinimumLevel()
    {
        // Arrange
        var console = new TestConsole();

        // Act
        using var provider = new SpectreConsoleLoggerProvider(console);

        // Assert
        Assert.AreEqual(LogLevel.Information, provider.MinimumLevel);
    }

    [TestMethod]
    public void Constructor_WithNullOptions_UsesDefaultOptions()
    {
        // Arrange
        var console = new TestConsole();

        // Act
        using var provider = new SpectreConsoleLoggerProvider(console, null);

        // Assert
        Assert.IsNotNull(provider.CreateLogger("cat"));
    }

    [TestMethod]
    public void CreateLogger_WithCategory_ReturnsLogger()
    {
        // Arrange
        var console = new TestConsole();
        using var provider = new SpectreConsoleLoggerProvider(console);

        // Act
        var logger = provider.CreateLogger("MyCategory");

        // Assert
        Assert.IsNotNull(logger);
    }

    [TestMethod]
    public void CreateLogger_WithSameCategoryTwice_ReturnsSameInstance()
    {
        // Arrange
        var console = new TestConsole();
        using var provider = new SpectreConsoleLoggerProvider(console);

        // Act
        var first = provider.CreateLogger("Shared");
        var second = provider.CreateLogger("Shared");

        // Assert
        Assert.AreSame(first, second);
    }

    [TestMethod]
    public void MinimumLevel_WhenChanged_AffectsCreatedLoggers()
    {
        // Arrange
        var console = new TestConsole();
        using var provider = new SpectreConsoleLoggerProvider(console)
        {
            MinimumLevel = LogLevel.Warning,
        };
        var logger = provider.CreateLogger("cat");

        // Act
        provider.MinimumLevel = LogLevel.Trace;

        // Assert
        Assert.IsTrue(logger.IsEnabled(LogLevel.Debug));
    }

    [TestMethod]
    public void Dispose_AfterCreatingLoggers_DoesNotThrow()
    {
        // Arrange
        var console = new TestConsole();
        var provider = new SpectreConsoleLoggerProvider(console);
        provider.CreateLogger("cat");

        // Act
        provider.Dispose();

        // Assert
        Assert.IsNotNull(provider);
    }
}
