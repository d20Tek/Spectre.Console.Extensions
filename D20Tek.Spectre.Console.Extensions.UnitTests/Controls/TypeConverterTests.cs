using D20Tek.Spectre.Console.Extensions.Controls;
using D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class TypeConverterTests
{

    [TestMethod]
    public void TryConvertFromStringWithCulture_WithConversionError_ReturnsFalse()
    {
        // arrange

        // act
        var result = TypeConverterHelper.TryConvertFromStringWithCulture<int>(
            "error", CultureInfo.CurrentCulture, out int x);

        // assert
        Assert.IsFalse(result);
        Assert.AreEqual(0, x);
    }

    [TestMethod]
    public void ConvertToString_WithInvalidConversion_ThrowsException()
    {
        // arrange
        var input = new ConvertibleType { Value = "invalid" };

        // act - assert
        Assert.Throws<InvalidOperationException>([ExcludeFromCodeCoverage]() => TypeConverterHelper.ConvertToString(input));
    }


    [TestMethod]
    public void Convert_WithBool_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString(true);

        // assert
        Assert.AreEqual("True", result);
    }

    [TestMethod]
    public void Convert_WithByte_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString<byte>(2);

        // assert
        Assert.AreEqual("2", result);
    }

    [TestMethod]
    public void Convert_WithSByte_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString<sbyte>(-2);

        // assert
        Assert.AreEqual("-2", result);
    }

    [TestMethod]
    public void Convert_WithChar_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString('x');

        // assert
        Assert.AreEqual("x", result);
    }

    [TestMethod]
    public void Convert_WithDouble_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString<double>(10);

        // assert
        Assert.AreEqual("10", result);
    }

    [TestMethod]
    public void Convert_WithShort_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString<short>(10);

        // assert
        Assert.AreEqual("10", result);
    }

    [TestMethod]
    public void Convert_WithLong_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString<long>(10);

        // assert
        Assert.AreEqual("10", result);
    }

    [TestMethod]
    public void Convert_WithFloat_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString<float>(10);

        // assert
        Assert.AreEqual("10", result);
    }

    [TestMethod]
    public void Convert_WithUShort_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString<ushort>(10);

        // assert
        Assert.AreEqual("10", result);
    }

    [TestMethod]
    public void Convert_WithUInt_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString<uint>(10);

        // assert
        Assert.AreEqual("10", result);
    }

    [TestMethod]
    public void Convert_WithULong_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString<ulong>(10);

        // assert
        Assert.AreEqual("10", result);
    }

    [TestMethod]
    public void Convert_WithObject_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString<object>(10);

        // assert
        Assert.AreEqual("10", result);
    }

    [TestMethod]
    public void Convert_WithCultureInfo_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString(CultureInfo.InvariantCulture);

        // assert
        Assert.AreEqual("(Default)", result, true);
    }

    [TestMethod]
    public void Convert_WithDateTime_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString(DateTime.MinValue);

        // assert
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void Convert_WithDateTimeOffset_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString(DateTimeOffset.MinValue);

        // assert
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void Convert_WithTimeSpan_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString(TimeSpan.Zero);

        // assert
        Assert.AreEqual("00:00:00", result);
    }

    [TestMethod]
    public void Convert_WithGuid_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString(Guid.Empty);

        // assert
        Assert.AreEqual(Guid.Empty.ToString(), result);
    }

    [TestMethod]
    public void Convert_WithUri_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString(new Uri("https://test.com"));

        // assert
        Assert.AreEqual("https://test.com", result);
    }

    [TestMethod]
    public void Convert_WithArray_ConvertsToString()
    {
        // arrange
        int[] arr = [1, 2, 3];

        // act
        var result = TypeConverterHelper.ConvertToString<Array>(arr);

        // assert
        Assert.AreEqual("Int32[] Array", result);
    }

    [TestMethod]
    public void Convert_WithCollection_ConvertsToString()
    {
        // arrange
        ICollection coll = new List<int>() { 1, 2, 3 };

        // act
        var result = TypeConverterHelper.ConvertToString(coll);

        // assert
        Assert.AreEqual("(Collection)", result);
    }

    [TestMethod]
    public void Convert_WithInt128_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString<Int128>(10);

        // assert
        Assert.AreEqual("10", result);
    }

    [TestMethod]
    public void Convert_WithUInt128_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString<UInt128>(10);

        // assert
        Assert.AreEqual("10", result);
    }

    [TestMethod]
    public void Convert_WithHalf_ConvertsToString()
    {
        // arrange
        var half = new Half();

        // act
        var result = TypeConverterHelper.ConvertToString(half);

        // assert
        Assert.AreEqual("0", result);
    }

    [TestMethod]
    public void Convert_WithDateOnly_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString(DateOnly.MinValue);

        // assert
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void Convert_WithTimeOnly_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString(TimeOnly.MinValue);

        // assert
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void Convert_WithVersion_ConvertsToString()
    {
        // arrange
        var v = new Version(1, 10);

        // act
        var result = TypeConverterHelper.ConvertToString(v);

        // assert
        Assert.AreEqual("1.10", result);
    }

    [TestMethod]
    public void Convert_WithIntArray_ConvertsToString()
    {
        // arrange
        int[] arr = [1, 2, 3];

        // act
        var result = TypeConverterHelper.ConvertToString(arr);

        // assert
        Assert.AreEqual("Int32[] Array", result);
    }

    [TestMethod]
    public void Convert_WithEnum_ConvertsToString()
    {
        // arrange

        // act
        var result = TypeConverterHelper.ConvertToString(TestType.Second);

        // assert
        Assert.AreEqual("Second", result);
    }

    internal enum TestType
    {
        None = 0,
        First,
        Second,
        Third
    }
}
