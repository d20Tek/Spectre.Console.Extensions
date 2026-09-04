//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Logging;
using D20Tek.Spectre.Console.Extensions.Settings;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Logging;

[TestClass]
public class SpectreLoggingExtensionsTests
{
    [TestMethod]
    public void AddSpectreConsole_WithNullBuilder_ThrowsException()
    {
        // Arrange

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>(
            [ExcludeFromCodeCoverage] () => SpectreLoggingExtensions.AddSpectreConsole(null!));
    }

    [TestMethod]
    public void AddSpectreConsole_WithDefaults_RegistersProvider()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddLogging(b => b.AddSpectreConsole());

        // Assert
        var provider = services.BuildServiceProvider();
        var loggerProvider = provider.GetServices<ILoggerProvider>()
                                     .OfType<SpectreConsoleLoggerProvider>()
                                     .SingleOrDefault();
        Assert.IsNotNull(loggerProvider);
    }

    [TestMethod]
    public void AddSpectreConsole_WithVerbosity_SetsMappedMinimumLevel()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddLogging(b => b.AddSpectreConsole(VerbosityLevel.Detailed));

        // Assert
        var provider = services.BuildServiceProvider();
        var loggerProvider = provider.GetServices<ILoggerProvider>()
                                     .OfType<SpectreConsoleLoggerProvider>()
                                     .Single();
        Assert.AreEqual(LogLevel.Debug, loggerProvider.MinimumLevel);
    }

    [TestMethod]
    public void AddSpectreConsole_WithConfigureAndRegisteredConsole_LogsThroughConsole()
    {
        // Arrange
        var console = new TestConsole();
        var services = new ServiceCollection();
        services.AddSingleton<global::Spectre.Console.IAnsiConsole>(console);

        // Act
        services.AddLogging(b => b.AddSpectreConsole(
            VerbosityLevel.Normal,
            options => options.IncludeCategory = true));

        // Assert
        var provider = services.BuildServiceProvider();
        var logger = provider.GetRequiredService<ILogger<SpectreLoggingExtensionsTests>>();
        logger.LogInformation("through console");
        Assert.Contains("through console", console.Output);
    }
}
