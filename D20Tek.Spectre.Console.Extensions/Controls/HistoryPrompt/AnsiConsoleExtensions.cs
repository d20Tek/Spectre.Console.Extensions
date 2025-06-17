using Spectre.Console;
using System.Text;
using Wcwidth;

namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal static class AnsiConsoleExtensions
{
    internal static async Task<string> ReadLine(this IAnsiConsole console, Style? style, bool secret, char? mask, IEnumerable<string>? items = null, IEnumerable<string>? history = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(console);

        style ??= Style.Plain;
        StringBuilder buffer = new();
        var cursorIndex = 0;
        var insertMode = false;

        var autocomplete = new List<string>(items ?? []);
        var historyIndex = -1;
        string? prehistorySaved = null;

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var rawKey = await console.Input.ReadKeyAsync(true, cancellationToken).ConfigureAwait(false);
            if (rawKey == null)
            {
                continue;
            }

            var key = rawKey.Value;
            if (key.Key == ConsoleKey.Enter)
            {
                return buffer.ToString();
            }

            if (key.Key == ConsoleKey.Tab && autocomplete.Count > 0)
            {
                var replace = AutoCompletionStrategy.AutoComplete(
                    autocomplete,
                    buffer.ToString(),
                    key.Modifiers.HasFlag(ConsoleModifiers.Shift));

                if (!string.IsNullOrEmpty(replace))
                {
                    // Render the suggestion
                    console.Write("\b \b".Repeat(buffer.Length), style);
                    console.Write(replace);
                    buffer.Clear();
                    buffer.Insert(0, replace);
                    cursorIndex = buffer.Length;
                    continue;
                }
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (buffer.Length > 0 && cursorIndex >= 1)
                {
                    var characterToRemove = buffer[cursorIndex - 1];
                    buffer.Remove(cursorIndex - 1, 1);
                    cursorIndex--;

                    if (mask != null)
                    {
                        if (UnicodeCalculator.GetWidth(characterToRemove) == 1)
                        {
                            console.Write("\b \b");
                        }
                        else if (UnicodeCalculator.GetWidth(characterToRemove) == 2)
                        {
                            console.Write("\b \b\b \b");
                        }
                    }

                    if (cursorIndex != buffer.Length && cursorIndex >= 0)
                    {
                        console.Write(secret ? buffer.ToString()[cursorIndex..].Mask(mask) : buffer.ToString()[cursorIndex..], style);
                        console.Write(" ", style);
                        console.Cursor.MoveLeft(buffer.Length - cursorIndex + 1);
                    }
                }

                continue;
            }

            if (key.Key == ConsoleKey.Insert
                || key.Modifiers.HasFlag(ConsoleModifiers.Shift) && key.Key == ConsoleKey.F12
                || key.Key == ConsoleKey.F20)
            {
                // Shift-F12 on a normal keyboard
                // Shift-F12 on an Apple keyboard is F20
                insertMode = !insertMode;
            }

            if (key.Key == ConsoleKey.LeftArrow && cursorIndex > 0)
            {
                console.Cursor.Show();
                console.Cursor.MoveLeft(1);
                cursorIndex--;
                continue;
            }

            if (key.Key == ConsoleKey.RightArrow && System.Console.CursorLeft <= buffer.Length + 1)
            {
                console.Cursor.Show();
                console.Cursor.MoveRight(1);
                cursorIndex++;
                continue;
            }

            var historyCount = history?.Count() ?? 0;
            if (key.Key == ConsoleKey.UpArrow && historyCount > 0 && historyIndex < historyCount)
            {
                // Erase what is there
                var bufferLength = buffer.Length;

                var amountToMoveCursorLeft = bufferLength == 0 ? 0 : cursorIndex;
                console.Cursor.MoveLeft(amountToMoveCursorLeft);
                console.Write(" ".Repeat(bufferLength));
                console.Cursor.MoveLeft(bufferLength);

                historyIndex++;
                if (historyIndex > historyCount)
                {
                    historyIndex = historyCount;
                }
                else if (historyIndex == 0)
                {
                    prehistorySaved = buffer.ToString();
                }

                var prev = history!.Reverse().Skip(historyIndex).Take(1).FirstOrDefault();
                buffer.Clear();
                if (prev != null)
                {
                    buffer.Insert(0, prev);
                    console.Write(buffer.ToString());
                    cursorIndex = buffer.Length;
                }
                else
                {
                    cursorIndex = 0;
                }

                continue;
            }

            if (key.Key == ConsoleKey.DownArrow && historyCount > 0 && historyIndex > -1)
            {
                // Erase what is there
                var bufferLength = buffer.Length;

                var amountToMoveCursorLeft = bufferLength == 0 ? 0 : cursorIndex;
                console.Cursor.MoveLeft(amountToMoveCursorLeft);
                console.Write(" ".Repeat(bufferLength));
                console.Cursor.MoveLeft(bufferLength);

                historyIndex--;
                buffer.Clear();

                if (historyIndex == -1)
                {
                    if (prehistorySaved != null)
                    {
                        buffer.Insert(0, prehistorySaved);
                    }
                }
                else
                {
                    buffer.Insert(0, history!.Reverse().Skip(historyIndex).Take(1).FirstOrDefault() ?? string.Empty);
                }

                console.Write(buffer.ToString());
                cursorIndex = buffer.Length;

                continue;
            }

            if (!char.IsControl(key.KeyChar))
            {
                if (cursorIndex == buffer.Length)
                {
                    buffer.Append(key.KeyChar);
                    cursorIndex++;
                    var output = key.KeyChar.ToString();
                    console.Write(secret ? output.Mask(mask) : output, style);
                }
                else if (insertMode)
                {
                    buffer.Insert(cursorIndex, key.KeyChar);
                    var output = key.KeyChar.ToString();
                    console.Write(secret ? output.Mask(mask) : output, style);
                    console.Write(secret ? buffer.ToString()[(cursorIndex + 1)..].Mask(mask) : buffer.ToString()[(cursorIndex + 1)..], style);
                    console.Cursor.MoveLeft(buffer.Length - cursorIndex - 1);
                    cursorIndex++;
                }
                else
                {
                    if (buffer.Length == 0)
                    {
                        buffer.Append(key.KeyChar);
                    }
                    else
                    {
                        buffer[cursorIndex] = key.KeyChar;
                    }

                    cursorIndex++;
                    var output = key.KeyChar.ToString();
                    console.Write(secret ? output.Mask(mask) : output, style);
                }
            }
        }
    }
}
