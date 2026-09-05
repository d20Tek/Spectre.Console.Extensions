//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace StoreFront.Cli.Tests.Fakes;

/// <summary>
/// A fake <see cref="ICommandApp"/> that records the arguments it was run with and returns a
/// configured exit code. Used to drive <c>ShellCommand</c> without a real command pipeline.
/// </summary>
[ExcludeFromCodeCoverage]
internal sealed class FakeCommandApp(int expectedResult = 0) : ICommandApp
{
    private readonly int _expectedResult = expectedResult;

    /// <summary>
    /// Gets the list of argument sets the app was run with, in order.
    /// </summary>
    public List<string[]> Invocations { get; } = [];

    /// <inheritdoc />
    public void Configure(Action<IConfigurator> configuration)
    {
    }

    /// <inheritdoc />
    public int Run(IEnumerable<string> args, CancellationToken cancellation)
    {
        Invocations.Add(args.ToArray());
        return _expectedResult;
    }

    /// <inheritdoc />
    public Task<int> RunAsync(IEnumerable<string> args, CancellationToken cancellation)
    {
        Invocations.Add(args.ToArray());
        return Task.FromResult(_expectedResult);
    }
}
