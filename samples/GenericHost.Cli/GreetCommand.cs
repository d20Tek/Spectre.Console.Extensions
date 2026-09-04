//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace GenericHost.Cli;

internal sealed class GreetCommand(
    IOptions<GreetingOptions> options,
    IAnsiConsole console,
    ILogger<GreetCommand> logger)
    : Command<GreetCommand.Settings>
{
    private readonly GreetingOptions _options = (options ?? throw new ArgumentNullException(nameof(options))).Value;
    private readonly IAnsiConsole _console = console ?? throw new ArgumentNullException(nameof(console));
    private readonly ILogger<GreetCommand> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

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
        // GreetingOptions, IAnsiConsole, and ILogger are all resolved from the .NET Generic Host's
        // service provider. The command type itself is registered by Spectre.Console.Cli at run
        // time, and the HostTypeRegistrar bridges those registrations to the host container.
        var repeat = settings.Repeat > 0 ? settings.Repeat : _options.MaxRepeat;
        _logger.LogInformation("Greeting {Name} {Repeat} time(s).", settings.Name, repeat);

        for (var i = 0; i < repeat; i++)
        {
            _console.MarkupLineInterpolated($"[green]{_options.Message}[/], [yellow]{settings.Name}[/]{_options.Punctuation}");
        }

        return 0;
    }
}
