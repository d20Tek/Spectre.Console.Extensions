using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Commands;

[ExcludeFromCodeCoverage]
internal class FakeCommandApp(int expectedResult = 0) : ICommandApp
{
    private readonly int _expectedResult = expectedResult;

    public void Configure(Action<IConfigurator> configuration) => throw new NotImplementedException();

    public int Run(IEnumerable<string> args) => _expectedResult;

    public Task<int> RunAsync(IEnumerable<string> args) => Task.FromResult(_expectedResult);
}
