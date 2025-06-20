using System.Globalization;

namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal static class AutoCompletionStrategy
{
    public static string AutoComplete(List<string> autocomplete, string text, bool isBackDirection) =>
        AutoComplete(autocomplete, text, isBackDirection, autocomplete.Find(i => i == text));

    private static string AutoComplete(List<string> autocomplete, string text, bool isBackDirection, string? found) =>
        found switch
        {
            null when !string.IsNullOrEmpty(text) => GetFuzzyMatch(autocomplete, text),
            null => autocomplete.First(),
            _ => GetAutoCompleteValue(GetDirectionFor(isBackDirection), autocomplete, found)
        };

    private static string GetFuzzyMatch(List<string> autocomplete, string text) =>
        autocomplete.Find(i => i.StartsWith(text, true, CultureInfo.InvariantCulture)) ?? autocomplete.First();

    private static string GetAutoCompleteValue(
        Direction autoCompleteDirection,
        List<string> autocomplete,
        string found)
    {
        var index = autoCompleteDirection == Direction.Forward ?
            (autocomplete.IndexOf(found) + 1) % autocomplete.Count :
            (autocomplete.IndexOf(found) - 1 + autocomplete.Count) % autocomplete.Count;

        return autocomplete.ElementAt(index);
    }

    private static Direction GetDirectionFor(bool isBackDirection) =>
        isBackDirection ? Direction.Backward : Direction.Forward;


    private enum Direction
    {
        Forward,
        Backward,
    }
}
