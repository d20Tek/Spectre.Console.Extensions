//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Logging;
using D20Tek.Spectre.Console.Extensions.Settings;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions;

/// <summary>
/// Extension methods that add verbosity-aware logging to a CommandAppBuilder.
/// </summary>
public static class LoggingCommandAppBuilderExtensions
{
    /// <summary>
    /// Adds logging that renders through an IAnsiConsole to the builder's DI container, with the
    /// minimum log level derived from the supplied verbosity level. Requires that a DI container
    /// supporting lifetimes has already been configured, for example by calling WithDIContainer.
    /// </summary>
    /// <param name="builder">CommandAppBuilder to extend.</param>
    /// <param name="minimumVerbosity">
    /// [Optional] The verbosity level that sets the minimum LogLevel to emit. Defaults to Normal.
    /// </param>
    /// <param name="console">
    /// [Optional] Console used to render log entries. Defaults to AnsiConsole.Console when null.
    /// </param>
    /// <param name="configure">
    /// [Optional] Delegate to configure how log entries are rendered.
    /// </param>
    /// <returns>Returns the CommandAppBuilder.</returns>
    /// <exception cref="ArgumentNullException">When builder is null.</exception>
    /// <exception cref="InvalidOperationException">
    /// When no registrar has been configured or the registrar does not support lifetimes.
    /// </exception>
    public static CommandAppBuilder WithLogging(
        this CommandAppBuilder builder,
        VerbosityLevel minimumVerbosity = VerbosityLevel.Normal,
        IAnsiConsole? console = null,
        Action<SpectreConsoleLoggerOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var services = builder.GetServiceCollection();
        if (console is not null)
        {
            services.AddSingleton(console);
        }

        services.AddLogging(logging => logging.AddSpectreConsole(minimumVerbosity, configure));

        return builder;
    }
}
