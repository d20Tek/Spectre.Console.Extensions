//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Settings;
using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Services
{
    internal class ConsoleVerbosityWriter : IVerbosityWriter
    {
        private readonly IAnsiConsole _console;

        public VerbosityLevel Verbosity { get; set; } = VerbosityLevel.Normal;

        public ConsoleVerbosityWriter(IAnsiConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        public void MarkupDetailed(string text) =>
            this.MarkupLine(text, VerbosityLevel.Detailed);

        public void MarkupDiagnostics(string text) =>
            this.MarkupLine(text, VerbosityLevel.Diagnostic);

        public void MarkupNormal(string text) =>
            this.MarkupLine(text, VerbosityLevel.Normal);

        public void MarkupSummary(string text) =>
            this.MarkupLine(text, VerbosityLevel.Minimal);

        public void WriteDetailed(string text) =>
            this.WriteLine(text, VerbosityLevel.Detailed);

        public void WriteDiagnostics(string text) =>
            this.WriteLine(text, VerbosityLevel.Diagnostic);

        public void WriteNormal(string text) =>
            this.WriteLine(text, VerbosityLevel.Normal);

        public void WriteSummary(string text) =>
            this.WriteLine(text, VerbosityLevel.Minimal);

        private void WriteLine(string text, VerbosityLevel expectedVerbosity)
        {
            if (this.Verbosity >= expectedVerbosity)
            {
                _console.WriteLine(text);
            }
        }

        private void MarkupLine(string text, VerbosityLevel expectedVerbosity)
        {
            if (this.Verbosity >= expectedVerbosity)
            {
                _console.MarkupLine(text);
            }
        }
    }
}
