//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Injection;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StoreFront.Cli.Configuration;
using StoreFront.Cli.Presentation;
using StoreFront.Cli.Services;

namespace StoreFront.Cli.Tests.Fakes;

/// <summary>
/// Helper for registering the StoreFront services into a <see cref="CommandAppTestContext"/> so
/// that command tests resolve real services backed by an in-memory SQLite database.
/// </summary>
internal static class TestStoreServices
{
    /// <summary>
    /// Registers logging, the store database, options, services, and the receipt renderer into
    /// the test context's registrar.
    /// </summary>
    /// <param name="context">The command app test context.</param>
    /// <param name="db">The in-memory test database whose contexts are injected.</param>
    /// <param name="options">Optional store options; defaults are used when null.</param>
    public static void Register(CommandAppTestContext context, TestDatabase db, StoreOptions? options = null)
    {
        var services = context.Registrar.WithLifetimes().Services;
        services.AddLogging();
        services.AddScoped(_ => db.CreateContext());
        services.AddSingleton<IOptions<StoreOptions>>(Options.Create(options ?? new StoreOptions()));
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<ICheckoutService, CheckoutService>();
        services.AddScoped<IReceiptService, ReceiptService>();
        services.AddSingleton<ReceiptRenderer>();
    }
}
