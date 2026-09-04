//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.Spectre.Console.Extensions.Configuration;

/// <summary>
/// Extension methods that add Microsoft.Extensions.Configuration and Options binding to a
/// CommandAppBuilder. These hooks require that a DI container has already been configured,
/// for example by calling WithDIContainer.
/// </summary>
public static class ConfigurationCommandAppBuilderExtensions
{
    /// <summary>
    /// Builds an IConfiguration and registers it in the builder's DI container. By default the
    /// configuration reads from an optional appsettings.json file and environment variables. The
    /// optional configure delegate can add or replace configuration sources.
    /// </summary>
    /// <param name="builder">CommandAppBuilder to extend.</param>
    /// <param name="configure">
    /// [Optional] Delegate to customize the configuration sources. When null, the default sources
    /// (appsettings.json and environment variables) are used.
    /// </param>
    /// <returns>Returns the CommandAppBuilder.</returns>
    /// <exception cref="ArgumentNullException">When builder is null.</exception>
    /// <exception cref="InvalidOperationException">
    /// When no DI container has been configured. Call WithDIContainer before WithConfiguration.
    /// </exception>
    public static CommandAppBuilder WithConfiguration(
        this CommandAppBuilder builder,
        Action<IConfigurationBuilder>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var services = builder.GetServiceCollection();
        var configBuilder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory);
        if (configure is null)
        {
            configBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                         .AddEnvironmentVariables();
        }
        else
        {
            configure(configBuilder);
        }

        IConfiguration configuration = configBuilder.Build();
        services.AddSingleton(configuration);

        return builder;
    }

    /// <summary>
    /// Binds a configuration section to a strongly typed options class, registered so that it can
    /// be injected as IOptions&lt;TOptions&gt;. Data annotations on the options class are validated.
    /// Call WithConfiguration first so that an IConfiguration is available in the container.
    /// </summary>
    /// <typeparam name="TOptions">The options class to bind and register.</typeparam>
    /// <param name="builder">CommandAppBuilder to extend.</param>
    /// <param name="sectionName">The configuration section name to bind from.</param>
    /// <returns>Returns the CommandAppBuilder.</returns>
    /// <exception cref="ArgumentNullException">When builder is null.</exception>
    /// <exception cref="ArgumentException">When sectionName is null or whitespace.</exception>
    /// <exception cref="InvalidOperationException">
    /// When no DI container has been configured. Call WithDIContainer before WithOptions.
    /// </exception>
    public static CommandAppBuilder WithOptions<TOptions>(
        this CommandAppBuilder builder,
        string sectionName)
        where TOptions : class
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentException.ThrowIfNullOrWhiteSpace(sectionName);

        builder.GetServiceCollection()
               .AddOptions<TOptions>()
               .BindConfiguration(sectionName)
               .ValidateDataAnnotations();

        return builder;
    }
}
