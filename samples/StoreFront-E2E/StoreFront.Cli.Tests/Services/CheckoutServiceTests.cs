//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using StoreFront.Cli.Configuration;
using StoreFront.Cli.Services;
using StoreFront.Cli.Tests.Fakes;

namespace StoreFront.Cli.Tests.Services;

[TestClass]
public sealed class CheckoutServiceTests
{
    private static CheckoutService CreateService(Data.StoreDbContext context, decimal taxRate = 0.10m)
    {
        var options = Options.Create(new StoreOptions { TaxRate = taxRate });
        return new CheckoutService(context, options, NullLogger<CheckoutService>.Instance);
    }

    [TestMethod]
    public void Checkout_ValidCart_PersistsTransactionWithComputedTotals()
    {
        // Arrange
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var service = CreateService(context, taxRate: 0.10m);
        var cart = new List<CartItem> { new("COF-001", 2), new("MUG-001", 1) };

        // Act
        var transaction = service.Checkout(cart);

        // Assert
        var expectedSubtotal = (12.99m * 2) + 9.50m;
        Assert.AreEqual(expectedSubtotal, transaction.Subtotal);
        Assert.AreEqual(decimal.Round(expectedSubtotal * 0.10m, 2), transaction.Tax);
        Assert.AreEqual(transaction.Subtotal + transaction.Tax, transaction.Total);
        Assert.HasCount(2, transaction.Lines);
        Assert.IsGreaterThan(0, transaction.Id);
    }

    [TestMethod]
    public void Checkout_ValidCart_SavesTransactionToDatabase()
    {
        // Arrange
        using var db = new TestDatabase();
        var cart = new List<CartItem> { new("TEA-001", 3) };
        int transactionId;

        using (var context = db.CreateContext())
        {
            var service = CreateService(context);

            // Act
            transactionId = service.Checkout(cart).Id;
        }

        // Assert
        using var verifyContext = db.CreateContext();
        var persisted = verifyContext.Transactions.Find(transactionId);
        Assert.IsNotNull(persisted);
    }

    [TestMethod]
    public void Checkout_NullCart_ThrowsArgumentNullException()
    {
        // Arrange
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var service = CreateService(context);

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () => service.Checkout(null!));
    }

    [TestMethod]
    public void Checkout_EmptyCart_ThrowsArgumentException()
    {
        // Arrange
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var service = CreateService(context);

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>([ExcludeFromCodeCoverage] () => service.Checkout([]));
    }

    [TestMethod]
    public void Checkout_UnknownSku_ThrowsArgumentException()
    {
        // Arrange
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var service = CreateService(context);
        var cart = new List<CartItem> { new("NOPE-999", 1) };

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>([ExcludeFromCodeCoverage] () => service.Checkout(cart));
    }

    [TestMethod]
    public void Checkout_NonPositiveQuantity_ThrowsArgumentException()
    {
        // Arrange
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var service = CreateService(context);
        var cart = new List<CartItem> { new("COF-001", 0) };

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>([ExcludeFromCodeCoverage] () => service.Checkout(cart));
    }
}
