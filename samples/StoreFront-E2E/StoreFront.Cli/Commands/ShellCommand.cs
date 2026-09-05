//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Commands;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using StoreFront.Cli.Configuration;

namespace StoreFront.Cli.Commands;

/// <summary>
/// The interactive StoreFront shell. Acts as the default command and lets the user run the
/// catalog, checkout, receipt, and history commands in a single resident session.
/// </summary>
internal sealed class ShellCommand(ICommandApp app, IOptions<StoreOptions> options, IAnsiConsole console)
    : InteractiveCommandBase(app, console)
{
    private readonly StoreOptions _options = options.Value;

    /// <inheritdoc />
    protected override void ShowWelcomeMessage(IAnsiConsole console)
    {
        console.Write(new FigletText(_options.Name).Centered().Color(Color.Green));
        console.MarkupLine(
            "[green]Welcome to the StoreFront shell.[/] Try [blue]catalog[/], " +
            "[blue]checkout --item COF-001:2[/], [blue]history[/], or [blue]receipt 1[/].");
        console.MarkupLine("Type [yellow]--help[/] to list commands or [yellow]exit[/] to quit.");
    }

    /// <inheritdoc />
    protected override string GetAppPromptPrefix() => "store>";

    /// <inheritdoc />
    protected override void ShowExitMessage(IAnsiConsole console) =>
        console.MarkupLine($"[green]Thanks for visiting the {_options.Name}![/]");
}
