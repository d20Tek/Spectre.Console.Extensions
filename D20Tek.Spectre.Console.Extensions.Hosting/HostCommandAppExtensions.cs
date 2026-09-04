//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Hosting;

/// <summary>
/// Extension methods that run a Spectre.Console.Cli <see cref="CommandApp"/> using an
/// already-built .NET Generic Host. Command types resolve from the host's
/// <see cref="IServiceProvider"/>, so they can inject configuration, options, logging, and any
/// hosted services registered with the host.
/// </summary>
public static class HostCommandAppExtensions
{
    /// <summary>
    /// Creates a <see cref="CommandApp"/> bridged to the host and runs it asynchronously.
    /// </summary>
    /// <param name="host">The built host whose service provider resolves command types.</param>
    /// <param name="args">Command line arguments to run with.</param>
    /// <param name="configure">Delegate to configure the CommandApp's commands.</param>
    /// <returns>The application's exit code.</returns>
    /// <exception cref="ArgumentNullException">When host, args, or configure is null.</exception>
    public static Task<int> RunCommandAppAsync(
        this IHost host,
        string[] args,
        Action<IConfigurator> configure)
    {
        ArgumentNullException.ThrowIfNull(host, nameof(host));
        ArgumentNullException.ThrowIfNull(args, nameof(args));
        ArgumentNullException.ThrowIfNull(configure, nameof(configure));

        var app = host.CreateCommandApp(configure);
        return app.RunAsync(args);
    }

    /// <summary>
    /// Creates a <see cref="CommandApp"/> bridged to the host and runs it synchronously.
    /// </summary>
    /// <param name="host">The built host whose service provider resolves command types.</param>
    /// <param name="args">Command line arguments to run with.</param>
    /// <param name="configure">Delegate to configure the CommandApp's commands.</param>
    /// <returns>The application's exit code.</returns>
    /// <exception cref="ArgumentNullException">When host, args, or configure is null.</exception>
    public static int RunCommandApp(
        this IHost host,
        string[] args,
        Action<IConfigurator> configure)
    {
        ArgumentNullException.ThrowIfNull(host, nameof(host));
        ArgumentNullException.ThrowIfNull(args, nameof(args));
        ArgumentNullException.ThrowIfNull(configure, nameof(configure));

        var app = host.CreateCommandApp(configure);
        return app.Run(args);
    }

    /// <summary>
    /// Creates a <see cref="CommandApp"/> bridged to the host using a
    /// <see cref="HostTypeRegistrar"/>, applying any registered <see cref="HostStartupBase"/>
    /// command configuration followed by the supplied command configuration.
    /// </summary>
    /// <param name="host">The built host whose service provider resolves command types.</param>
    /// <param name="configure">Delegate to configure the CommandApp's commands.</param>
    /// <returns>The configured CommandApp.</returns>
    /// <exception cref="ArgumentNullException">When host or configure is null.</exception>
    public static CommandApp CreateCommandApp(this IHost host, Action<IConfigurator> configure)
    {
        ArgumentNullException.ThrowIfNull(host, nameof(host));
        ArgumentNullException.ThrowIfNull(configure, nameof(configure));

        var registrar = new HostTypeRegistrar(host.Services);
        var app = new CommandApp(registrar);

        foreach (var startup in host.Services.GetServices<HostStartupBase>())
        {
            app.Configure(config => startup.ConfigureCommands(config));
        }

        app.Configure(configure);
        return app;
    }

    /// <summary>
    /// Creates a fluent <see cref="HostCommandAppBuilder"/> bridged to the host.
    /// </summary>
    /// <param name="host">The built host whose service provider resolves command types.</param>
    /// <returns>A new <see cref="HostCommandAppBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">When host is null.</exception>
    public static HostCommandAppBuilder CreateCommandAppBuilder(this IHost host)
    {
        ArgumentNullException.ThrowIfNull(host, nameof(host));
        return new HostCommandAppBuilder(host);
    }
}
