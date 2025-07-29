using D20Tek.Spectre.Console.Extensions.Controls;
using Spectre.Console;
using Spectre.Console.Cli;

namespace InteractivePrompt.Cli;

internal class GetNetWorthCommand : Command
{
    public override int Execute(CommandContext context)
    {
        var amount = AnsiConsole.Prompt(
            new CurrencyPrompt("Enter your net worth:")
                .WithDefaultValue(1000m)
                .WithExampleHint(1000m));

        AnsiConsole.MarkupLine($"You entered: {amount:C}");

        return 0;
    }
}
