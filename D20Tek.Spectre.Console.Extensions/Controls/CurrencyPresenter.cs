namespace D20Tek.Spectre.Console.Extensions.Controls;

/// <summary>
/// Class that handles the display of currency represented by a decimal.
/// </summary>
public static class CurrencyPresenter
{
    /// <summary>
    /// Renders the currency decimal value with culture-aware currency presentation.
    /// Allows caller to customize positive and negative values with style strings.
    /// </summary>
    /// <param name="value">Currency value.</param>
    /// <param name="positiveStyle">Positive style in string format. [optional]</param>
    /// <param name="negativeStyle">Negative style in string format. [optional]</param>
    /// <returns>Markup string representation.</returns>
    public static string Render(this decimal value, string? positiveStyle = null, string? negativeStyle = null) =>
        Render(value, FormatCurrency(value), positiveStyle, negativeStyle);

    /// <summary>
    /// Renders the currency decimal value with culture-aware currency with an abbreviated presentation.
    /// Allows caller to customize positive and negative values with style strings.
    /// </summary>
    /// <param name="value">Currency value.</param>
    /// <param name="positiveStyle">Positive style in string format. [optional]</param>
    /// <param name="negativeStyle">Negative style in string format. [optional]</param>
    /// <returns>Markup string representation.</returns>
    public static string RenderAbbreviated(
        this decimal value,
        string? positiveStyle = null,
        string? negativeStyle = null) =>
        Render(value, CurrencyAbbreviator.Format(value), positiveStyle, negativeStyle);

    private static string Render(decimal value, string text, string? positiveStyle, string? negativeStyle) =>
        value >= 0 ? AppendStyleOrDefault(text, positiveStyle) : AppendStyleOrDefault(text, negativeStyle);

    private static string AppendStyleOrDefault(this string value, string? style) =>
        string.IsNullOrEmpty(style) ? value : $"[{style}]{value}[/]";

    private static string FormatCurrency(decimal value) => $"{value:C}";
}
