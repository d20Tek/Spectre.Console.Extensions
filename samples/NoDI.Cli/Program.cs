//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions;

namespace NoDI.Cli
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await new CommandAppBuilder()
                             .WithStartup<Startup>()
                             .Build<MyCommand>()
                             .RunAsync(args);
        }
    }
}
