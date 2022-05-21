//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace NoDI.Cli
{
    public class MyCommand : Command
    {
        public override int Execute([NotNull] CommandContext context)
        {
            AnsiConsole.WriteLine($"=> MyCommand: Executing command - {context.Name}.");

            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine($"[green]Command completed successfully![/]");

            return 0; 
        }
    }
}
