//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Settings;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Logging;

[TestClass]
public class LoggingCommandAppBuilderExtensionsTests
{
    [TestMethod]
    public void WithLogging_WithNullBuilder_ThrowsException()
    {
        // Arrange

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>(
            [ExcludeFromCodeCoverage] () =>
                LoggingCommandAppBuilderExtensions.WithLogging(null!));
    }

    [TestMethod]
    public void WithLogging_WithoutRegistrar_ThrowsException()
    {
        // Arrange
        var builder = new CommandAppBuilder();

        // Act - Assert
        Assert.ThrowsExactly<InvalidOperationException>(
            [ExcludeFromCodeCoverage] () => builder.WithLogging());
    }

    [TestMethod]
    public void WithLogging_WithDIContainer_ReturnsBuilder()
    {
        // Arrange
        var builder = new CommandAppBuilder().WithDIContainer();

        // Act
        var result = builder.WithLogging();

        // Assert
        Assert.AreSame(builder, result);
    }

    [TestMethod]
    public void WithLogging_WithDIContainer_RegistersLoggingServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = new CommandAppBuilder().WithDIContainer(services);

        // Act
        builder.WithLogging(VerbosityLevel.Detailed);

        // Assert
        var provider = services.BuildServiceProvider();
        var logger = provider.GetService<ILogger<LoggingCommandAppBuilderExtensionsTests>>();
        Assert.IsNotNull(logger);
    }

    [TestMethod]
    public void WithLogging_WithCustomConsole_LogsThroughConsole()
    {
        // Arrange
        var console = new TestConsole();
        var services = new ServiceCollection();
        var builder = new CommandAppBuilder().WithDIContainer(services);

        // Act
        builder.WithLogging(VerbosityLevel.Normal, console);

        // Assert
        var provider = services.BuildServiceProvider();
        var logger = provider.GetRequiredService<ILogger<LoggingCommandAppBuilderExtensionsTests>>();
        logger.LogWarning("warned");
        Assert.Contains("warned", console.Output);
    }

    [TestMethod]
    public void WithLogging_WithConfigureOptions_AppliesOptions()
    {
        // Arrange
        var console = new TestConsole();
        var services = new ServiceCollection();
        var builder = new CommandAppBuilder().WithDIContainer(services);

        // Act
        builder.WithLogging(
            VerbosityLevel.Normal,
            console,
            options => options.IncludeCategory = true);

        // Assert
        var provider = services.BuildServiceProvider();
        var logger = provider.GetRequiredService<ILogger<LoggingCommandAppBuilderExtensionsTests>>();
        logger.LogInformation("categorized");
        var output = console.Output.Replace("\n", string.Empty).Replace("\r", string.Empty);
        Assert.Contains(nameof(LoggingCommandAppBuilderExtensionsTests), output);
    }
}
