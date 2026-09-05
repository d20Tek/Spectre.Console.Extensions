//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Controls;
using Microsoft.Extensions.Options;
using Spectre.Console;
using StoreFront.Cli.Configuration;
using StoreFront.Cli.Data;
using System.Globalization;

namespace StoreFront.Cli.Presentation;

/// <summary>
/// Renders a <see cref="Transaction"/> as a formatted receipt, using the currency presenter
/// and a separator row from the extension library's table helpers.
/// </summary>
public sealed class ReceiptRenderer(IOptions<StoreOptions> options, IAnsiConsole console)
{
    private static readonly int[] ColumnWidths = [24, 6, 12, 14];
    private readonly StoreOptions _options = options.Value;
    private readonly IAnsiConsole _console = console;

    /// <summary>
    /// Writes a formatted receipt for the specified transaction to the console.
    /// </summary>
    /// <param name="transaction">The transaction to render.</param>
    public void Render(Transaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        var culture = CultureInfo.GetCultureInfo(_options.CurrencyCulture);

        _console.Write(new Rule($"[bold]{Markup.Escape(_options.Name)}[/]").LeftJustified());
        _console.MarkupLineInterpolated(
            $"[grey]Transaction #{transaction.Id} - {transaction.CreatedUtc.ToLocalTime():g}[/]");

        var table = new Table().Border(TableBorder.None).HideHeaders();
        table.AddColumn(new TableColumn("Product").Width(ColumnWidths[0]));
        table.AddColumn(new TableColumn("Qty").Width(ColumnWidths[1]).RightAligned());
        table.AddColumn(new TableColumn("Unit").Width(ColumnWidths[2]).RightAligned());
        table.AddColumn(new TableColumn("Total").Width(ColumnWidths[3]).RightAligned());

        foreach (var line in transaction.Lines)
        {
            table.AddRow(
                new Markup(Markup.Escape(line.ProductName)),
                new Markup(line.Quantity.ToString(culture)).RightJustified(),
                new Markup(RenderCurrency(line.UnitPrice, culture)).RightJustified(),
                new Markup(RenderCurrency(line.LineTotal, culture)).RightJustified());
        }

        table.AddSeparatorRow(ColumnWidths);
        AddTotalRow(table, "Subtotal", transaction.Subtotal, culture);
        AddTotalRow(table, $"Tax ({_options.TaxRate:P1})", transaction.Tax, culture);
        AddTotalRow(table, "[bold]Total[/]", transaction.Total, culture, "bold green");

        _console.Write(table);
    }

    private static void AddTotalRow(Table table, string label, decimal amount, CultureInfo culture, string? style = null)
    {
        table.AddRow(
            new Markup(label),
            Text.Empty,
            Text.Empty,
            new Markup(RenderCurrency(amount, culture, style)).RightJustified());
    }

    private static string RenderCurrency(decimal amount, CultureInfo culture, string? style = null)
    {
        var text = Markup.Escape(amount.ToString("C", culture));
        return string.IsNullOrEmpty(style) ? text : $"[{style}]{text}[/]";
    }
}
