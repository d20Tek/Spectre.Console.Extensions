using Spectre.Console;
using System.Text;

namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal sealed class BackspaceHandler : IInputStateHandler
{
    public InputState Handle(ConsoleKeyInfo key, InputState state)
    {
        if (ShouldHandle(key))
        {
            return CanBackspace(state.Buffer, state.CursorIndex) ? 
                ProcessBackspace(state.Buffer, state.CursorIndex, state) :
                state with { Handled = true };
        }
        return state;
    }

    private static bool ShouldHandle(ConsoleKeyInfo key) => key.Key == ConsoleKey.Backspace;

    private static bool CanBackspace(StringBuilder builder, int cursorIndex) => builder.Length > 0 && cursorIndex >= 1;

    private InputState ProcessBackspace(StringBuilder builder, int cursorIndex, InputState state)
    {
        cursorIndex = RemoveCharacter(builder, cursorIndex, state);
        if (cursorIndex != builder.Length && cursorIndex >= 0)
        {
            RenderUpdate(builder, cursorIndex, state.Request);
        }

        return state with { CursorIndex = cursorIndex, Handled = true };
    }

    private static int RemoveCharacter(StringBuilder builder, int cursorIndex, InputState state)
    {
        var characterToRemove = builder[cursorIndex - 1];
        builder.Remove(cursorIndex - 1, 1);

        UnicodeTextHelper.HandleMask(characterToRemove, state.Request.Mask, state.Request.AnsiConsole);

        return cursorIndex - 1;
    }

    private void RenderUpdate(StringBuilder builder, int cursorIndex, ReadLineRequest request)
    {
        var textUpdate = GetUpdatedText(builder, cursorIndex, request.IsSecret, request.Mask);
        request.AnsiConsole.Write($"{textUpdate} ", request.PromptStyle);
        request.AnsiConsole.Cursor.MoveLeft(builder.Length - cursorIndex + 1);
    }

    private static string GetUpdatedText(StringBuilder builder, int index, bool isSecret, char? mask) =>
        isSecret ? builder.ToString()[index..].Mask(mask) : builder.ToString()[index..];
}
