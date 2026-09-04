//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Logging.Cli;

internal sealed class LogSampleCommand(ILogger<LogSampleCommand> logger) : Command<LogSampleCommand.Settings>
{
    private readonly ILogger<LogSampleCommand> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "[NAME]")]
        [Description("The name to greet in the log output.")]
        [DefaultValue("world")]
        public string Name { get; set; } = "world";
    }

    protected override int Execute(
        [NotNull] CommandContext context,
        [NotNull] Settings settings,
        CancellationToken cancellation)
    {
        _logger.LogTrace("Starting {Command} for {Name}.", context.Name, settings.Name);
        _logger.LogDebug("Resolved settings with Name = {Name}.", settings.Name);
        _logger.LogInformation("Hello, {Name}! The logging sample is running.", settings.Name);
        _logger.LogWarning("This is a warning message that shows verbosity filtering.");

        try
        {
            throw new InvalidOperationException("Simulated failure to demonstrate exception logging.");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "An error occurred while processing the greeting.");
        }

        _logger.LogInformation("Logging sample completed.");
        return 0;
    }
}
