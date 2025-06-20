using System.ComponentModel;
using System.Globalization;

namespace D20Tek.Spectre.Console.Extensions.Controls;

internal static class TypeConverterHelper
{
    public static string ConvertToString<T>(T input) =>
        GetTypeConverter<T>().ConvertToInvariantString(input)
            ?? throw new InvalidOperationException("Couldn't convert input to a string.");

    public static bool TryConvertFromString<T>(string input, out T? result) =>
        TryConvertFrom(input, null, out result);

    public static bool TryConvertFromStringWithCulture<T>(string input, CultureInfo? info, out T? result) =>
        TryConvertFrom(input, info, out result);

    public static TypeConverter GetTypeConverter<T>() =>
        IntrinsicConverters.GetConverter(typeof(T)) ?? TypeDescriptor.GetConverter(typeof(T));

    private static bool TryConvertFrom<T>(string input, CultureInfo? culture, out T? result)
    {
        try
        {
            var converter = GetTypeConverter<T>();

            result = culture is null
                ? (T?)converter.ConvertFromInvariantString(input)
                : (T?)converter.ConvertFromString(null!, culture, input);

            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }
}