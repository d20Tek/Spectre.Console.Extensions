//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;

namespace D20Tek.Spectre.Console.Extensions;

/// <summary>
/// Provides fluent assertion entry points over <see cref="CommandAppBasicResult"/> and derived
/// result types, enabling expressions such as
/// <c>result.ShouldSucceed().AndOutputContains("done")</c>.
/// </summary>
public static class CommandAppResultAssertionExtensions
{
    /// <summary>
    /// Begins a fluent assertion chain over the specified command app result.
    /// </summary>
    /// <param name="result">The command app result to assert against.</param>
    /// <returns>A new <see cref="CommandAppResultAssertions"/> instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is null.</exception>
    public static CommandAppResultAssertions Should(this CommandAppBasicResult result) =>
        new(result);

    /// <summary>
    /// Asserts that the command app exited successfully with an exit code of zero.
    /// </summary>
    /// <param name="result">The command app result to assert against.</param>
    /// <returns>A <see cref="CommandAppResultAssertions"/> instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is null.</exception>
    /// <exception cref="CommandAppAssertionException">Thrown when the exit code is not zero.</exception>
    public static CommandAppResultAssertions ShouldSucceed(this CommandAppBasicResult result) =>
        result.Should().AndSucceed();

    /// <summary>
    /// Asserts that the command app failed with a non-zero exit code.
    /// </summary>
    /// <param name="result">The command app result to assert against.</param>
    /// <returns>A <see cref="CommandAppResultAssertions"/> instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is null.</exception>
    /// <exception cref="CommandAppAssertionException">Thrown when the exit code is zero.</exception>
    public static CommandAppResultAssertions ShouldFail(this CommandAppBasicResult result) =>
        result.Should().AndFail();

    /// <summary>
    /// Asserts that the command app returned the specified exit code.
    /// </summary>
    /// <param name="result">The command app result to assert against.</param>
    /// <param name="exitCode">The expected exit code.</param>
    /// <returns>A <see cref="CommandAppResultAssertions"/> instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is null.</exception>
    /// <exception cref="CommandAppAssertionException">Thrown when the exit code does not match.</exception>
    public static CommandAppResultAssertions ShouldReturnExitCode(
        this CommandAppBasicResult result,
        int exitCode) =>
        result.Should().AndReturnExitCode(exitCode);
}
