//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Spectre.Console.Extensions.Controls;

/// <summary>
/// Specifies the kind of filesystem entry a <see cref="PathPrompt"/> should validate the input against.
/// </summary>
public enum PathKind
{
    /// <summary>
    /// Accepts either a file or a directory.
    /// </summary>
    Any,

    /// <summary>
    /// Requires the path to resolve to a file.
    /// </summary>
    File,

    /// <summary>
    /// Requires the path to resolve to a directory.
    /// </summary>
    Directory
}
