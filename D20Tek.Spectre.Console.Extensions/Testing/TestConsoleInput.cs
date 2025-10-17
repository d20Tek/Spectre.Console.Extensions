//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Testing;

/// <summary>
/// Console input for testing CommandApps.
/// </summary>
public class TestConsoleInput : IAnsiConsoleInput
{
    private readonly Queue<ConsoleKeyInfo> _input = new();

    /// <summary>
    /// Pushes the specified text to the input queue.
    /// </summary>
    /// <param name="input">The input string.</param>
    public void PushText(string input)
    {
        ArgumentNullException.ThrowIfNull(input);
        foreach (var character in input)
        {
            PushCharacter(character);
        }
    }

    /// <summary>
    /// Pushes the specified text followed by 'Enter' to the input queue.
    /// </summary>
    /// <param name="input">The input.</param>
    public void PushTextWithEnter(string input)
    {
        PushText(input);
        PushKey(ConsoleKey.Enter);
    }

    /// <summary>
    /// Pushes the specified character to the input queue.
    /// </summary>
    /// <param name="input">The input.</param>
    public void PushCharacter(char input) => 
        _input.Enqueue(new ConsoleKeyInfo(input, (ConsoleKey)input, false, false, char.IsUpper(input)));

    /// <summary>
    /// Pushes the specified key to the input queue.
    /// </summary>
    /// <param name="input">The input.</param>
    public void PushKey(ConsoleKey input) =>
        _input.Enqueue(new ConsoleKeyInfo((char)input, input, false, false, false));

    /// <summary>
    /// Pushes the specified key to the input queue.
    /// </summary>
    /// <param name="input">The input.</param>
    public void PushKey(ConsoleKeyInfo input) => _input.Enqueue(input);

    /// <inheritdoc/>
    public bool IsKeyAvailable() => _input.Count > 0;

    /// <inheritdoc/>
    public ConsoleKeyInfo? ReadKey(bool intercept)
    {
        if (_input.Count == 0)
        {
            throw new InvalidOperationException("No input available.");
        }

        return _input.Dequeue();
    }

    /// <inheritdoc/>
    public Task<ConsoleKeyInfo?> ReadKeyAsync(bool intercept, CancellationToken cancellationToken) =>
        Task.FromResult(ReadKey(intercept));
}
