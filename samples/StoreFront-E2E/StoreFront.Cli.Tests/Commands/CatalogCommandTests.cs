//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using StoreFront.Cli.Commands;
using StoreFront.Cli.Tests.Fakes;

namespace StoreFront.Cli.Tests.Commands;

[TestClass]
public sealed class CatalogCommandTests
{
    private static CommandAppTestContext CreateContext(TestDatabase db)
    {
        var context = new CommandAppTestContext();
        TestStoreServices.Register(context, db);
        context.Configure(config => config.AddCommand<CatalogCommand>("catalog"));
        return context;
    }

    [TestMethod]
    public void Execute_SeededDatabase_ReturnsSuccess()
    {
        // Arrange
        using var db = new TestDatabase();
        var context = CreateContext(db);

        // Act
        var result = context.Run(["catalog"]);

        // Assert
        Assert.AreEqual(0, result.ExitCode);
    }

    [TestMethod]
    public void Execute_SeededDatabase_RendersProductRows()
    {
        // Arrange
        using var db = new TestDatabase();
        var context = CreateContext(db);

        // Act
        var result = context.Run(["catalog"]);

        // Assert
        Assert.Contains("COF-001", result.Output);
        Assert.Contains("House Blend Coffee", result.Output);
    }

    [TestMethod]
    public void Execute_SeededDatabase_RendersPopulatedCount()
    {
        // Arrange
        using var db = new TestDatabase();
        var context = CreateContext(db);

        // Act
        var result = context.Run(["catalog"]);

        // Assert
        Assert.Contains("8 product(s) in catalog.", result.Output);
    }

    [TestMethod]
    public void Execute_EmptyDatabase_RendersZeroCount()
    {
        // Arrange
        using var db = new TestDatabase(seed: false);
        var context = CreateContext(db);

        // Act
        var result = context.Run(["catalog"]);

        // Assert
        Assert.AreEqual(0, result.ExitCode);
        Assert.Contains("0 product(s) in catalog.", result.Output);
    }
}
