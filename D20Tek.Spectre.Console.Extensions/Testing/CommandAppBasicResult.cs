//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Spectre.Console.Extensions.Testing;

/// <summary>
/// The basic test result returned from running command app tests.
/// </summary>
/// <remarks>
/// Constructor for basic result.
/// </remarks>
/// <param name="exitCode">Initial command app exit code.</param>
/// <param name="output">Initial command app output text.</param>
public class CommandAppBasicResult(int exitCode, string? output)
{
    /// <summary>
    /// Gets the exit code returned by the command app.
    /// </summary>
    public int ExitCode { get; } = exitCode;

    /// <summary>
    /// Gets the output text from the command app. This text contains the console
    /// output produced by the command app.
    /// </summary>
    public string Output { get; } = output ?? string.Empty;
}
