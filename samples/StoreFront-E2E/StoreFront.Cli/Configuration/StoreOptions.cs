//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace StoreFront.Cli.Configuration;

/// <summary>
/// Strongly typed options bound from the "Store" configuration section. These values are
/// injected into commands and services as <see cref="Microsoft.Extensions.Options.IOptions{TOptions}"/>.
/// </summary>
public sealed class StoreOptions
{
    /// <summary>
    /// The configuration section name that these options bind from.
    /// </summary>
    public const string SectionName = "Store";

    /// <summary>
    /// Gets or sets the display name of the store.
    /// </summary>
    [Required]
    public string Name { get; set; } = "StoreFront";

    /// <summary>
    /// Gets or sets the culture name used for currency formatting and parsing (for example "en-US").
    /// </summary>
    [Required]
    public string CurrencyCulture { get; set; } = "en-US";

    /// <summary>
    /// Gets or sets the tax rate applied at checkout, expressed as a fraction (for example 0.085 for 8.5%).
    /// </summary>
    [Range(0.0, 1.0)]
    public decimal TaxRate { get; set; } = 0.085m;

    /// <summary>
    /// Gets or sets the SQLite database file path used to store catalog and transaction data.
    /// </summary>
    [Required]
    public string DatabasePath { get; set; } = "storefront.db";
}
