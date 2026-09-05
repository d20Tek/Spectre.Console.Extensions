//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StoreFront.Cli.Data;

namespace StoreFront.Cli.Tests.Fakes;

/// <summary>
/// Creates an isolated <see cref="StoreDbContext"/> backed by an open in-memory SQLite
/// connection. The connection stays open for the lifetime of this instance so the schema
/// and data persist across contexts. Dispose to release the underlying database.
/// </summary>
internal sealed class TestDatabase : IDisposable
{
    private readonly SqliteConnection _connection;

    /// <summary>
    /// Initializes a new in-memory database, creating the schema and optionally seeding it.
    /// </summary>
    /// <param name="seed">When true, the catalog seed data is applied.</param>
    public TestDatabase(bool seed = true)
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        using (var context = CreateContext())
        {
            context.Database.EnsureCreated();
            if (seed)
            {
                SeedData.EnsureSeeded(context);
            }
        }
    }

    /// <summary>
    /// Creates a new <see cref="StoreDbContext"/> bound to the shared in-memory connection.
    /// </summary>
    /// <returns>A new context instance.</returns>
    public StoreDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<StoreDbContext>()
            .UseSqlite(_connection)
            .Options;

        return new StoreDbContext(options);
    }

    /// <inheritdoc />
    public void Dispose() => _connection.Dispose();
}
