//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Spectre.Console.Extensions.Testing;

/// <summary>
/// The exception thrown when a fluent assertion over a command app result fails. Using a
/// dedicated exception keeps the assertion helpers independent of any specific test framework.
/// </summary>
public class CommandAppAssertionException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandAppAssertionException"/> class.
    /// </summary>
    public CommandAppAssertionException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandAppAssertionException"/> class with
    /// the specified error message.
    /// </summary>
    /// <param name="message">The message that describes the assertion failure.</param>
    public CommandAppAssertionException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandAppAssertionException"/> class with
    /// the specified error message and a reference to the inner exception that is the cause.
    /// </summary>
    /// <param name="message">The message that describes the assertion failure.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public CommandAppAssertionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
