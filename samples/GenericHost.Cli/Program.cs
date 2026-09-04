//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Hosting;
using GenericHost.Cli;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;

// Build a standard .NET Generic Host. Configuration (appsettings.json + environment variables),
// options binding, logging, and any other hosted services are configured through the host in the
// usual way. The host owns the service provider and application lifetime.
var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<GreetingOptions>(builder.Configuration.GetSection(GreetingOptions.SectionName));
builder.Services.AddSingleton(AnsiConsole.Console);

var host = builder.Build();

// CreateCommandAppBuilder bridges the built host to Spectre.Console.Cli. Command types are
// registered by Spectre at run time and resolved from the host's service provider, so GreetCommand
// can inject IOptions<GreetingOptions>, IAnsiConsole, and ILogger without any extra wiring.
return await host.CreateCommandAppBuilder()
    .WithDefaultCommand<GreetCommand>()
    .RunAsync(args);


/////////////////////////////////////////////////////////////////////////////////////////////////
// The equivalent IHostBuilder (Host.CreateDefaultBuilder) style looks like this:
//
// var host = Host.CreateDefaultBuilder(args)
//     .ConfigureServices((context, services) =>
//     {
//         services.Configure<GreetingOptions>(
//             context.Configuration.GetSection(GreetingOptions.SectionName));
//         services.AddSingleton(AnsiConsole.Console);
//     })
//     .Build();
//
// return await host.CreateCommandAppBuilder()
//     .WithDefaultCommand<GreetCommand>()
//     .RunAsync(args);
