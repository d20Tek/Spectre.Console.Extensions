//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using StoreFront.Cli.Commands;
using StoreFront.Cli.Tests.Fakes;

namespace StoreFront.Cli.Tests.Commands;

[TestClass]
public sealed class HistoryCommandTests
{
    private static CommandAppTestContext CreateContext(TestDatabase db)
    {
        var context = new CommandAppTestContext();
        TestStoreServices.Register(context, db);
        context.Configure(config => config.AddCommand<HistoryCommand>("history"));
        return context;
    }

    private static void SeedTransaction(TestDatabase db, string item)
    {
        var checkoutContext = new CommandAppTestContext();
        TestStoreServices.Register(checkoutContext, db);
        checkoutContext.Configure(config => config.AddCommand<CheckoutCommand>("checkout"));
        checkoutContext.Run(["checkout", "--item", item]);
    }

    [TestMethod]
    public void Execute_NoTransactions_ReturnsSuccessWithEmptyMessage()
    {
        // Arrange
        using var db = new TestDatabase();
        var context = CreateContext(db);

        // Act
        var result = context.Run(["history"]);

        // Assert
        Assert.AreEqual(0, result.ExitCode);
        Assert.Contains("No transactions have been recorded yet.", result.Output);
    }

    [TestMethod]
    public void Execute_SingleTransaction_RendersRowAndCount()
    {
        // Arrange
        using var db = new TestDatabase();
        SeedTransaction(db, "COF-001:2");
        var context = CreateContext(db);

        // Act
        var result = context.Run(["history"]);

        // Assert
        Assert.AreEqual(0, result.ExitCode);
        Assert.Contains("1 transaction(s).", result.Output);
    }

    [TestMethod]
    public void Execute_MultipleTransactions_RendersAllInCount()
    {
        // Arrange
        using var db = new TestDatabase();
        SeedTransaction(db, "COF-001:1");
        SeedTransaction(db, "MUG-001:2");
        var context = CreateContext(db);

        // Act
        var result = context.Run(["history"]);

        // Assert
        Assert.AreEqual(0, result.ExitCode);
        Assert.Contains("2 transaction(s).", result.Output);
    }
}
