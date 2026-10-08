//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Controls;

/// <summary>
/// A text prompt for filesystem path input. Validates that the entered path exists (optionally
/// restricted to a file or a directory, and to a set of extensions), and offers tab auto-completion
/// against the entries of the base directory by leveraging the existing
/// <see cref="HistoryTextPrompt{T}"/> auto-complete infrastructure.
/// </summary>
public sealed partial class PathPrompt : IPrompt<string>
{
    private readonly string _promptLabel;
    private string? _defaultValue;
    private string _baseDirectory;
    private PathKind _pathKind = PathKind.Any;
    private string[]? _extensions;
    private bool _mustExist = true;
    private bool _includeHidden;
    private string? _errorMessage;
    private Style _style = Style.Plain;

    /// <summary>
    /// Constructor that takes a prompt label string.
    /// </summary>
    /// <param name="promptLabel">Prompt label text.</param>
    /// <exception cref="ArgumentNullException">An exception when no string is provided.</exception>
    public PathPrompt(string promptLabel)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(promptLabel, nameof(promptLabel));
        _promptLabel = promptLabel;
        _baseDirectory = Directory.GetCurrentDirectory();
    }

    /// <summary>
    /// Sets the base directory used to resolve relative paths and to list auto-complete entries.
    /// Defaults to the current working directory.
    /// </summary>
    /// <param name="baseDirectory">The base directory to use.</param>
    /// <returns>Current prompt</returns>
    public PathPrompt WithBaseDirectory(string baseDirectory)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(baseDirectory, nameof(baseDirectory));
        _baseDirectory = baseDirectory;
        return this;
    }

    /// <summary>
    /// Restricts the kind of filesystem entry the path must resolve to when <see cref="MustExist(bool)"/>
    /// is enabled. Defaults to <see cref="PathKind.Any"/>.
    /// </summary>
    /// <param name="pathKind">The kind of path required.</param>
    /// <returns>Current prompt</returns>
    public PathPrompt WithPathKind(PathKind pathKind)
    {
        _pathKind = pathKind;
        return this;
    }

    /// <summary>
    /// Restricts accepted paths to the specified file extensions (for example, ".json", ".txt").
    /// Also filters the auto-complete entries to matching files and directories.
    /// </summary>
    /// <param name="extensions">The allowed file extensions.</param>
    /// <returns>Current prompt</returns>
    public PathPrompt WithExtensions(params string[] extensions)
    {
        ArgumentNullException.ThrowIfNull(extensions);
        _extensions = extensions;
        return this;
    }

    /// <summary>
    /// Sets a default value to use for the prompt when the user presses Enter without input.
    /// </summary>
    /// <param name="value">The default path value.</param>
    /// <returns>Current prompt</returns>
    public PathPrompt WithDefaultValue(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        _defaultValue = value;
        return this;
    }

    /// <summary>
    /// Sets a custom error message to use when the prompt has input errors.
    /// </summary>
    /// <param name="message">Custom error message to show.</param>
    /// <returns>Current prompt</returns>
    public PathPrompt WithErrorMessage(string message)
    {
        _errorMessage = message;
        return this;
    }

    /// <summary>
    /// Sets the prompt style to use when displaying its label text.
    /// </summary>
    /// <param name="promptStyle">Prompt style to use.</param>
    /// <returns>Current prompt</returns>
    public PathPrompt WithPromptStyle(Style promptStyle)
    {
        _style = promptStyle;
        return this;
    }

    /// <summary>
    /// Controls whether the entered path must exist on disk. Enabled by default.
    /// </summary>
    /// <param name="mustExist">Whether existence validation is enforced.</param>
    /// <returns>Current prompt</returns>
    public PathPrompt MustExist(bool mustExist = true)
    {
        _mustExist = mustExist;
        return this;
    }

    /// <summary>
    /// Controls whether hidden files and directories are included in the auto-complete entries.
    /// Disabled by default.
    /// </summary>
    /// <param name="includeHidden">Whether hidden entries should be included.</param>
    /// <returns>Current prompt</returns>
    public PathPrompt IncludeHidden(bool includeHidden = true)
    {
        _includeHidden = includeHidden;
        return this;
    }

    /// <inheritdoc/>
    public string Show(IAnsiConsole console) => ShowInternal(console);

    /// <inheritdoc/>
    public Task<string> ShowAsync(IAnsiConsole console, CancellationToken token) => ShowInternalAsync(console, token);
}
