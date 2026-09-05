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
public sealed class ReceiptServiceTests
{
    private static int SeedTransaction(TestDatabase db, params CartItem[] items)
    {
        using var context = db.CreateContext();
        var service = new CheckoutService(
            context, Options.Create(new StoreOptions()), NullLogger<CheckoutService>.Instance);
        return service.Checkout(items).Id;
    }

    [TestMethod]
    public void GetTransactions_NoTransactions_ReturnsEmptyList()
    {
        // Arrange
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var service = new ReceiptService(context);

        // Act
        var transactions = service.GetTransactions();

        // Assert
        Assert.AreEqual(0, transactions.Count);
    }

    [TestMethod]
    public void GetTransactions_WithTransactions_ReturnsWithLines()
    {
        // Arrange
        using var db = new TestDatabase();
        SeedTransaction(db, new CartItem("COF-001", 1));
        using var context = db.CreateContext();
        var service = new ReceiptService(context);

        // Act
        var transactions = service.GetTransactions();

        // Assert
        Assert.AreEqual(1, transactions.Count);
        Assert.AreEqual(1, transactions[0].Lines.Count);
    }

    [TestMethod]
    public void FindById_KnownId_ReturnsTransactionWithLines()
    {
        // Arrange
        using var db = new TestDatabase();
        var id = SeedTransaction(db, new CartItem("COF-001", 2));
        using var context = db.CreateContext();
        var service = new ReceiptService(context);

        // Act
        var transaction = service.FindById(id);

        // Assert
        Assert.IsNotNull(transaction);
        Assert.AreEqual(id, transaction.Id);
        Assert.AreEqual(1, transaction.Lines.Count);
    }

    [TestMethod]
    public void FindById_UnknownId_ReturnsNull()
    {
        // Arrange
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var service = new ReceiptService(context);

        // Act
        var transaction = service.FindById(999);

        // Assert
        Assert.IsNull(transaction);
    }
}
