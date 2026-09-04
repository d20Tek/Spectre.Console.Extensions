//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions;
using Spectre.Console.Cli;

namespace Logging.Cli;

internal sealed class Startup : StartupBase
{
    public override void ConfigureServices(ITypeRegistrar registrar)
    {
        // No additional services are required for this sample. Logging is wired up
        // through the CommandAppBuilder using WithLogging in Program.cs, which makes
        // ILogger<T> available for injection into any command.
    }

    public override IConfigurator ConfigureCommands(IConfigurator config)
    {
        config.CaseSensitivity(CaseSensitivity.None);
        config.SetApplicationName("Logging.Cli");
        config.ValidateExamples();

        config.AddCommand<LogSampleCommand>("greet")
            .WithDescription("Writes a few log messages at different levels through Spectre.Console.")
            .WithExample(["greet", "Linus"]);

        return config;
    }
}
