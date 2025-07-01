using D20Tek.Spectre.Console.Extensions.Controls;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console;
using System;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class TableExtensionsTests
{

    [TestMethod]
    public void AddSeparatorRow_WithDefaultStyling_UpdatesTable()
    {
        // arrange
        var console = new TestConsole();
        int[] columns = [10, 10, 5];
        var table = new Table().AddColumns(
            new TableColumn("Col1").Centered().Width(10),
            new TableColumn("Col2").Width(10),
            new TableColumn("Col3").RightAligned().Width(5));

        // act
        table.AddSeparatorRow(columns);
        console.Write(table);

        // assert
        Assert.AreEqual(1, table.Rows.Count);
        Assert.AreEqual(3, table.Columns.Count);
        StringAssert.Contains(console.Output, "──────────");
    }

    [TestMethod]
    public void AddSeparatorRow_WithDifferentTableAndRowColumns_ThrowsException()
    {
        // arrange
        int[] columns = [10, 10, 5];
        var table = new Table();

        // act - assert
        Assert.Throws<InvalidOperationException>([ExcludeFromCodeCoverage]() => table.AddSeparatorRow(columns));
    }

    [TestMethod]
    public void AddSeparatorRow_WithSpecifiedStyling_UpdatesTable()
    {
        // arrange
        var console = new TestConsole();
        int[] columns = [10, 10, 5];
        var table = new Table().AddColumns(
            new TableColumn("Col1").Centered().Width(10),
            new TableColumn("Col2").Width(10),
            new TableColumn("Col3").RightAligned().Width(5));

        // act
        table.AddSeparatorRow(columns, "white", '=');
        console.Write(table);

        // assert
        Assert.AreEqual(1, table.Rows.Count);
        Assert.AreEqual(3, table.Columns.Count);
        StringAssert.Contains(console.Output, "==========");
    }
}
