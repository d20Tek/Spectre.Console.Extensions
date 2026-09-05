//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace StoreFront.Cli.Data;

/// <summary>
/// Entity Framework Core context for the store front sample, backed by SQLite.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="StoreDbContext"/> class.
/// </remarks>
/// <param name="options">The context options.</param>
public sealed class StoreDbContext(DbContextOptions<StoreDbContext> options) : DbContext(options)
{

    /// <summary>
    /// Gets the product catalog set.
    /// </summary>
    public DbSet<Product> Products => Set<Product>();

    /// <summary>
    /// Gets the transactions set.
    /// </summary>
    public DbSet<Transaction> Transactions => Set<Transaction>();

    /// <summary>
    /// Gets the transaction line items set.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public DbSet<TransactionLine> TransactionLines => Set<TransactionLine>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.Sku).IsUnique();
            entity.Property(p => p.Sku).IsRequired();
            entity.Property(p => p.Name).IsRequired();
            entity.Property(p => p.UnitPrice).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Subtotal).HasColumnType("decimal(18,2)");
            entity.Property(t => t.Tax).HasColumnType("decimal(18,2)");
            entity.Property(t => t.Total).HasColumnType("decimal(18,2)");
            entity.HasMany(t => t.Lines)
                  .WithOne()
                  .HasForeignKey(l => l.TransactionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TransactionLine>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.Property(l => l.ProductName).IsRequired();
            entity.Property(l => l.UnitPrice).HasColumnType("decimal(18,2)");
            entity.Ignore(l => l.LineTotal);
        });
    }
}
