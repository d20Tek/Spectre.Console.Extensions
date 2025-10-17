//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Testing;

/// <summary>
/// App result with basic data and additional context and settings for the 
/// command that was run
/// </summary>
/// <remarks>
/// Constructor.
/// </remarks>
/// <param name="exitCode">Exit code.</param>
/// <param name="output">Console output text.</param>
/// <param name="context">Command context.</param>
/// <param name="settings">Command settings</param>
public class CommandAppResult(
    int exitCode,
    string output,
    CommandContext? context,
    CommandSettings? settings) : CommandAppBasicResult(exitCode, output)
{
    /// <summary>
    /// Gets the command context for this execution result.
    /// </summary>
    public CommandContext? Context { get; } = context;

    /// <summary>
    /// Gets the command settings for this execution result.
    /// </summary>
    public CommandSettings? Settings { get; } = settings;
}
