//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Hosting.Fakes;

internal sealed class GreetCommand(IGreetingService greetingService, IAnsiConsole console) : Command
{
    private readonly IGreetingService _greetingService = greetingService;
    private readonly IAnsiConsole _console = console;

    protected override int Execute(CommandContext context, CancellationToken cancellation)
    {
        _console.WriteLine(_greetingService.Greet("World"));
        return 0;
    }
}
