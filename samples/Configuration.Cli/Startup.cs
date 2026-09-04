//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions;
using Spectre.Console.Cli;

namespace Configuration.Cli;

internal sealed class Startup : StartupBase
{
    public override void ConfigureServices(ITypeRegistrar registrar)
    {
        // Configuration and options are wired up through the CommandAppBuilder using
        // WithConfiguration and WithOptions in Program.cs, which makes IConfiguration
        // and IOptions<GreetingOptions> available for injection into any command.
    }

    public override IConfigurator ConfigureCommands(IConfigurator config)
    {
        config.CaseSensitivity(CaseSensitivity.None);
        config.SetApplicationName("Configuration.Cli");
        config.ValidateExamples();

        config.AddCommand<GreetCommand>("greet")
            .WithDescription("Greets the given name using messages bound from configuration.")
            .WithExample(["greet", "Linus"])
            .WithExample(["greet", "Linus", "--repeat", "2"]);

        config.AddCommand<InfoCommand>("info")
            .WithDescription("Shows app metadata read directly through the IConfiguration interface.")
            .WithExample(["info"]);

        return config;
    }
}
