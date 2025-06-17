using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace D20Tek.Spectre.Console.Extensions.Controls;

internal static class IntrinsicConverters
{
    internal const DynamicallyAccessedMemberTypes ConverterAnnotation =
        DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields;

    private delegate TypeConverter FuncWithDam([DynamicallyAccessedMembers(ConverterAnnotation)] Type type);

    private static readonly Dictionary<Type, FuncWithDam> _converters = new()
    {
        [typeof(bool)] = _ => new BooleanConverter(),
        [typeof(byte)] = _ => new ByteConverter(),
        [typeof(sbyte)] = _ => new SByteConverter(),
        [typeof(char)] = _ => new CharConverter(),
        [typeof(double)] = _ => new DoubleConverter(),
        [typeof(string)] = _ => new StringConverter(),
        [typeof(int)] = _ => new Int32Converter(),
        [typeof(short)] = _ => new Int16Converter(),
        [typeof(long)] = _ => new Int64Converter(),
        [typeof(float)] = _ => new SingleConverter(),
        [typeof(ushort)] = _ => new UInt16Converter(),
        [typeof(uint)] = _ => new UInt32Converter(),
        [typeof(ulong)] = _ => new UInt64Converter(),
        [typeof(object)] = _ => new TypeConverter(),
        [typeof(CultureInfo)] = _ => new CultureInfoConverter(),
        [typeof(DateTime)] = _ => new DateTimeConverter(),
        [typeof(DateTimeOffset)] = _ => new DateTimeOffsetConverter(),
        [typeof(decimal)] = _ => new DecimalConverter(),
        [typeof(TimeSpan)] = _ => new TimeSpanConverter(),
        [typeof(Guid)] = _ => new GuidConverter(),
        [typeof(Uri)] = _ => new UriTypeConverter(),
        [typeof(Array)] = _ => new ArrayConverter(),
        [typeof(ICollection)] = _ => new CollectionConverter(),
        [typeof(Enum)] = CreateEnumConverter(),
        [typeof(Int128)] = _ => new Int128Converter(),
        [typeof(Half)] = _ => new HalfConverter(),
        [typeof(UInt128)] = _ => new UInt128Converter(),
        [typeof(DateOnly)] = _ => new DateOnlyConverter(),
        [typeof(TimeOnly)] = _ => new TimeOnlyConverter(),
        [typeof(Version)] = _ => new VersionConverter(),
    };

    public static TypeConverter? GetConverter([DynamicallyAccessedMembers(ConverterAnnotation)] Type type)
    {
        var t = NormalizeType(type);
        return _converters.TryGetValue(type, out var factory) ? factory(type) : null;
    }

    private static Type NormalizeType(Type type) =>
        type switch
        {
            _ when type.IsArray => typeof(Array),
            _ when typeof(ICollection).IsAssignableFrom(type) => typeof(ICollection),
            _ when type.IsEnum => typeof(Enum),
            _ => type
        };

    private static FuncWithDam CreateEnumConverter() =>
        ([DynamicallyAccessedMembers(ConverterAnnotation)] type) => new EnumConverter(type);
}
