using D20Tek.Spectre.Console.Extensions.Commands;
using Spectre.Console;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Commands;

internal class TestInteractivePrompt : InteractiveCommandBase
{
    public TestInteractivePrompt(ICommandApp app, IAnsiConsole console) : base(app, console)
    {
    }

    protected override void ShowWelcomeMessage(IAnsiConsole console) => console.WriteLine("Test Starting...");

    protected override string GetAppPromptPrefix()
    {
        base.GetAppPromptPrefix();
        return "test>";
    }

    protected override void ShowExitMessage(IAnsiConsole console)
    {
        base.ShowExitMessage(console);
        console.WriteLine("Test Ending...");
    }

    protected override bool CanContinue(int result)
    {
        base.CanContinue(result);
        return result == 0;
    }
}
