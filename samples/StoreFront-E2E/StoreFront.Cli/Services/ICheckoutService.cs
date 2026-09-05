//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using StoreFront.Cli.Data;

namespace StoreFront.Cli.Services;

/// <summary>
/// A requested purchase of a product at a given quantity, used as input to a checkout.
/// </summary>
/// <param name="Sku">The product SKU to purchase.</param>
/// <param name="Quantity">The quantity to purchase.</param>
public readonly record struct CartItem(string Sku, int Quantity);

/// <summary>
/// Creates checkout transactions from a set of requested cart items.
/// </summary>
public interface ICheckoutService
{
    /// <summary>
    /// Creates and persists a transaction for the specified cart items, computing the
    /// subtotal, tax, and total. The persistence is performed within a database transaction.
    /// </summary>
    /// <param name="items">The requested cart items.</param>
    /// <returns>The persisted transaction.</returns>
    /// <exception cref="ArgumentException">When items is empty or a SKU is unknown.</exception>
    Transaction Checkout(IReadOnlyList<CartItem> items);
}
