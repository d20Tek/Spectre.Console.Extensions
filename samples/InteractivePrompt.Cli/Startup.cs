using D20Tek.Spectre.Console.Extensions;
using Spectre.Console.Cli;

namespace InteractivePrompt.Cli;

internal sealed class Startup : StartupBase
{
    public override IConfigurator ConfigureCommands(IConfigurator config)
    {
        config.CaseSensitivity(CaseSensitivity.None)
              .SetApplicationName("inter-prompt")
              .SetApplicationVersion("1.0")
              .ValidateExamples();

        config.AddCommand<InteractiveCommand>("start")
              .WithDescription("Starts an interactive prompt for this sample CLI.")
              .WithExample(["start"]);

        config.AddCommand<EchoCommand>("echo")
              .WithDescription("Echoes out the text that call provides.")
              .WithExample(["echo", "--text", "'Test string'"]);

        return config;
    }

    public override void ConfigureServices(ITypeRegistrar registrar)
    {
    }
}
