//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;

[ExcludeFromCodeCoverage]
internal class MockCommandWithSettings(IAnsiConsole console) : Command<MockCommandWithSettings.MockSettings>
{
    internal class MockSettings : CommandSettings
    {
        [CommandOption("-v|--value <TEXT>")]
        [Description("The verbosity level for this operation (low, med, high).")]
        [DefaultValue("default")]
        public string Value { get; set; } = string.Empty; 
    }

    private readonly IAnsiConsole _writer = console;

    public override int Execute(
        [NotNull] CommandContext context,
        [NotNull] MockSettings settings,
        CancellationToken cancellation)
    {
        _writer.WriteLine("Success!");
        return 0;
    }
}
