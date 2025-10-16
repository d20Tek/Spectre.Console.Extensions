//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Spectre.Console.Extensions.Settings;

/// <summary>
/// Supported verbosity levels that are shared among many cli tools.
/// </summary>
public enum VerbosityLevel
{
    /// <summary>
    /// Quiet or no output.
    /// </summary>
    Quiet = 0,

    /// <summary>
    /// Shorthand for quiet.
    /// </summary>
    Q = 0,

    /// <summary>
    /// Relatively little output
    /// </summary>
    Minimal = 1,

    /// <summary>
    /// Shorthand for minimal.
    /// </summary>
    M = 1,

    /// <summary>
    /// Standard output. This should be the default if verbosity level is not set.
    /// </summary>
    Normal = 2,

    /// <summary>
    /// Shorthand for normal.
    /// </summary>
    N = 2,

    /// <summary>
    /// Relatively verbose, but not exhaustive.
    /// </summary>
    Detailed = 3,

    /// <summary>
    /// Shorthand for detailed.
    /// </summary>
    D = 3,

    /// <summary>
    /// The most verbose and informative output used for debugging commands.
    /// </summary>
    Diagnostic = 4,

    /// <summary>
    /// Shortand for diagnostic
    /// </summary>
    Diag = 4,
}