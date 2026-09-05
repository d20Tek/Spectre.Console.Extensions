//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Controls;
using Spectre.Console;
using Spectre.Console.Cli;
using StoreFront.Cli.Services;

namespace StoreFront.Cli.Commands;

/// <summary>
/// Lists the products available in the store catalog in a formatted table.
/// </summary>
internal sealed class CatalogCommand(ICatalogService catalog, IAnsiConsole console)
    : Command<CatalogCommand.Settings>
{
    private readonly ICatalogService _catalog = catalog;
    private readonly IAnsiConsole _console = console;

    /// <summary>
    /// Settings for the catalog command.
    /// </summary>
    internal sealed class Settings : CommandSettings
    {
    }

    /// <inheritdoc />
    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var products = _catalog.GetProducts();

        var table = new Table().Border(TableBorder.Rounded).Expand();
        table.AddColumn("[bold]SKU[/]");
        table.AddColumn("[bold]Product[/]");
        table.AddColumn(new TableColumn("[bold]Price[/]").RightAligned());

        foreach (var product in products)
        {
            table.AddRow(
                new Markup($"[blue]{product.Sku}[/]"),
                new Markup(Markup.Escape(product.Name)),
                new Markup(product.UnitPrice.Render("green")).RightJustified());
        }

        _console.Write(table);
        _console.MarkupLineInterpolated($"[grey]{products.Count} product(s) in catalog.[/]");
        return 0;
    }
}
