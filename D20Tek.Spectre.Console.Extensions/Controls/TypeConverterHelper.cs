using System.ComponentModel;
using System.Globalization;

namespace D20Tek.Spectre.Console.Extensions.Controls;

internal static class TypeConverterHelper
{
    public static string ConvertToString<T>(T input) =>
        GetTypeConverter<T>().ConvertToInvariantString(input)
            ?? throw new InvalidOperationException("Couldn't convert input to a string.");

    public static bool TryConvertFromString<T>(string input, out T? result)
    {
        try
        {
            result = (T?)GetTypeConverter<T>().ConvertFromInvariantString(input);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }

    public static bool TryConvertFromStringWithCulture<T>(string input, CultureInfo? info, out T? result)
    {
        try
        {
            if (info is null)
            {
                return TryConvertFromString(input, out result);
            }
            else
            {
                result = (T?)GetTypeConverter<T>().ConvertFromString(null!, info, input);
            }

            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }

    public static TypeConverter GetTypeConverter<T>() =>
        IntrinsicConverters.GetConverter(typeof(T)) ?? TypeDescriptor.GetConverter(typeof(T));
}