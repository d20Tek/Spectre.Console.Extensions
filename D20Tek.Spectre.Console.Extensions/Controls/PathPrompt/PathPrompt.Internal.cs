//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;
using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Controls;

public sealed partial class PathPrompt
{
    private string ShowInternal(IAnsiConsole console) =>
        ShowInternalAsync(console, CancellationToken.None).GetAwaiter().GetResult();

    private async Task<string> ShowInternalAsync(IAnsiConsole console, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(console);

        return await console.RunExclusive(async () =>
        {
            var validator = CreateValidator();
            var request = CreateReadLineRequest(console);

            console.Markup(BuildPromptMarkup());

            while (true)
            {
                var input = await console.ReadLine(request, token).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(input) && _defaultValue is not null)
                {
                    console.WriteLine();
                    return _defaultValue;
                }

                var result = validator.Validate(input);
                if (result.Successful)
                {
                    console.WriteLine();
                    return input;
                }

                console.WriteLine();
                console.MarkupLine(result.Message!);
                console.Markup(BuildPromptMarkup());
            }
        }).ConfigureAwait(false);
    }

    private PathValidator CreateValidator() =>
        new(_mustExist, _pathKind, _baseDirectory, _extensions, _errorMessage, _defaultValue);

    private ReadLineRequest CreateReadLineRequest(IAnsiConsole console) =>
        new(console, _style, false, null, [], [], GetCompletionEntries);

    private List<string> GetCompletionEntries(string typedText)
    {
        var (resolvedDirectory, typedDirectory, prefix) = SplitTypedPath(typedText);

        if (!Directory.Exists(resolvedDirectory)) return [];

        return [.. Directory.EnumerateFileSystemEntries(resolvedDirectory)
            .Where(IncludeEntry)
            .Where(entry => Path.GetFileName(entry).StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .Select(entry =>
                typedDirectory
                + Path.GetFileName(entry)
                + (Directory.Exists(entry) ? Path.DirectorySeparatorChar.ToString() : string.Empty))
            .OrderBy(entry => entry, StringComparer.OrdinalIgnoreCase)];
    }

    internal (string ResolvedDirectory, string TypedDirectory, string Prefix) SplitTypedPath(string typedText)
    {
        if (string.IsNullOrEmpty(typedText)) return (_baseDirectory, string.Empty, string.Empty);

        var typedDirectoryPart = Path.GetDirectoryName(typedText) ?? string.Empty;
        var prefix = Path.GetFileName(typedText);

        if (string.IsNullOrEmpty(typedDirectoryPart))
        {
            var resolved = Path.IsPathRooted(typedText) ? Path.GetPathRoot(typedText)! : _baseDirectory;
            return (resolved, string.Empty, prefix);
        }

        var resolvedDirectoryPart = Path.IsPathRooted(typedDirectoryPart)
            ? typedDirectoryPart
            : Path.Combine(_baseDirectory, typedDirectoryPart);

        var typedDirectoryWithSeparator = typedDirectoryPart.EndsWith(Path.DirectorySeparatorChar)
            || typedDirectoryPart.EndsWith(Path.AltDirectorySeparatorChar)
                ? typedDirectoryPart
                : typedDirectoryPart + Path.DirectorySeparatorChar;

        return (resolvedDirectoryPart, typedDirectoryWithSeparator, prefix);
    }

    private bool IncludeEntry(string entry)
    {
        var name = Path.GetFileName(entry);
        if (!_includeHidden && name.StartsWith('.')) return false;

        if (_extensions is { Length: > 0 } && _pathKind != PathKind.Directory && !Directory.Exists(entry))
        {
            return _extensions.Any(ext => entry.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }

        return true;
    }

    private string BuildPromptMarkup()
    {
        var label = _promptLabel.TrimEnd();
        return _defaultValue is not null
            ? $"{label} [green]({_defaultValue})[/]: "
            : $"{label}: ";
    }
}
