//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Hosting;

/// <summary>
/// Fluent builder that creates a Spectre.Console.Cli <see cref="CommandApp"/> bridged to an
/// already-built .NET Generic Host. It mirrors the ergonomics of CommandAppBuilder while
/// letting the host own configuration, options, logging, services, and lifetime. Command types
/// resolve from the host's <see cref="IServiceProvider"/> via a <see cref="HostTypeRegistrar"/>.
/// </summary>
public sealed class HostCommandAppBuilder
{
    private readonly IHost _host;
    private Action<IConfigurator>? _configureCommands;
    private Action<CommandApp>? _setDefaultCommand;

    internal CommandApp? App { get; private set; }

    /// <summary>
    /// Constructor that takes the built host used to resolve command types.
    /// </summary>
    /// <param name="host">The built host whose service provider resolves command types.</param>
    /// <exception cref="ArgumentNullException">When host is null.</exception>
    public HostCommandAppBuilder(IHost host)
    {
        ArgumentNullException.ThrowIfNull(host, nameof(host));
        _host = host;
    }

    /// <summary>
    /// Gets the host associated with this builder.
    /// </summary>
    public IHost Host => _host;

    /// <summary>
    /// Sets the default command to run when no command name is specified.
    /// </summary>
    /// <typeparam name="TDefault">The default command type.</typeparam>
    /// <returns>Returns the HostCommandAppBuilder.</returns>
    public HostCommandAppBuilder WithDefaultCommand<TDefault>() where TDefault : class, ICommand
    {
        _setDefaultCommand = app => app.SetDefaultCommand<TDefault>();
        return this;
    }

    /// <summary>
    /// Configures the CommandApp's commands.
    /// </summary>
    /// <param name="configure">Delegate to configure commands.</param>
    /// <returns>Returns the HostCommandAppBuilder.</returns>
    /// <exception cref="ArgumentNullException">When configure is null.</exception>
    public HostCommandAppBuilder ConfigureCommands(Action<IConfigurator> configure)
    {
        ArgumentNullException.ThrowIfNull(configure, nameof(configure));
        _configureCommands = configure;
        return this;
    }

    /// <summary>
    /// Builds the CommandApp bridged to the host, applying the default command and command
    /// configuration specified on this builder. When a <see cref="HostStartupBase"/> was
    /// registered on the host (via WithStartup), its ConfigureCommands is applied as well.
    /// </summary>
    /// <returns>Returns the HostCommandAppBuilder.</returns>
    public HostCommandAppBuilder Build()
    {
        var registrar = new HostTypeRegistrar(_host.Services);
        var app = new CommandApp(registrar);

        _setDefaultCommand?.Invoke(app);

        var startups = _host.Services.GetServices<HostStartupBase>();
        foreach (var startup in startups)
        {
            app.Configure(config => startup.ConfigureCommands(config));
        }

        if (_configureCommands is not null)
        {
            app.Configure(_configureCommands);
        }

        App = app;
        return this;
    }

    /// <summary>
    /// Runs the built CommandApp asynchronously. Calls <see cref="Build"/> automatically when
    /// the app has not yet been built.
    /// </summary>
    /// <param name="args">Command line arguments to run with.</param>
    /// <returns>The application's exit code.</returns>
    /// <exception cref="ArgumentNullException">When args is null.</exception>
    public Task<int> RunAsync(string[] args)
    {
        ArgumentNullException.ThrowIfNull(args, nameof(args));

        App ??= Build().App;
        return App!.RunAsync(args);
    }

    /// <summary>
    /// Runs the built CommandApp synchronously. Calls <see cref="Build"/> automatically when
    /// the app has not yet been built.
    /// </summary>
    /// <param name="args">Command line arguments to run with.</param>
    /// <returns>The application's exit code.</returns>
    /// <exception cref="ArgumentNullException">When args is null.</exception>
    public int Run(string[] args)
    {
        ArgumentNullException.ThrowIfNull(args, nameof(args));

        App ??= Build().App;
        return App!.Run(args);
    }
}
