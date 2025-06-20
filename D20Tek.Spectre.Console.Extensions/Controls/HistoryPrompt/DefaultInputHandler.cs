using Spectre.Console;
using System.Text;

namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal sealed class DefaultInputHandler : IInputStateHandler
{
    public InputState Handle(ConsoleKeyInfo key, InputState state) =>
        ShouldHandle(key) ? state with { CursorIndex = ProcessKey(key, state), Handled = true } : state;

    private bool ShouldHandle(ConsoleKeyInfo key) => !char.IsControl(key.KeyChar);

    private int ProcessKey(ConsoleKeyInfo key, InputState state) =>
        (state.CursorIndex == state.Buffer.Length, state.InsertMode) switch
        {
            (true, _) => AppendCharacter(state.Buffer, key.KeyChar, state.Request, state.CursorIndex),
            (false, true) => InsertCharacter(state.Buffer, key.KeyChar, state.Request, state.CursorIndex),
            _ => OverwriteCharacter(state.Buffer, key.KeyChar, state.Request, state.CursorIndex)
        };

    private int AppendCharacter(StringBuilder buffer, char keyChar, ReadLineRequest request, int cursor)
    {
        buffer.Append(keyChar);
        WriteTextWithSecret(keyChar.ToString(), request);
        return cursor + 1;
    }

    private int InsertCharacter(StringBuilder buffer, char keyChar, ReadLineRequest request, int cursor)
    {
        buffer.Insert(cursor, keyChar);
        WriteTextWithSecret(keyChar.ToString(), request);
        WriteTextWithSecret(buffer.ToString()[(cursor + 1)..], request);
        request.AnsiConsole.Cursor.MoveLeft(buffer.Length - cursor - 1);
        return cursor + 1;
    }

    private int OverwriteCharacter(StringBuilder buffer, char keyChar, ReadLineRequest request, int cursor)
    {
        ArgumentOutOfRangeException.ThrowIfZero(buffer.Length);
        buffer[cursor] = keyChar;
        WriteTextWithSecret(keyChar.ToString(), request);
        return cursor + 1;
    }

    private void WriteTextWithSecret(string text, ReadLineRequest request) =>
        request.AnsiConsole.Write(request.IsSecret ? text.Mask(request.Mask) : text, request.PromptStyle);
}
