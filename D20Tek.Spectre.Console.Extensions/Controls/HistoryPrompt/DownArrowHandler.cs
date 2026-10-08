using Spectre.Console;
using System.Text;

namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal sealed class DownArrowHandler : IInputStateHandler
{
    private const string _blank = " ";

    public InputState Handle(ConsoleKeyInfo key, InputState state)
    {
        if (key.Key == ConsoleKey.DownArrow && ShouldDescend(state))
        {
            return CompletionDescendHelper.TryDescendIntoDirectory(state) with { Handled = true };
        }

        if (ShouldHandle(key, state.Request.History.Count, state.HistoryIndex))
        {
            ErasePrevious(state);
            UpdateBufferToNextEntry(state.HistoryIndex - 1, state);
            RenderUpdatedBuffer(state.Buffer, state.Request.AnsiConsole);

            return state with
            {
                HistoryIndex = state.HistoryIndex - 1,
                CursorIndex = state.Buffer.Length,
                Handled = true
            };
        }

        return state;
    }

    // DownArrow descends into a directory suggestion when there is no history to navigate
    // and the cursor is at the end of a buffer that names a directory. This mirrors
    // RightArrow's descend behavior for completion-aware prompts such as PathPrompt.
    private static bool ShouldDescend(InputState state) =>
        state.Request.CompletionProvider is not null
        && state.Request.History.Count == 0
        && state.CursorIndex == state.Buffer.Length;

    private static bool ShouldHandle(ConsoleKeyInfo key, int historyCount, int historyIndex) =>
        key.Key == ConsoleKey.DownArrow && historyCount > 0 && historyIndex > -1;

    private static void ErasePrevious(InputState state)
    {
        var bufferLength = state.Buffer.Length;
        ArgumentOutOfRangeException.ThrowIfZero(bufferLength);

        var console = state.Request.AnsiConsole;
        console.Cursor.MoveLeft(state.CursorIndex);
        console.Write(_blank.Repeat(bufferLength));
        console.Cursor.MoveLeft(bufferLength);
    }

    private static void UpdateBufferToNextEntry(int historyIndex, InputState state)
    {
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
    }

    private static void RenderUpdatedBuffer(StringBuilder buffer, IAnsiConsole console) =>
        console.Write(buffer.ToString());
}
