using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Controls;

/// <summary>
/// Extension methods for Spectre.Console Table controls.
/// </summary>
public static class TableExtensions
{
    /// <summary>
    /// Adds a separator row to the specified table. Allows the caller to also specify the 
    /// separator character and its color. Defaults to '-' char and Color.Grey.
    /// </summary>
    /// <param name="table">Table to add row to.</param>
    /// <param name="columnWidths">Array of column widths in this table.</param>
    /// <param name="style">Color style to use for separator.</param>
    /// <param name="separatorChar">Character to use for separator.</param>
    public static void AddSeparatorRow(
        this Table table,
        int[] columnWidths,
        string style = "grey",
        char separatorChar = '─')
    {
        var row = Enumerable.Range(0, columnWidths.Length)
                            .Select(i => new Markup($"[{style}]{new string(separatorChar, columnWidths[i])}[/]"))
                            .ToArray();
        table.AddRow(row);
    }
}
