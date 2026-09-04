//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Hosting.Fakes;

internal sealed class GreetStartup : HostStartupBase
{
    public bool ConfigureServicesCalled { get; private set; }

    public bool ConfigureCommandsCalled { get; private set; }

    public override void ConfigureServices(IServiceCollection services)
    {
        ConfigureServicesCalled = true;
        services.AddSingleton<IGreetingService, GreetingService>();
    }

    public override IConfigurator ConfigureCommands(IConfigurator config)
    {
        ConfigureCommandsCalled = true;
        config.AddCommand<GreetCommand>("greet");
        return config;
    }
}
