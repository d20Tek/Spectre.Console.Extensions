//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Spectre.Console.Extensions.Logging;

/// <summary>
/// Options that control how the Spectre console logger renders log entries.
/// </summary>
public sealed class SpectreConsoleLoggerOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether the log level label (for example, "info")
    /// is included at the start of each rendered line. Defaults to true.
    /// </summary>
    public bool IncludeLevelLabel { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the log category (typically the source
    /// type name) is included in each rendered line. Defaults to false.
    /// </summary>
    public bool IncludeCategory { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a timestamp is included at the start of
    /// each rendered line. Defaults to false.
    /// </summary>
    public bool IncludeTimestamp { get; set; }

    /// <summary>
    /// Gets or sets the format string used to render the timestamp when
    /// <see cref="IncludeTimestamp"/> is enabled. Defaults to "HH:mm:ss".
    /// </summary>
    public string TimestampFormat { get; set; } = "HH:mm:ss";
}
