//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Settings;
using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Services
{
    /// <summary>
    /// Console output service that respected the set verbosity level.
    /// </summary>
    public class ConsoleVerbosityWriter : IVerbosityWriter
    {
        private readonly IAnsiConsole _console;

        /// <inheritdoc/>
        public VerbosityLevel Verbosity { get; set; } = VerbosityLevel.Normal;

        /// <summary>
        /// Constructor that takes a console object to use for output.
        /// </summary>
        /// <param name="console">Console to use for output.</param>
        /// <exception cref="ArgumentNullException">when console parameter is null.</exception>
        public ConsoleVerbosityWriter(IAnsiConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        /// <inheritdoc/>
        public void MarkupDetailed(string text) =>
            this.MarkupLine(text, VerbosityLevel.Detailed);

        /// <inheritdoc/>
        public void MarkupDiagnostics(string text) =>
            this.MarkupLine(text, VerbosityLevel.Diagnostic);

        /// <inheritdoc/>
        public void MarkupNormal(string text) =>
            this.MarkupLine(text, VerbosityLevel.Normal);

        /// <inheritdoc/>
        public void MarkupSummary(string text) =>
            this.MarkupLine(text, VerbosityLevel.Minimal);

        /// <inheritdoc/>
        public void WriteDetailed(string text) =>
            this.WriteLine(text, VerbosityLevel.Detailed);

        /// <inheritdoc/>
        public void WriteDiagnostics(string text) =>
            this.WriteLine(text, VerbosityLevel.Diagnostic);

        /// <inheritdoc/>
        public void WriteNormal(string text) =>
            this.WriteLine(text, VerbosityLevel.Normal);

        /// <inheritdoc/>
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
