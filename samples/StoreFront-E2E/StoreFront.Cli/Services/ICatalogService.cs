//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using StoreFront.Cli.Data;

namespace StoreFront.Cli.Services;

/// <summary>
/// Provides read access to the seeded product catalog.
/// </summary>
public interface ICatalogService
{
    /// <summary>
    /// Gets all products in the catalog, ordered by SKU.
    /// </summary>
    /// <returns>The list of products.</returns>
    IReadOnlyList<Product> GetProducts();

    /// <summary>
    /// Finds a product by its SKU code.
    /// </summary>
    /// <param name="sku">The SKU to search for (case-insensitive).</param>
    /// <returns>The matching product, or null when none is found.</returns>
    Product? FindBySku(string sku);
}
