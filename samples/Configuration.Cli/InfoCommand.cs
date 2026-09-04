//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace Configuration.Cli;

internal sealed class InfoCommand(IConfiguration configuration, IAnsiConsole console) : Command<InfoCommand.Settings>
{
    private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    private readonly IAnsiConsole _console = console ?? throw new ArgumentNullException(nameof(console));

    public sealed class Settings : CommandSettings { }

    protected override int Execute(
        [NotNull] CommandContext context,
        [NotNull] Settings settings,
        CancellationToken cancellation)
    {
        // Instead of binding to a strongly typed options class, this command reads values
        // directly through the IConfiguration interface that WithConfiguration registered in
        // the container. Individual keys are read with indexer/GetValue, and a whole section
        // can be accessed with GetSection.
        var title = _configuration["App:Title"] ?? "(unknown)";
        var version = _configuration.GetValue<string>("App:Version") ?? "(unknown)";
        var features = _configuration.GetSection("App:Features").Get<string[]>() ?? [];

        _console.MarkupLineInterpolated($"[bold]{title}[/] v[yellow]{version}[/]");
        _console.MarkupLineInterpolated($"Features: [green]{string.Join(", ", features)}[/]");

        return 0;
    }
}
