//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Samples.Common.Commands;
using D20Tek.Spectre.Console.Extensions.Injection;

namespace Ninject.Cli
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var app = NinjectFactory.CreateCommandApp<Startup, DefaultCommand>();
            return await app.RunAsync(args);
        }
    }
}
