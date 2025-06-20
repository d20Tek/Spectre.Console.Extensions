using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal sealed class DownArrowHandler : IInputStateHandler
{
    public InputState Handle(ConsoleKeyInfo key, InputState state)
    {
        var historyCount = state.Request.History.Count;
        if (ShouldHandle(key, historyCount, state.HistoryIndex))
        {
            var bufferLength = state.Buffer.Length;
            ArgumentOutOfRangeException.ThrowIfZero(bufferLength);

            var amountToMoveCursorLeft = state.CursorIndex;
            state.Request.AnsiConsole.Cursor.MoveLeft(amountToMoveCursorLeft);
            state.Request.AnsiConsole.Write(" ".Repeat(bufferLength));
            state.Request.AnsiConsole.Cursor.MoveLeft(bufferLength);

            var historyIndex = state.HistoryIndex - 1;
            state.Buffer.Clear();

            if (historyIndex == -1)
            {
                if (state.SavedHistory != null)
                {
                    state.Buffer.Insert(0, state.SavedHistory);
                }
            }
            else
            {
                var history = state.Request.History.AsEnumerable();
                state.Buffer.Insert(0, history.Reverse().Skip(historyIndex).First());
            }

            state.Request.AnsiConsole.Write(state.Buffer.ToString());
            var cursorIndex = state.Buffer.Length;

            return state with { HistoryIndex = historyIndex, CursorIndex = cursorIndex, Handled = true };
        }

        return state;
    }

    private bool ShouldHandle(ConsoleKeyInfo key, int historyCount, int historyIndex) =>
        key.Key == ConsoleKey.DownArrow && historyCount > 0 && historyIndex > -1;
}
