//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Settings;

namespace D20Tek.Spectre.Console.Extensions.Services
{
    /// <summary>
    /// Interface for a writer of output text based on a set verbosity level.
    /// </summary>
    public interface IVerbosityWriter
    {
        /// <summary>
        /// Gets or sets the verbosity level for current command app.
        /// </summary>
        public VerbosityLevel Verbosity { get; set; }

        /// <summary>
        /// Displays markup text for minimal verbosity.
        /// </summary>
        /// <param name="text">Text to display.</param>
        public void MarkupSummary(string text = "");

        /// <summary>
        /// Displays markup text for normal verbosity.
        /// </summary>
        /// <param name="text">Text to display.</param>
        public void MarkupNormal(string text = "");

        /// <summary>
        /// Displays markup text for detailed verbosity.
        /// </summary>
        /// <param name="text">Text to display.</param>
        public void MarkupDetailed(string text = "");

        /// <summary>
        /// Displays markup text for diagnostic verbosity.
        /// </summary>
        /// <param name="text">Text to display.</param>
        public void MarkupDiagnostics(string text = "");

        /// <summary>
        /// Displays simple text for minimal verbosity.
        /// </summary>
        /// <param name="text">Text to display.</param>
        public void WriteSummary(string text = "");

        /// <summary>
        /// Displays simple text for normal verbosity.
        /// </summary>
        /// <param name="text">Text to display.</param>
        public void WriteNormal(string text = "");

        /// <summary>
        /// Displays simple text for detailed verbosity.
        /// </summary>
        /// <param name="text">Text to display.</param>
        public void WriteDetailed(string text = "");

        /// <summary>
        /// Displays simple text for diagnostic verbosity.
        /// </summary>
        /// <param name="text">Text to display.</param>
        public void WriteDiagnostics(string text = "");
    }
}
