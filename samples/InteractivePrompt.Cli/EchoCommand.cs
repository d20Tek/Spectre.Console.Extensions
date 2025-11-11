using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace InteractivePrompt.Cli;

internal sealed class EchoCommand(IAnsiConsole console) : Command<EchoCommand.Request>
{
    public sealed class Request : CommandSettings
    {
        [CommandOption("-t|--text <OUTPUT-TEXT>")]
        [Description("Holds the text to echo output.")]
        [DefaultValue("Hello world!")]
        public string? Text { get; set; }
    }

    private readonly IAnsiConsole _console = console;

    public override int Execute(CommandContext context, Request request, CancellationToken cancellation)
    {
        _console.MarkupLine($"[yellow]echo:[/] {request.Text}");
        return 0;
    }
}
