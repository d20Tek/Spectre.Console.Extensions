//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using DependencyInjection.Cli;
using DependencyInjection.Cli.Commands;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace D20Tek.CountryService.Cli
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var startup = new Startup();
            var registrar = startup.ConfigureServices(new ServiceCollection());

            var app = new CommandApp<DefaultCommand>(registrar);
            app.Configure(config => startup.ConfigureCommands(config));

            return await app.RunAsync(args);
        }
    }
}
