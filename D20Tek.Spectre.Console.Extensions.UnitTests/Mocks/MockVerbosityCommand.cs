//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Services;
using D20Tek.Spectre.Console.Extensions.Settings;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;

internal class MockVerbosityCommand(IVerbosityWriter console) : Command<VerbositySettings>
{
    private readonly IVerbosityWriter _writer = console;

    public override int Execute(CommandContext context, VerbositySettings settings)
    {
        _writer.Verbosity = settings.Verbosity;

        _writer.WriteSummary("Minimal");
        _writer.WriteNormal("Normal");
        _writer.WriteDetailed("Detailed");
        _writer.WriteDiagnostics("Diagnostics");
        return 0;
    }
}
