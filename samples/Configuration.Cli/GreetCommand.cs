//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Configuration.Cli;

internal sealed class GreetCommand(IOptions<GreetingOptions> options, IAnsiConsole console)
    : Command<GreetCommand.Settings>
{
    private readonly GreetingOptions _options = (options ?? throw new ArgumentNullException(nameof(options))).Value;
    private readonly IAnsiConsole _console = console ?? throw new ArgumentNullException(nameof(console));

    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "[NAME]")]
        [Description("The name to greet.")]
        [DefaultValue("world")]
        public string Name { get; set; } = "world";

        [CommandOption("-r|--repeat <COUNT>")]
        [Description("How many times to repeat the greeting. When not specified, the configured MaxRepeat value is used.")]
        [DefaultValue(0)]
        public int Repeat { get; set; }
    }

    protected override int Execute(
        [NotNull] CommandContext context,
        [NotNull] Settings settings,
        CancellationToken cancellation)
    {
        // GreetingOptions is bound from appsettings.json and validated via data annotations.
        // Command-line Settings remain separate from configuration-driven options. When no
        // --repeat override is supplied, the configured MaxRepeat value drives the count.
        var repeat = settings.Repeat > 0 ? settings.Repeat : _options.MaxRepeat;

        for (var i = 0; i < repeat; i++)
        {
            _console.MarkupLineInterpolated($"[green]{_options.Message}[/], [yellow]{settings.Name}[/]{_options.Punctuation}");
        }

        return 0;
    }
}
