//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions;
using D20Tek.Spectre.Console.Extensions.Injection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Spectre.Console.Cli;
using StoreFront.Cli.Commands;
using StoreFront.Cli.Configuration;
using StoreFront.Cli.Data;
using StoreFront.Cli.Presentation;
using StoreFront.Cli.Services;

namespace StoreFront.Cli;

/// <summary>
/// Wires up the store's dependency injection container, EF Core SQLite database, and CLI commands.
/// </summary>
internal sealed class Startup : StartupBase
{
    /// <inheritdoc />
    public override void ConfigureServices(ITypeRegistrar registrar)
    {
        var services = registrar.WithLifetimes().Services;

        services.AddDbContext<StoreDbContext>((provider, options) =>
        {
            var storeOptions = provider.GetRequiredService<IOptions<StoreOptions>>().Value;
            options.UseSqlite($"Data Source={storeOptions.DatabasePath}");
        });

        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<ICheckoutService, CheckoutService>();
        services.AddScoped<IReceiptService, ReceiptService>();
        services.AddSingleton<ReceiptRenderer>();
    }

    /// <inheritdoc />
    public override IConfigurator ConfigureCommands(IConfigurator config)
    {
        config.CaseSensitivity(CaseSensitivity.None);
        config.SetApplicationName("storefront");
        config.SetApplicationVersion("1.0");
        config.ValidateExamples();

        config.AddCommand<ShellCommand>("shell")
              .WithDescription("Starts the interactive StoreFront shell.")
              .WithExample(["shell"]);

        config.AddCommand<CatalogCommand>("catalog")
              .WithAlias("products")
              .WithDescription("Lists the products available in the store catalog.")
              .WithExample(["catalog"]);

        config.AddCommand<CheckoutCommand>("checkout")
              .WithDescription("Checks out one or more products and records a transaction.")
              .WithExample(["checkout", "--item", "COF-001:2", "--item", "MUG-001:1"]);

        config.AddCommand<ReceiptCommand>("receipt")
              .WithDescription("Prints the receipt for a previous transaction.")
              .WithExample(["receipt", "1"]);

        config.AddCommand<HistoryCommand>("history")
              .WithAlias("transactions")
              .WithDescription("Lists previous transactions.")
              .WithExample(["history"]);

        return config;
    }
}
