namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal static class StringExtensions
{
    internal static string Repeat(this string text, int count) =>
        (text, count) switch
        {
            (null, _) => throw new ArgumentNullException(nameof(text)),
            (_, <= 0) => string.Empty,
            (_, 1)    => text,
            _         => string.Concat(Enumerable.Repeat(text, count))
        };
}
