//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console;

namespace D20Tek.Samples.Common.Services;

internal class ConsoleDisplayWriter : IDisplayWriter
{
    public VerbosityLevel Verbosity { get; set; } = VerbosityLevel.med;

    public void MarkupSummary(string text) => MarkupLine(text, VerbosityLevel.low);

    public void MarkupIntermediate(string text) => MarkupLine(text, VerbosityLevel.med);

    public void MarkupDetailed(string text) => MarkupLine(text, VerbosityLevel.high);

    public void WriteSummary(string text) => WriteLine(text, VerbosityLevel.low);

    public void WriteIntermediate(string text) =>  WriteLine(text, VerbosityLevel.med);

    public void WriteDetailed(string text) => WriteLine(text, VerbosityLevel.high);

    private void WriteLine(string text, VerbosityLevel expectedVerbosity)
    {
        if (this.Verbosity >= expectedVerbosity)
        {
            AnsiConsole.WriteLine(text);
        }
    }

    private void MarkupLine(string text, VerbosityLevel expectedVerbosity)
    {
        if (this.Verbosity >= expectedVerbosity)
        {
            AnsiConsole.MarkupLine(text);
        }
    }
}
