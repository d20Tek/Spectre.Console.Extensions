//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using StoreFront.Cli.Commands;
using StoreFront.Cli.Services;
using StoreFront.Cli.Tests.Fakes;

namespace StoreFront.Cli.Tests.Commands;

[TestClass]
public sealed class ReceiptCommandTests
{
    private static CommandAppTestContext CreateContext(TestDatabase db)
    {
        var context = new CommandAppTestContext();
        TestStoreServices.Register(context, db);
        context.Configure(config => config.AddCommand<ReceiptCommand>("receipt"));
        return context;
    }

    private static int SeedTransaction(TestDatabase db)
    {
        var checkoutContext = new CommandAppTestContext();
        TestStoreServices.Register(checkoutContext, db);
        checkoutContext.Configure(config => config.AddCommand<CheckoutCommand>("checkout"));
        checkoutContext.Run(["checkout", "--item", "COF-001:1"]);

        using var context = db.CreateContext();
        return new ReceiptService(context).GetTransactions().Single().Id;
    }

    [TestMethod]
    public void Execute_KnownTransaction_ReturnsSuccess()
    {
        // Arrange
        using var db = new TestDatabase();
        var id = SeedTransaction(db);
        var context = CreateContext(db);

        // Act
        var result = context.Run(["receipt", id.ToString()]);

        // Assert
        Assert.AreEqual(0, result.ExitCode);
    }

    [TestMethod]
    public void Execute_KnownTransaction_RendersReceipt()
    {
        // Arrange
        using var db = new TestDatabase();
        var id = SeedTransaction(db);
        var context = CreateContext(db);

        // Act
        var result = context.Run(["receipt", id.ToString()]);

        // Assert
        Assert.Contains("House Blend Coffee", result.Output);
        Assert.Contains("Total", result.Output);
    }

    [TestMethod]
    public void Execute_UnknownTransaction_ReturnsErrorAndMessage()
    {
        // Arrange
        using var db = new TestDatabase();
        var context = CreateContext(db);

        // Act
        var result = context.Run(["receipt", "999"]);

        // Assert
        Assert.AreEqual(1, result.ExitCode);
        Assert.Contains("No transaction found with id 999.", result.Output);
    }
}
