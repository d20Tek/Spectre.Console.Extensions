using D20Tek.Spectre.Console.Extensions.Controls;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace InteractivePrompt.Cli;

internal class GetNetWorthCommand : Command<GetNetWorthCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-m|--min <MIN-VALUE>")]
        [Description("Holds the minimum value allowed.")]
        public decimal? Min { get; set; }

        [CommandOption("-x|--max <MAX-VALUE>")]
        [Description("Holds the maximum value allowed.")]
        public decimal? Max { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var prompt = new CurrencyPrompt("Enter your net worth:")
            .WithDefaultValue(1000m)
            .WithExampleHint(1000m);

        if (settings.Min is not null) prompt.WithMinValue(settings.Min.Value);
        if (settings.Max is not null) prompt.WithMaxValue(settings.Max.Value);

        var amount = AnsiConsole.Prompt(prompt);
        AnsiConsole.MarkupLine($"You entered: {amount:C}");

        return 0;
    }
}
