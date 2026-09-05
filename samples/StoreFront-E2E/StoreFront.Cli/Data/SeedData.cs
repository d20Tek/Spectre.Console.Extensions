//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace StoreFront.Cli.Data;

/// <summary>
/// Provides the initial product catalog seeded into the store database.
/// </summary>
public static class SeedData
{
    /// <summary>
    /// Ensures the database is created and the product catalog is seeded. Seeding is
    /// idempotent: products are only added when the catalog is empty.
    /// </summary>
    /// <param name="context">The store database context.</param>
    public static void EnsureSeeded(StoreDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.Database.EnsureCreated();

        if (context.Products.Any())
        {
            return;
        }

        context.Products.AddRange(GetCatalog());
        context.SaveChanges();
    }

    /// <summary>
    /// Gets the default product catalog used to seed the store.
    /// </summary>
    /// <returns>The list of seed products.</returns>
    public static IReadOnlyList<Product> GetCatalog() =>
    [
        new Product { Sku = "COF-001", Name = "House Blend Coffee", UnitPrice = 12.99m },
        new Product { Sku = "COF-002", Name = "Espresso Beans", UnitPrice = 15.49m },
        new Product { Sku = "TEA-001", Name = "Green Tea Tin", UnitPrice = 8.75m },
        new Product { Sku = "MUG-001", Name = "Ceramic Mug", UnitPrice = 9.50m },
        new Product { Sku = "MUG-002", Name = "Travel Tumbler", UnitPrice = 22.00m },
        new Product { Sku = "SNK-001", Name = "Dark Chocolate Bar", UnitPrice = 3.25m },
        new Product { Sku = "SNK-002", Name = "Almond Biscotti", UnitPrice = 5.60m },
        new Product { Sku = "GFT-001", Name = "Gift Card", UnitPrice = 25.00m },
    ];
}
