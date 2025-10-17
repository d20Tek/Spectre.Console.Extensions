//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions;
using NoDI.Cli;

await new CommandAppBuilder().WithStartup<Startup>()
                             .WithDefaultCommand<MyCommand>()
                             .Build()
                             .RunAsync(args);
