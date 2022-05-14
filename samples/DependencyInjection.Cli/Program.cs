//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Injection;
using DependencyInjection.Cli;
using DependencyInjection.Cli.Commands;

namespace D20Tek.CountryService.Cli
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var app = DependencyInjectionFactory.CreateCommandApp<Startup, DefaultCommand>();
            return await app.RunAsync(args);
        }
    }
}
