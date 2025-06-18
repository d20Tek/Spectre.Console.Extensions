namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal interface IInputStateHandler
{
    InputState Handle(ConsoleKeyInfo key, InputState state);
}
