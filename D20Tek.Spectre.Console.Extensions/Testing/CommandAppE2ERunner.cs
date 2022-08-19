//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Testing
{
    /// <summary>
    /// Test runner class that encapsulates all of the setup and configuration for 
    /// running end-to-end tests for our command apps.
    ///
    /// The command app entry point functions are expected in one of two forms:
    ///     public static int Main(string[] args)
    ///     public static async Task&lt;int&gt; Main(string[] args)
    /// </summary>
    public static class CommandAppE2ERunner
    {
        private static readonly char[] _separators = new char[] { ' ', '\t' };

        /// <summary>
        /// Runs the command app asychronously and returns the results from the specified program.
        /// </summary>
        /// <param name="mainEntryPointAsync">Entry point method for the app (usually Program.Main).</param>
        /// <param name="commandLine">Command line parameters represented as its string form.</param>
        /// <returns>Returns CommandAppBasicResult with information about the run command.</returns>
        public static async Task<CommandAppBasicResult> RunAsync(
            Func<string[], Task<int>> mainEntryPointAsync,
            string commandLine)
        {
            var args = commandLine.Split(_separators);
            return await RunAsync(mainEntryPointAsync, args);
        }

        /// <summary>
        /// Runs the command app asychronously and returns the results from the specified program.
        /// </summary>
        /// <param name="mainEntryPointAsync">Entry point method for the app (usually Program.Main).</param>
        /// <param name="args">Command line arguments represented as list of split strings.</param>
        /// <returns>Returns CommandAppBasicResult with information about the run command.</returns>
        public static async Task<CommandAppBasicResult> RunAsync(
            Func<string[], Task<int>> mainEntryPointAsync,
            string[] args)
        {
            ArgumentNullException.ThrowIfNull(mainEntryPointAsync, nameof(mainEntryPointAsync));

            AnsiConsole.Record();
            var exitCode = await mainEntryPointAsync(args);

            return new CommandAppBasicResult(exitCode, AnsiConsole.ExportText());
        }

        /// <summary>
        /// Runs the command app sychronously and returns the results from the specified program.
        /// </summary>
        /// <param name="mainEntryPoint">Entry point method for the app (usually Program.Main).</param>
        /// <param name="commandLine">Command line parameters represented as its string form.</param>
        /// <returns>Returns CommandAppBasicResult with information about the run command.</returns>
        public static CommandAppBasicResult Run(Func<string[], int> mainEntryPoint, string commandLine)
        {
            var args = commandLine.Split(_separators);
            return Run(mainEntryPoint, args);
        }

        /// <summary>
        /// Runs the command app sychronously and returns the results from the specified program.
        /// </summary>
        /// <param name="mainEntryPoint">Entry point method for the app (usually Program.Main).</param>
        /// <param name="args">Command line arguments represented as list of split strings.</param>
        /// <returns>Returns CommandAppBasicResult with information about the run command.</returns>
        public static CommandAppBasicResult Run(Func<string[], int> mainEntryPoint, string[] args)
        {
            ArgumentNullException.ThrowIfNull(mainEntryPoint, nameof(mainEntryPoint));

            AnsiConsole.Record();
            var exitCode = mainEntryPoint(args);

            return new CommandAppBasicResult(exitCode, AnsiConsole.ExportText());
        }
    }
}
