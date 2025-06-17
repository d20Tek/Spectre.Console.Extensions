namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal static class StringExtensions
{
    internal static string Repeat(this string text, int count)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        if (count <= 0)
        {
            return string.Empty;
        }

        if (count == 1)
        {
            return text;
        }

        return string.Concat(Enumerable.Repeat(text, count));
    }
}
