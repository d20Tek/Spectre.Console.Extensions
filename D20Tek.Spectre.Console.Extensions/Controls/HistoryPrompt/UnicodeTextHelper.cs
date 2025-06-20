using Spectre.Console;
using System.Diagnostics.CodeAnalysis;
using Wcwidth;

namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal static class UnicodeTextHelper
{
    private const string _backspaceText = "\b \b";
    private const string _backspaceUnicodeText = "\b \b\b \b";

    [ExcludeFromCodeCoverage]
    public static void HandleMask(char charToRemove, char? mask, IAnsiConsole console) =>
        console.Write((mask, UnicodeCalculator.GetWidth(charToRemove)) switch
        {
            (not null, 1) => _backspaceText,
            (not null, 2) => _backspaceUnicodeText,
            _ => string.Empty
        });
}
