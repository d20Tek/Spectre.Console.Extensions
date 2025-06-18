using Spectre.Console;
using System.Text;

namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal sealed class AutoCompleteHandler : IInputStateHandler
{
    public InputState Handle(ConsoleKeyInfo key, InputState state) => 
        ShouldHandle(key, state) ? ProcessAutoCompletion(key, state) : state;

    private bool ShouldHandle(ConsoleKeyInfo key, InputState state) =>
        key.Key == ConsoleKey.Tab && state.CompletionItems.Count > 0;

    private InputState ProcessAutoCompletion(ConsoleKeyInfo key, InputState state)
    {
        var replace = AutoCompletionStrategy.AutoComplete(
            state.CompletionItems,
            state.Buffer.ToString(),
            key.Modifiers.HasFlag(ConsoleModifiers.Shift));

        return (!string.IsNullOrEmpty(replace)) ?
            RenderSuggestion(state.Request.AnsiConsole, replace, state.Buffer, state) :
            state;
    }

    private InputState RenderSuggestion(IAnsiConsole console, string replace, StringBuilder buffer, InputState state)
    {
        console.Write("\b \b".Repeat(state.Buffer.Length), state.Request.PromptStyle);
        console.Write(replace);
        buffer.Clear()
              .Insert(0, replace);

        return state with { CursorIndex = buffer.Length, Handled = true };
    }
}
