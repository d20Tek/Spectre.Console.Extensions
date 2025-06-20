using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal sealed class UpArrowHandler : IInputStateHandler
{
    private const string _blank = " ";

    public InputState Handle(ConsoleKeyInfo key, InputState state)
    {
        var historyCount = state.Request.History.Count;
        if (ShouldHandle(key, historyCount, state.HistoryIndex))
        {
            ErasePrevious(state);

            var historyIndex = GetNextHistoryIndex(state.HistoryIndex, historyCount);
            var savedHistory = GetSavedHistoryIfExists(state, historyIndex);
            var entryText = GetPreviousHistoryEntry(state.Request.History, historyIndex);
            var cursorIndex = UpdateBuffer(state, entryText);

            return state with
            {
                SavedHistory = savedHistory,
                HistoryIndex = historyIndex,
                CursorIndex = cursorIndex,
                Handled = true
            };
        }

        return state;
    }

    private bool ShouldHandle(ConsoleKeyInfo key, int historyCount, int historyIndex) =>
        key.Key == ConsoleKey.UpArrow && historyCount > 0 && historyIndex < historyCount;

    private void ErasePrevious(InputState state)
    {
        var bufferLength = state.Buffer.Length;

        var amountToMoveCursorLeft = bufferLength == 0 ? 0 : state.CursorIndex;
        state.Request.AnsiConsole.Cursor.MoveLeft(amountToMoveCursorLeft);
        state.Request.AnsiConsole.Write(_blank.Repeat(bufferLength));
        state.Request.AnsiConsole.Cursor.MoveLeft(bufferLength);
    }

    private static int GetNextHistoryIndex(int currentIndex, int historyCount) =>
        Math.Min(currentIndex + 1, historyCount - 1);

    private static string? GetSavedHistoryIfExists(InputState state, int historyIndex) =>
        historyIndex == 0 ? state.Buffer.ToString() : state.SavedHistory;

    private static string GetPreviousHistoryEntry(IEnumerable<string> history, int index) =>
        history.Reverse().Skip(index).First();

    private static int UpdateBuffer(InputState state, string newContent)
    {
        state.Buffer.Clear();

        if (!string.IsNullOrEmpty(newContent))
        {
            state.Buffer.Insert(0, newContent);
            state.Request.AnsiConsole.Write(newContent);
        }

        return state.Buffer.Length;
    }
}
