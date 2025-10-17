using D20Tek.Spectre.Console.Extensions.Commands;
using Spectre.Console;
using Spectre.Console.Cli;

namespace InteractivePrompt.Cli;

internal class InteractiveCommand(ICommandApp app, IAnsiConsole console) : InteractiveCommandBase(app, console)
{
    protected override void ShowWelcomeMessage(IAnsiConsole console)
    {
        console.Write(new FigletText("Interactive Prompt").Centered().Color(Color.Green));
        console.MarkupLine(
            "[green]Running interactive mode.[/] Type 'exit' to quit or '--help' to see available commands.");
    }

    protected override string GetAppPromptPrefix() => "ip>";

    protected override void ShowExitMessage(IAnsiConsole console) => console.WriteLine("Bye!");
}
