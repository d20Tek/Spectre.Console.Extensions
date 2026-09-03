//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions;
using D20Tek.Spectre.Console.Extensions.Settings;
using Logging.Cli;

// WithLogging registers a Spectre.Console-rendered ILogger and derives the minimum
// LogLevel from the supplied verbosity level. Detailed maps to LogLevel.Debug, so the
// trace message below is filtered out while debug and higher are shown. The optional
// configure delegate controls how each log entry is rendered.
return await new CommandAppBuilder()
    .WithDIContainer()
    .WithLogging(
        VerbosityLevel.Detailed,
        configure: options =>
        {
            options.IncludeCategory = true;
            options.IncludeTimestamp = true;
        })
    .WithStartup<Startup>()
    .WithDefaultCommand<LogSampleCommand>()
    .Build()
    .RunAsync(args);
