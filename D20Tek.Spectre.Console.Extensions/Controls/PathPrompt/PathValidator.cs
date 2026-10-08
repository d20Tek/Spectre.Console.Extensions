//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Controls;

internal sealed class PathValidator(
    bool mustExist,
    PathKind pathKind,
    string baseDirectory,
    string[]? extensions,
    string? errorMessage,
    string? defaultValue)
{
    private static class Errors
    {
        public static string Required() => "[red]Please enter a path.[/]";

        public static string Extension(string input, string[] extensions) =>
            $"[red]'{input}' must have one of the following extensions: {string.Join(", ", extensions)}.[/]";

        public static string FileNotFound(string input) => $"[red]File '{input}' does not exist.[/]";

        public static string DirectoryNotFound(string input) => $"[red]Directory '{input}' does not exist.[/]";

        public static string PathNotFound(string input) => $"[red]Path '{input}' does not exist.[/]";
    }

    public ValidationResult Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return defaultValue is not null
                ? ValidationResult.Success()
                : ValidationResult.Error(errorMessage ?? Errors.Required());
        }

        var resolved = ResolvePath(input);
        if (HasInvalidExtension(resolved))
        {
            return ValidationResult.Error(errorMessage ?? Errors.Extension(input, extensions!));
        }

        return mustExist ? ValidateExists(input, resolved) : ValidationResult.Success();
    }

    private string ResolvePath(string input) => Path.IsPathRooted(input) ? input : Path.Combine(baseDirectory, input);

    private bool HasInvalidExtension(string resolved) =>
        extensions is { Length: > 0 } &&
        pathKind != PathKind.Directory &&
        !Directory.Exists(resolved) &&
        !extensions.Any(ext => resolved.EndsWith(ext, StringComparison.OrdinalIgnoreCase));

    private ValidationResult ValidateExists(string input, string resolved) =>
        pathKind switch
        {
            PathKind.File => 
                File.Exists(resolved)
                    ? ValidationResult.Success()
                    : ValidationResult.Error(errorMessage ?? Errors.FileNotFound(input)),
            PathKind.Directory => 
                Directory.Exists(resolved)
                    ? ValidationResult.Success()
                    : ValidationResult.Error(errorMessage ?? Errors.DirectoryNotFound(input)),
            _ => 
                File.Exists(resolved) || Directory.Exists(resolved)
                    ? ValidationResult.Success()
                    : ValidationResult.Error(errorMessage ?? Errors.PathNotFound(input))
        };
}
