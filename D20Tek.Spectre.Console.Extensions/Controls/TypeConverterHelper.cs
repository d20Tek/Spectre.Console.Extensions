using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace D20Tek.Spectre.Console.Extensions.Controls;

internal static class TypeConverterHelper
{
    internal static bool IsGetConverterSupported =>
        !AppContext.TryGetSwitch("Spectre.Console.Extensions.IsGetConverterSupported", out var enabled) || enabled;

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

    public static TypeConverter GetTypeConverter<T>() => GetConverter<T>() ?? GetConverterByAttribute<T>();

    private static TypeConverter? GetConverter<T>() =>
        IntrinsicConverters.GetConverter(typeof(T)) ?? TypeDescriptor.GetConverter(typeof(T));

    private static TypeConverter GetConverterByAttribute<T>() =>
        HasTypeConverterAttribute<T>() && Activator.CreateInstance<T>() is TypeConverter converter ?
            converter :
            throw new InvalidOperationException("Couldn't find Type converter.");

    private static bool HasTypeConverterAttribute<T>() =>
        typeof(T).GetCustomAttribute<TypeConverterAttribute>() is { ConverterTypeName: var typeName } &&
            Type.GetType(typeName, false, false) is not null;
}