//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace StoreFront.Cli.Data;

/// <summary>
/// A single line item within a <see cref="Transaction"/>, capturing the purchased
/// product, quantity, and the unit price at the time of sale.
/// </summary>
public sealed class TransactionLine
{
    /// <summary>
    /// Gets or sets the line item identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the owning transaction identifier.
    /// </summary>
    public int TransactionId { get; set; }

    /// <summary>
    /// Gets or sets the purchased product identifier.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the product name captured at the time of sale.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity purchased.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price captured at the time of sale.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets the extended price for this line (quantity multiplied by unit price).
    /// </summary>
    public decimal LineTotal => Quantity * UnitPrice;
}
