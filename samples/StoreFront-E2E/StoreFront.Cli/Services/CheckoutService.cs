//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoreFront.Cli.Configuration;
using StoreFront.Cli.Data;

namespace StoreFront.Cli.Services;

/// <summary>
/// Default <see cref="ICheckoutService"/> implementation that persists a transaction to the
/// store database within an explicit database transaction, applying the configured tax rate.
/// </summary>
public sealed class CheckoutService(StoreDbContext context, IOptions<StoreOptions> options, ILogger<CheckoutService> logger)
    : ICheckoutService
{
    private readonly StoreDbContext _context = context;
    private readonly StoreOptions _options = options.Value;
    private readonly ILogger<CheckoutService> _logger = logger;

    /// <inheritdoc />
    public Transaction Checkout(IReadOnlyList<CartItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        if (items.Count == 0)
        {
            throw new ArgumentException("Cannot checkout an empty cart.", nameof(items));
        }

        _logger.LogDebug("Starting checkout for {LineCount} line item(s).", items.Count);

        var transaction = BuildTransaction(items);

        using var dbTransaction = _context.Database.BeginTransaction();
        _context.Transactions.Add(transaction);
        _context.SaveChanges();
        dbTransaction.Commit();

        _logger.LogInformation(
            "Checkout complete. Transaction #{TransactionId} total {Total}.",
            transaction.Id,
            transaction.Total);

        return transaction;
    }

    private Transaction BuildTransaction(IReadOnlyList<CartItem> items)
    {
        var transaction = new Transaction { CreatedUtc = DateTime.UtcNow };

        foreach (var item in items)
        {
            if (item.Quantity <= 0)
            {
                throw new ArgumentException($"Quantity for SKU '{item.Sku}' must be greater than zero.", nameof(items));
            }

            var product = _context.Products.FirstOrDefault(p => p.Sku == item.Sku)
                ?? throw new ArgumentException($"Unknown product SKU '{item.Sku}'.", nameof(items));

            transaction.Lines.Add(new TransactionLine
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = item.Quantity,
                UnitPrice = product.UnitPrice,
            });
        }

        transaction.Subtotal = transaction.Lines.Sum(l => l.LineTotal);
        transaction.Tax = decimal.Round(transaction.Subtotal * _options.TaxRate, 2);
        transaction.Total = transaction.Subtotal + transaction.Tax;

        return transaction;
    }
}
