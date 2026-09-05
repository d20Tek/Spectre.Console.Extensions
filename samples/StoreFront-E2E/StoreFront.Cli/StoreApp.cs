//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions;
using D20Tek.Spectre.Console.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoreFront.Cli.Commands;
using StoreFront.Cli.Configuration;
using StoreFront.Cli.Data;

namespace StoreFront.Cli;

/// <summary>
/// Composes and runs the StoreFront CLI. Exposed as a reusable entry point so that both
/// <c>Program.cs</c> and the end-to-end tests can execute the full builder pipeline.
/// </summary>
public static class StoreApp
{
    /// <summary>
    /// Builds the StoreFront command app, seeds the database, and runs it with the given arguments.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    /// <returns>The process exit code.</returns>
    public static async Task<int> RunAsync(string[] args)
    {
        // Build the StoreFront CLI:
        //  - WithDIContainer wires up the Microsoft DI container.
        //  - WithConfiguration/WithOptions bind appsettings.json "Store" section to StoreOptions.
        //  - WithLogging enables verbosity-aware logging rendered through Spectre.Console.
        //  - WithStartup registers EF Core SQLite, the store services, and CLI commands.
        //  - WithDefaultCommand makes the interactive shell the default experience.
        var builder = new CommandAppBuilder()
            .WithDIContainer()
            .WithConfiguration()
            .WithOptions<StoreOptions>(StoreOptions.SectionName)
            .WithLogging()
            .WithStartup<Startup>()
            .WithDefaultCommand<ShellCommand>()
            .Build();

        // Ensure the SQLite database exists and the catalog is seeded before running any command.
        using (var scope = builder.GetServiceCollection().BuildServiceProvider().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            SeedData.EnsureSeeded(context);
        }

        return await builder.RunAsync(args);
    }
}
