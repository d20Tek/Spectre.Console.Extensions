using System.Globalization;

namespace D20Tek.Spectre.Console.Extensions.Controls;

internal static class CurrencyAbbreviator
{
    private const decimal _thousand = 1000M;
    private const decimal _million = 1_000_000M;
    private const decimal _billion = 1_000_000_000M;

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
            > _billion => $"{(value / _billion):0.##}B",
            > _million => $"{(value / _million):0.##}M",
            > _thousand => $"{(value / _thousand):0.##}K",
            _ => $"{value:0.##}"
        };

    private static string FormatPositiveCurrency(NumberFormatInfo format, string currencySymbol, string number) =>
        format.CurrencyPositivePattern switch
        {
            0 => $"{currencySymbol}{number}",     // $n
            1 => $"{number}{currencySymbol}",     // n$
            2 => $"{currencySymbol} {number}",    // $ n
            3 => $"{number} {currencySymbol}",    // n $
            _ => $"{currencySymbol}{number}",     // fallback
        };

    private static string FormatNegativeCurrency(NumberFormatInfo format, string currencySymbol, string number) =>
        format.CurrencyNegativePattern switch
        {
            0 => $"({currencySymbol}{number})",       // ($n)
            1 => $"-{currencySymbol}{number}",        // -$n
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
            _ => $"-{currencySymbol}{number}",        // fallback
        };
}
