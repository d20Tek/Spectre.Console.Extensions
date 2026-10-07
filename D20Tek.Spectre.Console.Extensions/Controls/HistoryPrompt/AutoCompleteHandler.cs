using Spectre.Console;
using System.Text;

namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal sealed class AutoCompleteHandler : IInputStateHandler
{
    private const string _backspaceText = "\b \b";

    public InputState Handle(ConsoleKeyInfo key, InputState state) =>
        ShouldHandle(key, state) ? ProcessAutoCompletion(key, state) : state;

    private static bool ShouldHandle(ConsoleKeyInfo key, InputState state) =>
        key.Key == ConsoleKey.Tab && (state.CompletionItems.Count > 0 || state.Request.CompletionProvider is not null);

    private InputState ProcessAutoCompletion(ConsoleKeyInfo key, InputState state)
    {
        var bufferText = state.Buffer.ToString();

        // Reuse the active completion set while cycling through it (buffer matches a previous
        // suggestion). Otherwise regenerate the completion set from the current typed text.
        var completionItems = state.Request.CompletionProvider is { } provider && !state.CompletionItems.Contains(bufferText)
            ? provider(bufferText)
            : state.CompletionItems;

        var replace = AutoCompletionStrategy.AutoComplete(
            completionItems,
            bufferText,
            key.Modifiers.HasFlag(ConsoleModifiers.Shift));

        return RenderSuggestion(state.Request.AnsiConsole, replace, state.Buffer, state with { CompletionItems = completionItems });
    }

    private InputState RenderSuggestion(IAnsiConsole console, string replace, StringBuilder buffer, InputState state)
    {
        console.Write(_backspaceText.Repeat(state.Buffer.Length), state.Request.PromptStyle);
        console.Write(replace);
        buffer.Clear()
              .Insert(0, replace);

        return state with { CursorIndex = buffer.Length, Handled = true };
    }
}
