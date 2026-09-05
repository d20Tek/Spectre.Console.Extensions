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
/// Checks out one or more products and records the resulting transaction, then prints its receipt.
/// </summary>
internal sealed class CheckoutCommand(
    ICheckoutService checkout,
    ReceiptRenderer renderer,
    IAnsiConsole console)
    : Command<CheckoutCommand.Settings>
{
    private readonly ICheckoutService _checkout = checkout;
    private readonly ReceiptRenderer _renderer = renderer;
    private readonly IAnsiConsole _console = console;

    /// <summary>
    /// Settings for the checkout command.
    /// </summary>
    internal sealed class Settings : CommandSettings
    {
        /// <summary>
        /// Gets or sets the cart items in "SKU:quantity" format (repeatable).
        /// </summary>
        [CommandOption("-i|--item <ITEM>")]
        [Description("A cart item in SKU:quantity format (for example COF-001:2). Repeatable.")]
        public string[] Items { get; set; } = [];

        /// <inheritdoc />
        public override ValidationResult Validate() =>
            Items.Length == 0
                ? ValidationResult.Error("At least one --item is required.")
                : ValidationResult.Success();
    }

    /// <inheritdoc />
    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var cart = new List<CartItem>();
        foreach (var raw in settings.Items)
        {
            var parts = raw.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2 || !int.TryParse(parts[1], out var quantity))
            {
                _console.MarkupLineInterpolated(
                    $"[red]Invalid item '{raw}'. Use SKU:quantity format (for example COF-001:2).[/]");
                return 1;
            }

            cart.Add(new CartItem(parts[0], quantity));
        }

        try
        {
            var transaction = _checkout.Checkout(cart);
            _renderer.Render(transaction);
            return 0;
        }
        catch (ArgumentException ex)
        {
            _console.MarkupLineInterpolated($"[red]{ex.Message}[/]");
            return 1;
        }
    }
}
