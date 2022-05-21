//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Samples.Common.Commands;
using D20Tek.Spectre.Console.Extensions.Injection;

namespace SimpleInjector.Cli
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await new CommandAppBuilder()
                             .WithSimpleInjectorContainer()
                             .WithStartup<Startup>()
                             .Build<DefaultCommand>()
                             .RunAsync(args);
        }
    }
}
