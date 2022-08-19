//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Testing
{
    /// <summary>
    /// Extension to IConfigurator to provide access to more of the configured metadata for test validation.
    /// </summary>
    public interface ITestConfigurator : IConfigurator
    {
        /// <summary>
        /// Gets list of configured commands and their metadata.
        /// </summary>
        IList<CommandMetadata> Commands { get; }

        /// <summary>
        /// Gets whether there is a default command specified.
        /// </summary>
        CommandMetadata? DefaultCommand { get; }

        /// <summary>
        /// Gets a list of examples strings that were configured for commands.
        /// </summary>
        IList<string[]> Examples { get; }
    }
}
