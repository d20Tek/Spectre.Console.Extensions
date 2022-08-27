//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------

namespace D20Tek.Spectre.Console.Extensions.Testing
{
    /// <summary>
    /// The basic test result returned from running command app tests.
    /// </summary>
    public class CommandAppBasicResult
    {
        /// <summary>
        /// Gets the exit code returned by the command app.
        /// </summary>
        public int ExitCode { get; }

        /// <summary>
        /// Gets the output text from the command app. This text contains the console
        /// output produced by the command app.
        /// </summary>
        public string Output { get; }

        /// <summary>
        /// Constructor for basic result.
        /// </summary>
        /// <param name="exitCode">Initial command app exit code.</param>
        /// <param name="output">Initial command app output text.</param>
        public CommandAppBasicResult(int exitCode, string output)
        {
            ExitCode = exitCode;
            Output = output ?? string.Empty;
        }
    }
}
