//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using StoreFront.Cli.Services;
using StoreFront.Cli.Tests.Fakes;

namespace StoreFront.Cli.Tests.Services;

[TestClass]
public sealed class CatalogServiceTests
{
    [TestMethod]
    public void GetProducts_SeededDatabase_ReturnsAllProductsOrderedBySku()
    {
        // Arrange
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var service = new CatalogService(context);

        // Act
        var products = service.GetProducts();

        // Assert
        Assert.AreEqual(8, products.Count);
        CollectionAssert.AreEqual(
            products.Select(p => p.Sku).ToList(),
            products.Select(p => p.Sku).OrderBy(s => s).ToList());
    }

    [TestMethod]
    public void GetProducts_EmptyDatabase_ReturnsEmptyList()
    {
        // Arrange
        using var db = new TestDatabase(seed: false);
        using var context = db.CreateContext();
        var service = new CatalogService(context);

        // Act
        var products = service.GetProducts();

        // Assert
        Assert.AreEqual(0, products.Count);
    }

    [TestMethod]
    public void FindBySku_KnownSku_ReturnsProduct()
    {
        // Arrange
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var service = new CatalogService(context);

        // Act
        var product = service.FindBySku("COF-001");

        // Assert
        Assert.IsNotNull(product);
        Assert.AreEqual("House Blend Coffee", product.Name);
    }

    [TestMethod]
    public void FindBySku_UnknownSku_ReturnsNull()
    {
        // Arrange
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var service = new CatalogService(context);

        // Act
        var product = service.FindBySku("DOES-NOT-EXIST");

        // Assert
        Assert.IsNull(product);
    }

    [TestMethod]
    public void FindBySku_NullOrWhitespaceSku_ThrowsArgumentException()
    {
        // Arrange
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var service = new CatalogService(context);

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>([ExcludeFromCodeCoverage] () => service.FindBySku(" "));
    }
}
