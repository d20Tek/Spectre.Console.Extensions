using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal sealed record ReadLineRequest(
    IAnsiConsole AnsiConsole,
    Style PromptStyle,
    bool IsSecret,
    char? Mask,
    List<string> Items,
    List<string> History)
{
    internal static ReadLineRequest Create<T>(HistoryTextPrompt<T> prompt, IAnsiConsole console) =>
        new(
            console,
            prompt.PromptStyle ?? Style.Plain,
            prompt.IsSecret,
            prompt.Mask,
            [.. prompt.Choices.Select(choice => prompt.Converter(choice))],
            prompt.History);
}
