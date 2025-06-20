using Spectre.Console;
using System.Text;

namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal sealed class DownArrowHandler : IInputStateHandler
{
    private const string _blank = " ";

    public InputState Handle(ConsoleKeyInfo key, InputState state)
    {
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

    private bool ShouldHandle(ConsoleKeyInfo key, int historyCount, int historyIndex) =>
        key.Key == ConsoleKey.DownArrow && historyCount > 0 && historyIndex > -1;

    private void ErasePrevious(InputState state)
    {
        var bufferLength = state.Buffer.Length;
        ArgumentOutOfRangeException.ThrowIfZero(bufferLength);

        var console = state.Request.AnsiConsole;
        console.Cursor.MoveLeft(state.CursorIndex);
        console.Write(_blank.Repeat(bufferLength));
        console.Cursor.MoveLeft(bufferLength);
    }

    private void UpdateBufferToNextEntry(int historyIndex, InputState state)
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

    private void RenderUpdatedBuffer(StringBuilder buffer, IAnsiConsole console) =>
        console.Write(buffer.ToString());
}
