using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;

internal class CustomType
{
    public string Value { get; set; } = string.Empty;
}

[ExcludeFromCodeCoverage]
internal sealed class CustomTypeConverter : TypeConverter
{
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) =>
        destinationType == typeof(string);

    public override object ConvertTo(
        ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        if (value is CustomType custom)
        {
            return custom.Value == "valid" ? "converted" : null;
        }
        return null;
    }
}

[TypeConverter(typeof(CustomTypeConverter))]
internal class ConvertibleType : CustomType { }
