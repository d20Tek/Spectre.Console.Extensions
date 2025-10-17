//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions;
using Spectre.Console.Cli;

namespace NoDI.Cli;

internal class Startup : StartupBase
{
    public override void ConfigureServices(ITypeRegistrar registrar)
    {
        // Not using DI, so no services registered.
    }

    public override IConfigurator ConfigureCommands(IConfigurator config)
    {
        config.CaseSensitivity(CaseSensitivity.None);
        config.SetApplicationName("NoDI.Cli");
        config.ValidateExamples();

        config.AddCommand<MyCommand>("mine")
              .WithDescription("Default command that displays some text.")
              .WithExample(["mine"]);

        return config;
    }
}
