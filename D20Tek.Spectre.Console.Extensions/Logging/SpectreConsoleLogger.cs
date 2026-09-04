//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Logging;

/// <summary>
/// An ILogger implementation that renders log entries through an IAnsiConsole, so that
/// log output is styled consistently with the rest of a Spectre.Console application and
/// can be captured by a test console.
/// </summary>
public sealed class SpectreConsoleLogger : ILogger
{
    private readonly IAnsiConsole _console;
    private readonly string _categoryName;
    private readonly SpectreConsoleLoggerOptions _options;
    private readonly Func<LogLevel> _minLevelAccessor;

    /// <summary>
    /// Constructor that takes the console, category, options, and a minimum level accessor.
    /// </summary>
    /// <param name="console">Console used to render log entries.</param>
    /// <param name="categoryName">Category (typically the source type name) for this logger.</param>
    /// <param name="options">Options controlling how entries are rendered.</param>
    /// <param name="minLevelAccessor">
    /// Accessor that returns the current minimum LogLevel to emit. A delegate is used so the
    /// level can be changed after the logger is created.
    /// </param>
    /// <exception cref="ArgumentNullException">When any argument is null.</exception>
    public SpectreConsoleLogger(
        IAnsiConsole console,
        string categoryName,
        SpectreConsoleLoggerOptions options,
        Func<LogLevel> minLevelAccessor)
    {
        ArgumentNullException.ThrowIfNull(console);
        ArgumentNullException.ThrowIfNull(categoryName);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(minLevelAccessor);

        _console = console;
        _categoryName = categoryName;
        _options = options;
        _minLevelAccessor = minLevelAccessor;
    }

    /// <inheritdoc/>
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    /// <inheritdoc/>
    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None && logLevel >= _minLevelAccessor();

    /// <inheritdoc/>
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        ArgumentNullException.ThrowIfNull(formatter);

        if (!IsEnabled(logLevel)) return;

        var message = formatter(state, exception);
        if (string.IsNullOrEmpty(message) && exception is null) return;

        var line = BuildLine(logLevel, message);
        _console.MarkupLine(line);

        if (exception is not null)
        {
            _console.WriteException(exception, ExceptionFormats.ShortenEverything);
        }
    }

    private string BuildLine(LogLevel logLevel, string message)
    {
        var builder = new System.Text.StringBuilder();

        if (_options.IncludeTimestamp)
        {
            builder.Append('[').Append("grey").Append(']')
                   .Append(Markup.Escape(DateTime.Now.ToString(_options.TimestampFormat)))
                   .Append("[/] ");
        }

        if (_options.IncludeLevelLabel)
        {
            var (label, color) = GetLevelDisplay(logLevel);
            builder.Append('[').Append(color).Append(']')
                   .Append(label)
                   .Append("[/] ");
        }

        if (_options.IncludeCategory)
        {
            builder.Append("[grey]").Append(Markup.Escape(_categoryName)).Append("[/] ");
        }

        builder.Append(Markup.Escape(message));
        return builder.ToString();
    }

    private static (string Label, string Color) GetLevelDisplay(LogLevel logLevel) =>
        logLevel switch
        {
            LogLevel.Debug => ("dbug", "grey"),
            LogLevel.Information => ("info", "green"),
            LogLevel.Warning => ("warn", "yellow"),
            LogLevel.Error => ("fail", "red"),
            LogLevel.Critical => ("crit", "white on red"),
            _ => ("trce", "grey"),
        };
}
