//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console;
using Spectre.Console.Cli;
using StoreFront.Cli.Presentation;
using StoreFront.Cli.Services;
using System.ComponentModel;

namespace StoreFront.Cli.Commands;

/// <summary>
/// Prints the receipt for a previously recorded transaction.
/// </summary>
internal sealed class ReceiptCommand(
    IReceiptService receipts,
    ReceiptRenderer renderer,
    IAnsiConsole console)
    : Command<ReceiptCommand.Settings>
{
    private readonly IReceiptService _receipts = receipts;
    private readonly ReceiptRenderer _renderer = renderer;
    private readonly IAnsiConsole _console = console;

    /// <summary>
    /// Settings for the receipt command.
    /// </summary>
    internal sealed class Settings : CommandSettings
    {
        /// <summary>
        /// Gets or sets the transaction identifier to print.
        /// </summary>
        [CommandArgument(0, "<TRANSACTION_ID>")]
        [Description("The identifier of the transaction to print.")]
        public int TransactionId { get; set; }
    }

    /// <inheritdoc />
    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var transaction = _receipts.FindById(settings.TransactionId);
        if (transaction is null)
        {
            _console.MarkupLineInterpolated($"[red]No transaction found with id {settings.TransactionId}.[/]");
            return 1;
        }

        _renderer.Render(transaction);
        return 0;
    }
}
