//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.Data.Sqlite;

namespace StoreFront.Cli.Tests.EndToEnd;

[TestClass]
[ExcludeFromCodeCoverage]
public sealed class StoreAppE2ETests
{
    private const string DatabasePathEnvVar = "Store__DatabasePath";
    private string _databasePath = string.Empty;

    [TestInitialize]
    public void TestInitialize()
    {
        // Point the app at an isolated SQLite file for each test through the environment
        // variable configuration provider, so runs do not interfere with each other.
        _databasePath = Path.Combine(Path.GetTempPath(), $"storefront-e2e-{Guid.NewGuid():N}.db");
        Environment.SetEnvironmentVariable(DatabasePathEnvVar, _databasePath);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        Environment.SetEnvironmentVariable(DatabasePathEnvVar, null);

        // Release pooled SQLite connections so the temporary database file can be deleted.
        SqliteConnection.ClearAllPools();
        if (File.Exists(_databasePath))
        {
            File.Delete(_databasePath);
        }
    }

    [TestMethod]
    public async Task Catalog_EndToEnd_ListsSeededProducts()
    {
        // Arrange & Act
        var result = await CommandAppE2ERunner.RunAsync(StoreApp.RunAsync, "catalog");

        // Assert
        Assert.AreEqual(0, result.ExitCode);
        Assert.Contains("COF-001", result.Output);
        Assert.Contains("8 product(s) in catalog.", result.Output);
    }

    [TestMethod]
    public async Task Checkout_ThenHistory_EndToEnd_RecordsAndListsTransaction()
    {
        // Arrange & Act
        var checkout = await CommandAppE2ERunner.RunAsync(
            StoreApp.RunAsync, "checkout --item COF-001:2 --item MUG-001:1");
        var history = await CommandAppE2ERunner.RunAsync(StoreApp.RunAsync, "history");

        // Assert
        Assert.AreEqual(0, checkout.ExitCode);
        Assert.Contains("House Blend Coffee", checkout.Output);
        Assert.AreEqual(0, history.ExitCode);
        Assert.Contains("1 transaction(s).", history.Output);
    }

    [TestMethod]
    public async Task Checkout_UnknownSku_EndToEnd_ReturnsErrorExitCode()
    {
        // Arrange & Act
        var result = await CommandAppE2ERunner.RunAsync(StoreApp.RunAsync, "checkout --item NOPE-999:1");

        // Assert
        Assert.AreEqual(1, result.ExitCode);
    }

    [TestMethod]
    public async Task Receipt_UnknownTransaction_EndToEnd_ReturnsErrorExitCode()
    {
        // Arrange & Act
        var result = await CommandAppE2ERunner.RunAsync(StoreApp.RunAsync, "receipt 999");

        // Assert
        Assert.AreEqual(1, result.ExitCode);
        Assert.Contains("No transaction found with id 999.", result.Output);
    }
}
