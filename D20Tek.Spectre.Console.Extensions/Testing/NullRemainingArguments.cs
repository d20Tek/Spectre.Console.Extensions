using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.Testing;

internal class NullRemainingArguments : IRemainingArguments
{
    private readonly List<string> _raw = [];
    private readonly ILookup<string, string?> _parsed;

    public NullRemainingArguments()
    {
        _parsed = new List<KeyValuePair<string, string?>>().ToLookup(
            [ExcludeFromCodeCoverage] (kvp) => kvp.Key,
            [ExcludeFromCodeCoverage] (kvp) => kvp.Value);
    }

    public ILookup<string, string?> Parsed => _parsed;

    public IReadOnlyList<string> Raw => _raw.AsReadOnly();

    public static NullRemainingArguments Instance => new();
}
