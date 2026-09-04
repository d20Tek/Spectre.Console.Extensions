//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace D20Tek.Spectre.Console.Extensions.Hosting;

/// <summary>
/// Extension methods that wire a <see cref="HostStartupBase"/> into the Generic Host lifecycle.
/// </summary>
public static class HostStartupExtensions
{
    /// <summary>
    /// Registers a <see cref="HostStartupBase"/> with the host builder. The startup's
    /// <see cref="HostStartupBase.ConfigureServices"/> runs immediately against the host's
    /// service collection (pre-build), and the startup instance is registered so that
    /// <see cref="HostStartupBase.ConfigureCommands"/> is applied automatically when the
    /// CommandApp is built by <see cref="HostCommandAppBuilder"/> or the
    /// <see cref="HostCommandAppExtensions"/> methods (post-build).
    /// </summary>
    /// <typeparam name="TStartup">The startup type, which must derive from
    /// <see cref="HostStartupBase"/> and have a public parameterless constructor.</typeparam>
    /// <param name="builder">The host application builder to configure.</param>
    /// <returns>The same host application builder.</returns>
    /// <exception cref="ArgumentNullException">When builder is null.</exception>
    public static IHostApplicationBuilder WithStartup<TStartup>(this IHostApplicationBuilder builder)
        where TStartup : HostStartupBase, new()
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));

        var startup = new TStartup();
        startup.ConfigureServices(builder.Services);
        builder.Services.AddSingleton<HostStartupBase>(startup);

        return builder;
    }
}
