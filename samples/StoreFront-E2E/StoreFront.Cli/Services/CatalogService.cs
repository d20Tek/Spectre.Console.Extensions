//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using StoreFront.Cli.Data;

namespace StoreFront.Cli.Services;

/// <summary>
/// Default <see cref="ICatalogService"/> implementation backed by <see cref="StoreDbContext"/>.
/// </summary>
public sealed class CatalogService(StoreDbContext context) : ICatalogService
{
    private readonly StoreDbContext _context = context;

    /// <inheritdoc />
    public IReadOnlyList<Product> GetProducts() =>
        _context.Products.AsNoTracking().OrderBy(p => p.Sku).ToList();

    /// <inheritdoc />
    public Product? FindBySku(string sku)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);
        return _context.Products.AsNoTracking()
                                .FirstOrDefault(p => p.Sku == sku);
    }
}
