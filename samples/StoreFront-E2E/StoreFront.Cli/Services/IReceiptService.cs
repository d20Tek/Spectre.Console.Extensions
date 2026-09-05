//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using StoreFront.Cli.Data;

namespace StoreFront.Cli.Services;

/// <summary>
/// Provides access to persisted transactions for receipt printing and history listing.
/// </summary>
public interface IReceiptService
{
    /// <summary>
    /// Gets all transactions ordered by most recent first.
    /// </summary>
    /// <returns>The list of transactions.</returns>
    IReadOnlyList<Transaction> GetTransactions();

    /// <summary>
    /// Finds a transaction by its identifier, including its line items.
    /// </summary>
    /// <param name="id">The transaction identifier.</param>
    /// <returns>The matching transaction, or null when none is found.</returns>
    Transaction? FindById(int id);
}
