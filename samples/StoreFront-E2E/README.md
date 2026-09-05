# StoreFront-E2E

A flagship, end-to-end sample for D20Tek.Spectre.Console.Extensions. It is a small interactive
store front CLI backed by an EF Core SQLite database, built to demonstrate how the library's
extension points fit together in one runnable application.

## Projects

- `StoreFront.Cli` - the interactive store front CLI (targets `net10.0`).
- `StoreFront.Cli.Tests` - unit, command, and end-to-end tests that use the library's testing helpers.

## What it showcases

- CommandAppBuilder pipeline: `WithDIContainer`, `WithConfiguration`, `WithOptions<StoreOptions>`,
  `WithLogging`, `WithStartup<Startup>`, and `WithDefaultCommand<ShellCommand>`.
- An interactive shell (built on `InteractiveCommandBase`) as the default command, so the catalog,
  checkout, receipt, and history commands run in a single resident session.
- Dependency injection of an EF Core `StoreDbContext` and store services into commands.
- Configuration binding from `appsettings.json` into a strongly typed `StoreOptions` (store name,
  currency culture, tax rate, and database path), so users could customize the CLI for another 
  storefront just by changing configuration.
- Verbosity-aware logging injected as `ILogger<T>` into the checkout service.
- The currency presenter and table separator controls used to render catalog listings and receipts.
- Database transactions: a checkout is persisted within an explicit EF Core transaction; receipts
  and history read those persisted transactions back.

## Running

```powershell
dotnet run --project StoreFront.Cli
```

The app seeds a SQLite catalog on first run and then starts the interactive shell. Commands to try:

```text
catalog
checkout --item COF-001:2 --item MUG-001:1
history
receipt 1
```

You can also run any command directly without entering the shell, for example:

```powershell
dotnet run --project StoreFront.Cli -- catalog
```

## Database schema and updates

This sample creates its SQLite database with EF Core's `Database.EnsureCreated()` (see
`SeedData.EnsureSeeded`) rather than using EF Core migrations. This keeps the sample simple and
dependency-light, but it has an important limitation: `EnsureCreated()` only builds the schema when
the database file does not already exist, and it never alters an existing database.

As a result, if you change the entity model (for example, add a property or a new entity), those
changes will not be applied to an existing `storefront.db`. To pick up schema changes you must
delete the database file and restart the app so it is recreated from the current model:

```powershell
Remove-Item StoreFront.Cli/bin/Debug/net10.0/storefront.db -ErrorAction SilentlyContinue
dotnet run --project StoreFront.Cli
```

If you need incremental schema evolution that preserves existing data, switch from
`EnsureCreated()` to EF Core migrations (`dotnet ef migrations add ...` plus `Database.Migrate()`).
The two approaches are mutually exclusive, so a database created with `EnsureCreated()` cannot later
be managed by migrations without being recreated.

## Testing

The `StoreFront.Cli.Tests` project demonstrates three layers of testing:

- Service-level unit tests running against an isolated in-memory SQLite connection.
- Command-level tests using `CommandAppTestContext` with store services registered in the container.
- End-to-end tests using `CommandAppE2ERunner` that invoke the full builder pipeline against a
  temporary SQLite database.

Run the tests with:

```powershell
dotnet test StoreFront.Cli.Tests
```

The StoreFront.Cli.Tests project demonstrates how to run commands using CommandAppTestContext and assert output and behavior.
