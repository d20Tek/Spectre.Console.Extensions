using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal sealed record ReadLineRequest(
    IAnsiConsole AnsiConsole,
    Style PromptStyle,
    bool IsSecret,
    char? Mask,
    List<string> Items,
    List<string> History);
