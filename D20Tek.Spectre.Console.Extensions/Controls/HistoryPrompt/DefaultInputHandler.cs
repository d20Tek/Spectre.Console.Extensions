using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal sealed class DefaultInputHandler : IInputStateHandler
{
    public InputState Handle(ConsoleKeyInfo key, InputState state)
    {
        if (ShouldHandle(key))
        {
            var cursorIndex = state.CursorIndex;
            if (cursorIndex == state.Buffer.Length)
            {
                state.Buffer.Append(key.KeyChar);
                cursorIndex++;
                WriteTextWithSecret(key.KeyChar.ToString(), state.Request);
            }
            else if (state.InsertMode)
            {
                state.Buffer.Insert(cursorIndex, key.KeyChar);
                WriteTextWithSecret(key.KeyChar.ToString(), state.Request);
                WriteTextWithSecret(state.Buffer.ToString()[(cursorIndex + 1)..], state.Request);
                state.Request.AnsiConsole.Cursor.MoveLeft(state.Buffer.Length - cursorIndex - 1);
                cursorIndex++;
            }
            else
            {
                if (state.Buffer.Length == 0)
                {
                    state.Buffer.Append(key.KeyChar);
                }
                else
                {
                    state.Buffer[cursorIndex] = key.KeyChar;
                }
                cursorIndex++;
                WriteTextWithSecret(key.KeyChar.ToString(), state.Request);
            }

            return state with { CursorIndex = cursorIndex, Handled = true };
        }

        return state;
    }

    private bool ShouldHandle(ConsoleKeyInfo key) => !char.IsControl(key.KeyChar);

    private void WriteTextWithSecret(string text, ReadLineRequest request) =>
        request.AnsiConsole.Write(request.IsSecret ? text.Mask(request.Mask) : text, request.PromptStyle);
}
