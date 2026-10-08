using System.Globalization;

namespace D20Tek.Spectre.Console.Extensions.Controls;

internal static class CurrencyAbbreviator
{
    internal static string Format(decimal value, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentCulture;
        var format = culture.NumberFormat;
        string currencySymbol = format.CurrencySymbol;

        string text = ValueToAbbr(Math.Abs(value));

        return value >= 0 ?
            FormatPositiveCurrency(format, currencySymbol, text) :
            FormatNegativeCurrency(format, currencySymbol, text);
    }

    private static string ValueToAbbr(decimal value) =>
        value switch
        {
            > 1_000_000_000M => $"{(value / 1_000_000_000M):0.##}B",
            > 1_000_000M => $"{(value / 1_000_000M):0.##}M",
            > 1000M => $"{(value / 1000M):0.##}K",
            _ => $"{value:0.##}"
        };

    private static string FormatPositiveCurrency(NumberFormatInfo format, string currencySymbol, string number) =>
        format.CurrencyPositivePattern switch
        {
            1 => $"{number}{currencySymbol}",     // n$
            2 => $"{currencySymbol} {number}",    // $ n
            3 => $"{number} {currencySymbol}",    // n $
            _ => $"{currencySymbol}{number}",     // $n & fallback
        };

    private static string FormatNegativeCurrency(NumberFormatInfo format, string currencySymbol, string number) =>
        format.CurrencyNegativePattern switch
        {
            0 => $"({currencySymbol}{number})",       // ($n)
            2 => $"{currencySymbol}-{number}",        // $-n
            3 => $"{currencySymbol}{number}-",        // $n-
            4 => $"({number}{currencySymbol})",       // (n$)
            5 => $"-{number}{currencySymbol}",        // -n$
            6 => $"{number}-{currencySymbol}",        // n-$
            7 => $"{number}{currencySymbol}-",        // n$-
            8 => $"-{number} {currencySymbol}",       // -n $
            9 => $"-{currencySymbol} {number}",       // -$ n
            10 => $"{number} {currencySymbol}-",      // n $-
            11 => $"{currencySymbol} {number}-",      // $ n-
            12 => $"({currencySymbol} {number})",     // ($ n)
            13 => $"({number} {currencySymbol})",     // (n $)
            14 => $"({currencySymbol}{number})",      // ($n) alt
            15 => $"({number}{currencySymbol})",      // (n$) alt
            _ => $"-{currencySymbol}{number}",        // -$n & fallback
        };
}
