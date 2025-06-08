using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace InteractivePrompt.Cli;

internal sealed class EchoCommand : Command<EchoCommand.Request>
{
    public sealed class Request : CommandSettings
    {
        [CommandOption("-t|--text <OUTPUT-TEXT>")]
        [Description("Holds the text to echo output.")]
        [DefaultValue("Hello world!")]
        public string? Text { get; set; }
    }

    private readonly IAnsiConsole _console;

    public EchoCommand(IAnsiConsole console) => _console = console;

    public override int Execute(CommandContext context, Request request)
    {
        _console.MarkupLine($"[yellow]echo:[/] {request.Text}");
        return 0;
    }
}
