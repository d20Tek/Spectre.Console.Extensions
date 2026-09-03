//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.Logging;
using Spectre.Console;
using System.Collections.Concurrent;

namespace D20Tek.Spectre.Console.Extensions.Logging;

/// <summary>
/// An ILoggerProvider that creates SpectreConsoleLogger instances, rendering log output
/// through an IAnsiConsole with a configurable minimum log level.
/// </summary>
public sealed class SpectreConsoleLoggerProvider : ILoggerProvider
{
    private readonly IAnsiConsole _console;
    private readonly SpectreConsoleLoggerOptions _options;
    private readonly ConcurrentDictionary<string, SpectreConsoleLogger> _loggers = new();

    /// <summary>
    /// Gets or sets the minimum LogLevel that created loggers will emit. Changing this value
    /// affects loggers that have already been created.
    /// </summary>
    public LogLevel MinimumLevel { get; set; } = LogLevel.Information;

    /// <summary>
    /// Constructor that takes the console and rendering options.
    /// </summary>
    /// <param name="console">Console used to render log entries.</param>
    /// <param name="options">
    /// [Optional] Options controlling how entries are rendered. A default instance is used when null.
    /// </param>
    /// <exception cref="ArgumentNullException">When console is null.</exception>
    public SpectreConsoleLoggerProvider(IAnsiConsole console, SpectreConsoleLoggerOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(console);
        _console = console;
        _options = options ?? new SpectreConsoleLoggerOptions();
    }

    /// <inheritdoc/>
    public ILogger CreateLogger(string categoryName) =>
        _loggers.GetOrAdd(
            categoryName,
            name => new SpectreConsoleLogger(_console, name, _options, () => MinimumLevel));

    /// <inheritdoc/>
    public void Dispose() => _loggers.Clear();
}
