//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Hosting;
using D20Tek.Spectre.Console.Extensions.UnitTests.Hosting.Fakes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Hosting;

[TestClass]
public class HostStartupExtensionsTests
{
    [TestMethod]
    public void WithStartup_WithNullBuilder_ThrowsException()
    {
        // Arrange - Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            HostStartupExtensions.WithStartup<GreetStartup>(null!));
    }

    [TestMethod]
    public void WithStartup_ReturnsSameBuilder()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();

        // Act
        var result = builder.WithStartup<GreetStartup>();

        // Assert
        Assert.AreSame(builder, result);
    }

    [TestMethod]
    public void WithStartup_RunsConfigureServicesPreBuild()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();

        // Act
        builder.WithStartup<GreetStartup>();
        using var host = builder.Build();

        // Assert - IGreetingService was registered by the startup's ConfigureServices.
        var service = host.Services.GetService<IGreetingService>();
        Assert.IsInstanceOfType<GreetingService>(service);
    }

    [TestMethod]
    public void WithStartup_RegistersStartupInstance()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();

        // Act
        builder.WithStartup<GreetStartup>();
        using var host = builder.Build();

        // Assert - the startup instance is registered so ConfigureCommands can run post-build.
        var startup = host.Services.GetService<HostStartupBase>();
        Assert.IsInstanceOfType<GreetStartup>(startup);
    }
}
