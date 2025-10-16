//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console;
using Spectre.Console.Cli;
using System.Threading.Tasks;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;

internal class MockAsyncCommand(IAnsiConsole console) : AsyncCommand
{
    private readonly IAnsiConsole _writer = console;

    public override Task<int> ExecuteAsync(CommandContext context)
    {
        _writer.WriteLine("Success!");
        return Task.FromResult(0);
    }
}
