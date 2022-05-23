//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Samples.Common.Commands;
using D20Tek.Spectre.Console.Extensions;

namespace Autofac.Cli
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await new CommandAppBuilder()
                             .WithAutofacContainer()
                             .WithStartup<Startup>()
                             .WithDefaultCommand<DefaultCommand>()
                             .Build()
                             .RunAsync(args);
        }
    }
}