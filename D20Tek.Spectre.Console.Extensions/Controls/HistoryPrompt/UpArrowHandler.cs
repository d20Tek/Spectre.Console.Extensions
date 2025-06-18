using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal sealed class UpArrowHandler : IInputStateHandler
{
    public InputState Handle(ConsoleKeyInfo key, InputState state)
    {
        var historyCount = state.Request.History.Count;
        if (ShouldHandle(key, historyCount, state.HistoryIndex))
        {
            var bufferLength = state.Buffer.Length;

            var amountToMoveCursorLeft = bufferLength == 0 ? 0 : state.CursorIndex;
            state.Request.AnsiConsole.Cursor.MoveLeft(amountToMoveCursorLeft);
            state.Request.AnsiConsole.Write(" ".Repeat(bufferLength));
            state.Request.AnsiConsole.Cursor.MoveLeft(bufferLength);

            string? savedHistory = null;
            var historyIndex = state.HistoryIndex + 1;
            if (historyIndex > historyCount)
            {
                historyIndex = historyCount;
            }
            else if (historyIndex == 0)
            {
                savedHistory = state.Buffer.ToString();
            }

            var history = state.Request.History.AsEnumerable();
            var prev = history.Skip(historyIndex).Take(1).FirstOrDefault();
            state.Buffer.Clear();
            var cursorIndex = 0;
            if (prev != null)
            {
                state.Buffer.Insert(0, prev);
                state.Request.AnsiConsole.Write(state.Buffer.ToString());
                cursorIndex = state.Buffer.Length;
            }

            return state with { SavedHistory = savedHistory, HistoryIndex = historyIndex, CursorIndex = cursorIndex, Handled = true };
        }

        return state;
    }

    private bool ShouldHandle(ConsoleKeyInfo key, int historyCount, int historyIndex) =>
        key.Key == ConsoleKey.UpArrow && historyCount > 0 && historyIndex < historyCount;
}
