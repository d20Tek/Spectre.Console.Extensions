using D20Tek.Spectre.Console.Extensions.Controls;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace InteractivePrompt.Cli;

internal class GetPathCommand : Command<GetPathCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-k|--kind <PATH-KIND>")]
        [Description("Restricts the kind of filesystem entry required (Any, File, Directory).")]
        [DefaultValue(PathKind.Any)]
        public PathKind Kind { get; set; }

        [CommandOption("-e|--ext <EXTENSION>")]
        [Description("Restricts accepted paths to the specified file extensions.")]
        public string[]? Extensions { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings, CancellationToken cancellation)
    {
        var prompt = new PathPrompt("Enter a path")
                            .WithBaseDirectory(Directory.GetCurrentDirectory())
                            .WithPathKind(settings.Kind);

        if (settings.Extensions is { Length: > 0 }) prompt.WithExtensions(settings.Extensions);

        var path = AnsiConsole.Prompt(prompt);
        AnsiConsole.MarkupLine($"You entered: [green]{path}[/]");

        return 0;
    }
}
