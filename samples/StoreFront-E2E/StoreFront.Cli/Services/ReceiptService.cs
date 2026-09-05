//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using StoreFront.Cli.Data;

namespace StoreFront.Cli.Services;

/// <summary>
/// Default <see cref="IReceiptService"/> implementation backed by <see cref="StoreDbContext"/>.
/// </summary>
public sealed class ReceiptService(StoreDbContext context) : IReceiptService
{
    private readonly StoreDbContext _context = context;

    /// <inheritdoc />
    public IReadOnlyList<Transaction> GetTransactions() =>
        _context.Transactions.AsNoTracking()
                             .Include(t => t.Lines)
                             .OrderByDescending(t => t.CreatedUtc)
                             .ThenByDescending(t => t.Id)
                             .ToList();

    /// <inheritdoc />
    public Transaction? FindById(int id) =>
        _context.Transactions.AsNoTracking()
                             .Include(t => t.Lines)
                             .FirstOrDefault(t => t.Id == id);
}
