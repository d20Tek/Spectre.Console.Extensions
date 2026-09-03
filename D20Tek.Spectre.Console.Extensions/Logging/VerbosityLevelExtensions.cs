//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Settings;
using Microsoft.Extensions.Logging;

namespace D20Tek.Spectre.Console.Extensions.Logging;

/// <summary>
/// Extension methods that map between the library's VerbosityLevel and the
/// Microsoft.Extensions.Logging LogLevel, so a single verbosity switch can drive
/// both console output and log filtering.
/// </summary>
public static class VerbosityLevelExtensions
{
    /// <summary>
    /// Converts a VerbosityLevel into the minimum LogLevel that should be emitted.
    /// </summary>
    /// <param name="verbosity">The verbosity level to convert.</param>
    /// <returns>
    /// The minimum LogLevel to emit. Quiet maps to Error, Minimal to Warning,
    /// Normal to Information, Detailed to Debug, and Diagnostic to Trace.
    /// </returns>
    public static LogLevel ToLogLevel(this VerbosityLevel verbosity) =>
        verbosity switch
        {
            VerbosityLevel.Quiet => LogLevel.Error,
            VerbosityLevel.Minimal => LogLevel.Warning,
            VerbosityLevel.Normal => LogLevel.Information,
            VerbosityLevel.Detailed => LogLevel.Debug,
            VerbosityLevel.Diagnostic => LogLevel.Trace,
            _ => LogLevel.Information,
        };

    /// <summary>
    /// Converts a LogLevel into the nearest VerbosityLevel.
    /// </summary>
    /// <param name="logLevel">The log level to convert.</param>
    /// <returns>
    /// The nearest VerbosityLevel. Trace maps to Diagnostic, Debug to Detailed,
    /// Information to Normal, Warning to Minimal, and Error, Critical, or None to Quiet.
    /// </returns>
    public static VerbosityLevel ToVerbosityLevel(this LogLevel logLevel) =>
        logLevel switch
        {
            LogLevel.Trace => VerbosityLevel.Diagnostic,
            LogLevel.Debug => VerbosityLevel.Detailed,
            LogLevel.Information => VerbosityLevel.Normal,
            LogLevel.Warning => VerbosityLevel.Minimal,
            LogLevel.Error => VerbosityLevel.Quiet,
            LogLevel.Critical => VerbosityLevel.Quiet,
            LogLevel.None => VerbosityLevel.Quiet,
            _ => VerbosityLevel.Normal,
        };
}
