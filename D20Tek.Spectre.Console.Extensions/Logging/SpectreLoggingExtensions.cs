//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Logging;

/// <summary>
/// Extension methods that add the Spectre console logger provider to an ILoggingBuilder.
/// </summary>
public static class SpectreLoggingExtensions
{
    /// <summary>
    /// Adds a logger provider that renders output through an IAnsiConsole. The logging builder's
    /// minimum level is also set from the mapped verbosity so that levels below Information (Debug
    /// and Trace) are emitted when a more detailed verbosity is requested.
    /// </summary>
    /// <param name="builder">The logging builder to extend.</param>
    /// <param name="minimumVerbosity">
    /// [Optional] The verbosity level that sets the minimum LogLevel to emit. Defaults to Normal.
    /// </param>
    /// <param name="configure">
    /// [Optional] Delegate to configure how log entries are rendered.
    /// </param>
    /// <returns>The logging builder, for chaining.</returns>
    /// <exception cref="ArgumentNullException">When builder is null.</exception>
    public static ILoggingBuilder AddSpectreConsole(
        this ILoggingBuilder builder,
        VerbosityLevel minimumVerbosity = VerbosityLevel.Normal,
        Action<SpectreConsoleLoggerOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var options = new SpectreConsoleLoggerOptions();
        configure?.Invoke(options);

        var minimumLevel = minimumVerbosity.ToLogLevel();
        builder.SetMinimumLevel(minimumLevel);

        builder.Services.TryAddSingleton<IAnsiConsole>(_ => AnsiConsole.Console);
        builder.Services.AddSingleton<ILoggerProvider>(sp =>
        {
            var console = sp.GetRequiredService<IAnsiConsole>();
            return new SpectreConsoleLoggerProvider(console, options)
            {
                MinimumLevel = minimumLevel,
            };
        });

        return builder;
    }
}
