//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions
{
    /// <summary>
    /// Extension methods to support test operations for CommandAppBuilder.
    /// </summary>
    public static class CommandAppBuilderTestExtensions
    {
        /// <summary>
        /// Extension method to app test configuration to the CommmandAppBuilder
        /// </summary>
        /// <param name="builder">Builder this method is extending.</param>
        /// <param name="action">Configuration action to run.</param>
        /// <returns>The current CommandAppBuilder.</returns>
        public static CommandAppBuilder WithTestConfiguration(
            this CommandAppBuilder builder,
            Action<IConfigurator> action)
        {
            builder.SetCustomConfig = () => {
                if (builder.App != null)
                {
                    builder.App.Configure(action);
                }
            };

            return builder;
        }
    }
}
