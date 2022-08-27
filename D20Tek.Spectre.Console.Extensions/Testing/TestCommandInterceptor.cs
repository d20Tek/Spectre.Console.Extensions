//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Testing
{
    internal class TestCommandInterceptor : ICommandInterceptor
    {
        public CommandContext? Context { get; private set; }
        
        public CommandSettings? Settings { get; private set; }

        public void Intercept(CommandContext context, CommandSettings settings)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }
    }
}
