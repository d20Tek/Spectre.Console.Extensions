//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using StoreFront.Cli.Commands;
using StoreFront.Cli.Tests.Fakes;

namespace StoreFront.Cli.Tests.Commands;

[TestClass]
public sealed class CheckoutCommandTests
{
    private static CommandAppTestContext CreateContext(TestDatabase db)
    {
        var context = new CommandAppTestContext();
        TestStoreServices.Register(context, db);
        context.Configure(config => config.AddCommand<CheckoutCommand>("checkout"));
        return context;
    }

    [TestMethod]
    public void Execute_ValidSingleItem_ReturnsSuccess()
    {
        // Arrange
        using var db = new TestDatabase();
        var context = CreateContext(db);

        // Act
        var result = context.Run(["checkout", "--item", "COF-001:2"]);

        // Assert
        result.ShouldSucceed();
    }

    [TestMethod]
    public void Execute_ValidMultipleItems_RendersReceipt()
    {
        // Arrange
        using var db = new TestDatabase();
        var context = CreateContext(db);

        // Act
        var result = context.Run(["checkout", "--item", "COF-001:2", "--item", "MUG-001:1"]);

        // Assert
        result.ShouldSucceed()
              .AndOutputContains("House Blend Coffee")
              .AndOutputContains("Ceramic Mug")
              .AndOutputContains("Total");
    }

    [TestMethod]
    public void Execute_ItemWithWhitespaceTrimmed_ReturnsSuccess()
    {
        // Arrange
        using var db = new TestDatabase();
        var context = CreateContext(db);

        // Act
        var result = context.Run(["checkout", "--item", "COF-001 : 3"]);

        // Assert
        result.ShouldSucceed();
    }

    [TestMethod]
    public void Execute_ItemMissingQuantity_ReturnsErrorAndMessage()
    {
        // Arrange
        using var db = new TestDatabase();
        var context = CreateContext(db);

        // Act
        var result = context.Run(["checkout", "--item", "COF-001"]);

        // Assert
        result.ShouldReturnExitCode(1).AndOutputContains("Invalid item 'COF-001'.");
    }

    [TestMethod]
    public void Execute_ItemWithExtraSegments_ReturnsError()
    {
        // Arrange
        using var db = new TestDatabase();
        var context = CreateContext(db);

        // Act
        var result = context.Run(["checkout", "--item", "COF-001:2:extra"]);

        // Assert
        result.ShouldReturnExitCode(1).AndOutputContains("Invalid item");
    }

    [TestMethod]
    public void Execute_ItemWithNonNumericQuantity_ReturnsError()
    {
        // Arrange
        using var db = new TestDatabase();
        var context = CreateContext(db);

        // Act
        var result = context.Run(["checkout", "--item", "COF-001:abc"]);

        // Assert
        result.ShouldReturnExitCode(1).AndOutputContains("Invalid item 'COF-001:abc'.");
    }

    [TestMethod]
    public void Execute_UnknownSku_ReturnsErrorFromService()
    {
        // Arrange
        using var db = new TestDatabase();
        var context = CreateContext(db);

        // Act
        var result = context.Run(["checkout", "--item", "NOPE-999:1"]);

        // Assert
        result.ShouldReturnExitCode(1).AndOutputContains("Unknown product SKU 'NOPE-999'.");
    }

    [TestMethod]
    public void Execute_NonPositiveQuantity_ReturnsErrorFromService()
    {
        // Arrange
        using var db = new TestDatabase();
        var context = CreateContext(db);

        // Act
        var result = context.Run(["checkout", "--item", "COF-001:0"]);

        // Assert
        result.ShouldReturnExitCode(1).AndOutputContains("must be greater than zero");
    }

    [TestMethod]
    public void Execute_NoItems_ReturnsValidationError()
    {
        // Arrange
        using var db = new TestDatabase();
        var context = CreateContext(db);

        // Act
        var result = context.Run(["checkout"]);

        // Assert
        result.ShouldFail().AndOutputContains("At least one --item is required.");
    }

    [TestMethod]
    public void Validate_WithItems_ReturnsSuccess()
    {
        // Arrange
        var settings = new CheckoutCommand.Settings { Items = ["COF-001:1"] };

        // Act
        var result = settings.Validate();

        // Assert
        Assert.IsTrue(result.Successful);
    }

    [TestMethod]
    public void Validate_WithoutItems_ReturnsError()
    {
        // Arrange
        var settings = new CheckoutCommand.Settings { Items = [] };

        // Act
        var result = settings.Validate();

        // Assert
        Assert.IsFalse(result.Successful);
        Assert.AreEqual("At least one --item is required.", result.Message);
    }
}
