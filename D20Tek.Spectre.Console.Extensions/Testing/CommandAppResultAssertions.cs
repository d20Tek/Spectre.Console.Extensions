//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace D20Tek.Spectre.Console.Extensions.Testing;

/// <summary>
/// Provides a fluent, chainable set of assertions over a <see cref="CommandAppBasicResult"/>
/// (and derived types such as <see cref="CommandAppResult"/>). Each assertion returns the same
/// instance so calls can be chained, for example
/// <c>result.ShouldSucceed().AndOutputContains("done")</c>.
/// </summary>
public class CommandAppResultAssertions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandAppResultAssertions"/> class.
    /// </summary>
    /// <param name="result">The command app result to assert against.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is null.</exception>
    public CommandAppResultAssertions(CommandAppBasicResult result)
    {
        ArgumentNullException.ThrowIfNull(result);
        Result = result;
    }

    /// <summary>
    /// Gets the command app result being asserted against.
    /// </summary>
    public CommandAppBasicResult Result { get; }

    /// <summary>
    /// Asserts that the command app exited successfully with an exit code of zero.
    /// </summary>
    /// <returns>The current assertions instance for chaining.</returns>
    /// <exception cref="CommandAppAssertionException">Thrown when the exit code is not zero.</exception>
    public CommandAppResultAssertions ShouldSucceed() => AndSucceed();

    /// <summary>
    /// Asserts that the command app failed with a non-zero exit code.
    /// </summary>
    /// <returns>The current assertions instance for chaining.</returns>
    /// <exception cref="CommandAppAssertionException">Thrown when the exit code is zero.</exception>
    public CommandAppResultAssertions ShouldFail() => AndFail();

    /// <summary>
    /// Asserts that the command app returned the specified exit code.
    /// </summary>
    /// <param name="exitCode">The expected exit code.</param>
    /// <returns>The current assertions instance for chaining.</returns>
    /// <exception cref="CommandAppAssertionException">Thrown when the exit code does not match.</exception>
    public CommandAppResultAssertions ShouldReturnExitCode(int exitCode) => AndReturnExitCode(exitCode);

    /// <summary>
    /// Asserts that the command app exited successfully with an exit code of zero.
    /// </summary>
    /// <returns>The current assertions instance for chaining.</returns>
    /// <exception cref="CommandAppAssertionException">Thrown when the exit code is not zero.</exception>
    public CommandAppResultAssertions AndSucceed()
    {
        if (Result.ExitCode != 0)
        {
            Fail($"Expected the command app to succeed (exit code 0), but it returned {Result.ExitCode}.");
        }

        return this;
    }

    /// <summary>
    /// Asserts that the command app failed with a non-zero exit code.
    /// </summary>
    /// <returns>The current assertions instance for chaining.</returns>
    /// <exception cref="CommandAppAssertionException">Thrown when the exit code is zero.</exception>
    public CommandAppResultAssertions AndFail()
    {
        if (Result.ExitCode == 0)
        {
            Fail("Expected the command app to fail (non-zero exit code), but it returned 0.");
        }

        return this;
    }

    /// <summary>
    /// Asserts that the command app returned the specified exit code.
    /// </summary>
    /// <param name="exitCode">The expected exit code.</param>
    /// <returns>The current assertions instance for chaining.</returns>
    /// <exception cref="CommandAppAssertionException">Thrown when the exit code does not match.</exception>
    public CommandAppResultAssertions AndReturnExitCode(int exitCode)
    {
        if (Result.ExitCode != exitCode)
        {
            Fail($"Expected exit code {exitCode}, but the command app returned {Result.ExitCode}.");
        }

        return this;
    }

    /// <summary>
    /// Asserts that the captured output contains the specified substring.
    /// </summary>
    /// <param name="expected">The substring the output should contain.</param>
    /// <param name="comparison">The string comparison to use. Defaults to <see cref="StringComparison.Ordinal"/>.</param>
    /// <returns>The current assertions instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="expected"/> is null.</exception>
    /// <exception cref="CommandAppAssertionException">Thrown when the output does not contain the substring.</exception>
    public CommandAppResultAssertions AndOutputContains(
        string expected,
        StringComparison comparison = StringComparison.Ordinal)
    {
        ArgumentNullException.ThrowIfNull(expected);
        if (!Result.Output.Contains(expected, comparison))
        {
            Fail($"Expected output to contain \"{expected}\", but it did not.{Environment.NewLine}Output:{Environment.NewLine}{Result.Output}");
        }

        return this;
    }

    /// <summary>
    /// Asserts that the captured output does not contain the specified substring.
    /// </summary>
    /// <param name="unexpected">The substring the output should not contain.</param>
    /// <param name="comparison">The string comparison to use. Defaults to <see cref="StringComparison.Ordinal"/>.</param>
    /// <returns>The current assertions instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="unexpected"/> is null.</exception>
    /// <exception cref="CommandAppAssertionException">Thrown when the output contains the substring.</exception>
    public CommandAppResultAssertions AndOutputDoesNotContain(
        string unexpected,
        StringComparison comparison = StringComparison.Ordinal)
    {
        ArgumentNullException.ThrowIfNull(unexpected);
        if (Result.Output.Contains(unexpected, comparison))
        {
            Fail($"Expected output not to contain \"{unexpected}\", but it did.{Environment.NewLine}Output:{Environment.NewLine}{Result.Output}");
        }

        return this;
    }

    /// <summary>
    /// Asserts that the captured output matches the specified regular expression pattern.
    /// </summary>
    /// <param name="pattern">The regular expression pattern the output should match.</param>
    /// <returns>The current assertions instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="pattern"/> is null.</exception>
    /// <exception cref="CommandAppAssertionException">Thrown when the output does not match the pattern.</exception>
    public CommandAppResultAssertions AndOutputMatches(string pattern)
    {
        ArgumentNullException.ThrowIfNull(pattern);
        if (!Regex.IsMatch(Result.Output, pattern))
        {
            Fail($"Expected output to match pattern \"{pattern}\", but it did not.{Environment.NewLine}Output:{Environment.NewLine}{Result.Output}");
        }

        return this;
    }

    /// <summary>
    /// Asserts that the captured output is empty.
    /// </summary>
    /// <returns>The current assertions instance for chaining.</returns>
    /// <exception cref="CommandAppAssertionException">Thrown when the output is not empty.</exception>
    public CommandAppResultAssertions AndOutputIsEmpty()
    {
        if (Result.Output.Length != 0)
        {
            Fail($"Expected output to be empty, but it was:{Environment.NewLine}{Result.Output}");
        }

        return this;
    }

    /// <summary>
    /// Asserts that the captured output is not empty.
    /// </summary>
    /// <returns>The current assertions instance for chaining.</returns>
    /// <exception cref="CommandAppAssertionException">Thrown when the output is empty.</exception>
    public CommandAppResultAssertions AndOutputIsNotEmpty()
    {
        if (Result.Output.Length == 0)
        {
            Fail("Expected output not to be empty, but it was.");
        }

        return this;
    }

    [DoesNotReturn]
    private static void Fail(string message) => throw new CommandAppAssertionException(message);
}
