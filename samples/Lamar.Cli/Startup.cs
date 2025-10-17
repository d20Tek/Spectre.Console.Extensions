//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Samples.Common.Commands;
using D20Tek.Samples.Common.Services;
using D20Tek.Spectre.Console.Extensions;
using Spectre.Console.Cli;

namespace Lamar.Cli;

internal class Startup : StartupBase
{
    public override void ConfigureServices(ITypeRegistrar registrar)
    {
        // register services here...
        registrar.Register(typeof(IDisplayWriter), typeof(ConsoleDisplayWriter));
    }

    public override IConfigurator ConfigureCommands(IConfigurator config)
    {
        config.CaseSensitivity(CaseSensitivity.None);
        config.SetApplicationName("Lamar.Cli");
        config.ValidateExamples();

        config.AddCommand<DefaultCommand>("default")
              .WithDescription("Default command that displays some text.")
              .WithExample(["default", "--verbose", "high"]);

        return config;
    }
}
