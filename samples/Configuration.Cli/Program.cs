//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Configuration.Cli;
using D20Tek.Spectre.Console.Extensions;
using D20Tek.Spectre.Console.Extensions.Configuration;

// WithConfiguration builds an IConfiguration from appsettings.json plus environment
// variables and registers it in the container. WithOptions binds the "Greeting" section
// to a strongly typed GreetingOptions class (validated via data annotations) so it can be
// injected as IOptions<GreetingOptions>. Configuration values stay separate from the
// command-line CommandSettings.
return await new CommandAppBuilder()
    .WithDIContainer()
    .WithConfiguration()
    .WithOptions<GreetingOptions>(GreetingOptions.SectionName)
    .WithStartup<Startup>()
    .WithDefaultCommand<GreetCommand>()
    .Build()
    .RunAsync(args);
