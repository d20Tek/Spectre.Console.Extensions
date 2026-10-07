using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal sealed class EnterHandler : IInputStateHandler
{
    public InputState Handle(ConsoleKeyInfo key, InputState state) =>
        (key.Key == ConsoleKey.Enter) ? state with { Handled = true, Done = true } : state;
}

internal sealed class InsertHandler : IInputStateHandler
{
    public InputState Handle(ConsoleKeyInfo key, InputState state) =>
        ShouldHandle(key) ? state with { InsertMode = !state.InsertMode, Handled = true } : state;

    private static bool ShouldHandle(ConsoleKeyInfo key) =>
        key.Key == ConsoleKey.Insert ||
            key.Modifiers.HasFlag(ConsoleModifiers.Shift) && key.Key == ConsoleKey.F12 ||
            key.Key == ConsoleKey.F20;
        // Shift-F12 on a normal keyboard
        // Shift-F12 on an Apple keyboard is F20
}

internal sealed class LeftArrowHandler : IInputStateHandler
{
    public InputState Handle(ConsoleKeyInfo key, InputState state)
    {
        if (ShouldHandle(key))
        {
            if (state.CursorIndex <= 0) return state with { Handled = true };

            state.Request.AnsiConsole.Cursor.Show();
            state.Request.AnsiConsole.Cursor.MoveLeft(1);
            return state with { CursorIndex = state.CursorIndex - 1, Handled = true };
        }

        return state;
    }

    private static bool ShouldHandle(ConsoleKeyInfo key) => key.Key == ConsoleKey.LeftArrow;
}

internal sealed class RightArrowHandler : IInputStateHandler
{
    public InputState Handle(ConsoleKeyInfo key, InputState state)
    {
        if (ShouldHandle(key))
        {
            if (state.CursorIndex > state.Buffer.Length - 1)
            {
                return CompletionDescendHelper.TryDescendIntoDirectory(state) with { Handled = true };
            }

            state.Request.AnsiConsole.Cursor.Show();
            state.Request.AnsiConsole.Cursor.MoveRight(1);
            return state with { CursorIndex = state.CursorIndex + 1, Handled = true };
        }

        return state;
    }

    private static bool ShouldHandle(ConsoleKeyInfo key) => key.Key == ConsoleKey.RightArrow;
}

// Shared helper that lets completion-aware prompts (such as PathPrompt) descend into a
// directory suggestion so that subsequent Tab presses cycle its contents instead of the
// parent directory's siblings. Used by both RightArrowHandler and DownArrowHandler.
internal static class CompletionDescendHelper
{
    public static InputState TryDescendIntoDirectory(InputState state)
    {
        if (state.Request.CompletionProvider is not { } provider)
        {
            return state;
        }

        var bufferText = state.Buffer.ToString();
        if (bufferText.Length == 0 || !bufferText.EndsWith(Path.DirectorySeparatorChar)
            && !bufferText.EndsWith(Path.AltDirectorySeparatorChar))
        {
            return state;
        }

        return state with { CompletionItems = provider(bufferText) };
    }
}
