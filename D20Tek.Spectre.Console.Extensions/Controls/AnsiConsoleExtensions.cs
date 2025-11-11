using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Controls;

/// <summary>
/// Extension methods for the IAnsiConsole.
/// </summary>
public static class AnsiConsoleExtensions
{
    /// <summary>
    /// Writes a list of messages with the MarkupLine command.
    /// </summary>
    /// <param name="console">IAnsiConsole being extended.</param>
    /// <param name="messages">Variable arguments list of comma-separated messages.</param>
    public static void WriteMessages(this IAnsiConsole console, params string[] messages) =>
        console.MarkupLine(string.Join(Environment.NewLine, messages));

    /// <summary>
    /// If the condition is true, writes a list of messages with the MarkupLine command.
    /// </summary>
    /// <param name="console">IAnsiConsole being extended.</param>
    /// <param name="condition">Boolean condition to check.</param>
    /// <param name="messages">Variable arguments list of comma-separated messages.</param>
    public static void WriteMessagesConditional(this IAnsiConsole console, bool condition, params string[] messages)
    {
        if (condition) console.WriteMessages(messages);
    }
}
