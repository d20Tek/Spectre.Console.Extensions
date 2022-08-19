//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Testing
{
    /// <summary>
    /// Test context to help unit test commands in isolation but with the 
    /// specified test CommandContext.
    /// </summary>
    public class CommandTestContext
    {
        /// <summary>
        /// Gets the CommandContext object for this test context.
        /// </summary>
        public CommandContext CommandContext { get; }

        /// <summary>
        /// Constructor to build the appropriate test CommandContext.
        /// </summary>
        /// <param name="remaining">The remaining arguments instance.</param>
        /// <param name="name">The name of the command.</param>
        /// <param name="data">Optional data passed to the command.</param>
        public CommandTestContext(
            IRemainingArguments? remaining = null,
            string name = "__defaultTestMethod",
            object? data = null)
        {
            var remainingArgs = remaining ?? new FakeRemainingArguments();
            CommandContext = new CommandContext(remainingArgs, name, data);
        }
    }
}
