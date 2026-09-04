# API Reference: Configuration

Package: `D20Tek.Spectre.Console.Extensions.Configuration`
Namespace: `D20Tek.Spectre.Console.Extensions.Configuration`

This document covers the extensions that add `Microsoft.Extensions.Configuration` and Options binding to a `CommandAppBuilder`. Both require a DI container, so call `WithDIContainer` first.

## Contents

- [ConfigurationCommandAppBuilderExtensions](#configurationcommandappbuilderextensions)

## ConfigurationCommandAppBuilderExtensions

| Member | Signature | Description |
|---|---|---|
| `WithConfiguration` | `static CommandAppBuilder WithConfiguration(this CommandAppBuilder builder, Action<IConfigurationBuilder>? configure = null)` | Builds an `IConfiguration` and registers it in the container. By default reads from an optional `appsettings.json` and environment variables; the delegate can add or replace sources. Throws `ArgumentNullException` when builder is null and `InvalidOperationException` when no DI container is configured. |
| `WithOptions<TOptions>` | `static CommandAppBuilder WithOptions<TOptions>(this CommandAppBuilder builder, string sectionName)` where `TOptions : class` | Binds a configuration section to a strongly typed options class registered as `IOptions<TOptions>`, validating data annotations. Throws `ArgumentNullException` when builder is null, `ArgumentException` when the section name is null or whitespace, and `InvalidOperationException` when no DI container is configured. |

## Related

- [Guide: Configuration and Options Binding](guide-configuration.md)
- [API Reference hub](api-reference.md)
