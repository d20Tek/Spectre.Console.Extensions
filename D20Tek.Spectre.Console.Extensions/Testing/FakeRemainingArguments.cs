//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Testing
{
    internal class FakeRemainingArguments : IRemainingArguments
    {
        private readonly Dictionary<string, List<string?>> _remaining = new Dictionary<string, List<string?>>();

        public ILookup<string, string?> Parsed => 
            _remaining.SelectMany(pair => pair.Value, (pair, value) => new { pair.Key, value })
                      .ToLookup(pair => pair.Key, pair => (string?)pair.value);

        public IReadOnlyList<string> Raw => new List<string>();
    }
}
