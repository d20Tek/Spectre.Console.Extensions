//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Configuration;
using D20Tek.Spectre.Console.Extensions.UnitTests.Configuration.Fakes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Configuration;

[TestClass]
public class ConfigurationCommandAppBuilderExtensions_WithConfigurationTests
{
    [TestMethod]
    public void WithConfiguration_WithNullBuilder_ThrowsException()
    {
        // Arrange

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            ConfigurationCommandAppBuilderExtensions.WithConfiguration(null!));
    }

    [TestMethod]
    public void WithConfiguration_WithoutRegistrar_ThrowsException()
    {
        // Arrange
        var builder = new CommandAppBuilder();

        // Act - Assert
        Assert.ThrowsExactly<InvalidOperationException>([ExcludeFromCodeCoverage] () => builder.WithConfiguration());
    }

    [TestMethod]
    public void WithConfiguration_WithDIContainer_ReturnsBuilder()
    {
        // Arrange
        var builder = new CommandAppBuilder().WithDIContainer();

        // Act
        var result = builder.WithConfiguration();

        // Assert
        Assert.AreSame(builder, result);
    }

    [TestMethod]
    public void WithConfiguration_WithDefaultSources_RegistersConfiguration()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = new CommandAppBuilder().WithDIContainer(services);

        // Act
        builder.WithConfiguration();

        // Assert
        var provider = services.BuildServiceProvider();
        var configuration = provider.GetService<IConfiguration>();
        Assert.IsNotNull(configuration);
    }

    [TestMethod]
    public void WithConfiguration_WithCustomSource_BindsProvidedValues()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = new CommandAppBuilder().WithDIContainer(services);
        var values = new Dictionary<string, string?>
        {
            ["Sample:Name"] = "custom-value",
        };

        // Act
        builder.WithConfiguration(config => config.AddInMemoryCollection(values));

        // Assert
        var provider = services.BuildServiceProvider();
        var configuration = provider.GetRequiredService<IConfiguration>();
        Assert.AreEqual("custom-value", configuration["Sample:Name"]);
    }

    [TestMethod]
    public void WithConfiguration_WithCustomSource_DoesNotUseDefaultSources()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = new CommandAppBuilder().WithDIContainer(services);

        // Act
        builder.WithConfiguration(config => config.AddInMemoryCollection([]));

        // Assert
        var provider = services.BuildServiceProvider();
        var configuration = provider.GetRequiredService<IConfiguration>();
        Assert.IsEmpty(configuration.GetChildren());
    }
}

[TestClass]
public class ConfigurationCommandAppBuilderExtensions_WithOptionsTests
{
    [TestMethod]
    public void WithOptions_WithNullBuilder_ThrowsException()
    {
        // Arrange

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>(
            [ExcludeFromCodeCoverage] () =>
                ConfigurationCommandAppBuilderExtensions.WithOptions<SampleOptions>(null!, "Sample"));
    }

    [TestMethod]
    public void WithOptions_WithNullSectionName_ThrowsException()
    {
        // Arrange
        var builder = new CommandAppBuilder().WithDIContainer();

        // Act - Assert
        Assert.ThrowsExactly<ArgumentNullException>(
            [ExcludeFromCodeCoverage] () => builder.WithOptions<SampleOptions>(null!));
    }

    [TestMethod]
    public void WithOptions_WithWhitespaceSectionName_ThrowsException()
    {
        // Arrange
        var builder = new CommandAppBuilder().WithDIContainer();

        // Act - Assert
        Assert.ThrowsExactly<ArgumentException>(
            [ExcludeFromCodeCoverage] () => builder.WithOptions<SampleOptions>("   "));
    }

    [TestMethod]
    public void WithOptions_WithoutRegistrar_ThrowsException()
    {
        // Arrange
        var builder = new CommandAppBuilder();

        // Act - Assert
        Assert.ThrowsExactly<InvalidOperationException>(
            [ExcludeFromCodeCoverage] () => builder.WithOptions<SampleOptions>("Sample"));
    }

    [TestMethod]
    public void WithOptions_WithDIContainer_ReturnsBuilder()
    {
        // Arrange
        var builder = new CommandAppBuilder().WithDIContainer();
        builder.WithConfiguration(config =>
            config.AddInMemoryCollection(new Dictionary<string, string?>()));

        // Act
        var result = builder.WithOptions<SampleOptions>("Sample");

        // Assert
        Assert.AreSame(builder, result);
    }

    [TestMethod]
    public void WithOptions_WithBoundSection_ResolvesTypedOptions()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = new CommandAppBuilder().WithDIContainer(services);
        var values = new Dictionary<string, string?>
        {
            ["Sample:Name"] = "bound-name",
            ["Sample:Count"] = "7",
        };

        // Act
        builder.WithConfiguration(config => config.AddInMemoryCollection(values))
               .WithOptions<SampleOptions>("Sample");

        // Assert
        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<SampleOptions>>();
        Assert.AreEqual("bound-name", options.Value.Name);
        Assert.AreEqual(7, options.Value.Count);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void WithOptions_WithInvalidValues_ThrowsOnResolve()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = new CommandAppBuilder().WithDIContainer(services);
        var values = new Dictionary<string, string?>
        {
            ["Sample:Name"] = string.Empty,
        };
        builder.WithConfiguration(config => config.AddInMemoryCollection(values))
               .WithOptions<SampleOptions>("Sample");
        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<SampleOptions>>();

        // Act - Assert
        Assert.ThrowsExactly<Microsoft.Extensions.Options.OptionsValidationException>(
            () => _ = options.Value);
    }
}
