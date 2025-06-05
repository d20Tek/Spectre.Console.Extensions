using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.Testing;

/// <summary>
/// Empty version of IRemainingArguments that is useful for unit testing commands.
/// </summary>
public class NullRemainingArguments : IRemainingArguments
{
    private readonly List<string> _raw = [];
    private readonly ILookup<string, string?> _parsed;

    /// <summary>
    /// Default empty constructor.
    /// </summary>
    public NullRemainingArguments()
    {
        _parsed = new List<KeyValuePair<string, string?>>().ToLookup(
            [ExcludeFromCodeCoverage] (kvp) => kvp.Key,
            [ExcludeFromCodeCoverage] (kvp) => kvp.Value);
    }

    /// <inheritdoc/>
    public ILookup<string, string?> Parsed => _parsed;

    /// <inheritdoc/>
    public IReadOnlyList<string> Raw => _raw.AsReadOnly();

    /// <summary>
    /// Gets the default instance of NullRemainingArguments.
    /// </summary>
    public static NullRemainingArguments Instance => new();
}
