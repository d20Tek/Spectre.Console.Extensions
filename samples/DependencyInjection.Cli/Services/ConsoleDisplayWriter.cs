//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------

using DependencyInjection.Cli.Commands;
using Spectre.Console;

namespace DependencyInjection.Cli.Services
{
    internal class ConsoleDisplayWriter : IDisplayWriter
    {
        public VerbosityLevel Verbosity { get; set; } = VerbosityLevel.med;

        public void MarkupSummary(string text) =>
            this.MarkupLine(text, VerbosityLevel.low);

        public void MarkupIntermediate(string text) =>
            this.MarkupLine(text, VerbosityLevel.med);

        public void MarkupDetailed(string text) =>
            this.MarkupLine(text, VerbosityLevel.high);

        public void WriteSummary(string text) =>
            this.WriteLine(text, VerbosityLevel.low);

        public void WriteIntermediate(string text) =>
            this.WriteLine(text, VerbosityLevel.med);

        public void WriteDetailed(string text) =>
            this.WriteLine(text, VerbosityLevel.high);
 
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
}
