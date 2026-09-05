//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Controls;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using StoreFront.Cli.Configuration;
using StoreFront.Cli.Services;
using System.Globalization;

namespace StoreFront.Cli.Commands;

/// <summary>
/// Lists previously recorded transactions in a summary table.
/// </summary>
internal sealed class HistoryCommand(
    IReceiptService receipts,
    IOptions<StoreOptions> options,
    IAnsiConsole console)
    : Command<EmptyCommandSettings>
{
    private readonly IReceiptService _receipts = receipts;
    private readonly StoreOptions _options = options.Value;
    private readonly IAnsiConsole _console = console;

    /// <inheritdoc />
    protected override int Execute(
        CommandContext context, EmptyCommandSettings settings, CancellationToken cancellationToken)
    {
        var transactions = _receipts.GetTransactions();
        if (transactions.Count == 0)
        {
            _console.MarkupLine("[yellow]No transactions have been recorded yet.[/]");
            return 0;
        }

        var culture = CultureInfo.GetCultureInfo(_options.CurrencyCulture);

        var table = new Table().Border(TableBorder.Rounded).Expand();
        table.AddColumn("[bold]#[/]");
        table.AddColumn("[bold]Date[/]");
        table.AddColumn(new TableColumn("[bold]Items[/]").RightAligned());
        table.AddColumn(new TableColumn("[bold]Total[/]").RightAligned());

        foreach (var transaction in transactions)
        {
            table.AddRow(
                new Markup($"[blue]{transaction.Id}[/]"),
                new Markup(Markup.Escape(transaction.CreatedUtc.ToLocalTime().ToString("g", culture))),
                new Markup(transaction.Lines.Sum(l => l.Quantity).ToString(culture)).RightJustified(),
                new Markup(transaction.Total.Render("green")).RightJustified());
        }

        _console.Write(table);
        _console.MarkupLineInterpolated($"[grey]{transactions.Count} transaction(s).[/]");
        return 0;
    }
}
