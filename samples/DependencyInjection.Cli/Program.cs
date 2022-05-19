﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Samples.Common.Commands;
using D20Tek.Spectre.Console.Extensions.Injection;

namespace DependencyInjection.Cli
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
