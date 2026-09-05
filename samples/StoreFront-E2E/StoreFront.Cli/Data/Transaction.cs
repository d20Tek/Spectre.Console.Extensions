//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace StoreFront.Cli.Data;

/// <summary>
/// A completed checkout transaction, including its line items and computed totals.
/// </summary>
public sealed class Transaction
{
    /// <summary>
    /// Gets or sets the transaction identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the transaction was created.
    /// </summary>
    public DateTime CreatedUtc { get; set; }

    /// <summary>
    /// Gets or sets the sum of the line item totals before tax.
    /// </summary>
    public decimal Subtotal { get; set; }

    /// <summary>
    /// Gets or sets the tax amount applied to the subtotal.
    /// </summary>
    public decimal Tax { get; set; }

    /// <summary>
    /// Gets or sets the final amount charged, including tax.
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Gets the line items included in this transaction.
    /// </summary>
    public List<TransactionLine> Lines { get; } = [];
}
