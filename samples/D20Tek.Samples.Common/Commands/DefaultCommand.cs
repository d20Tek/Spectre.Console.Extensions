//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Samples.Common.Services;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Samples.Common.Commands
{
    internal class DefaultCommand : Command<BaseSettings>
    {
        private readonly IDisplayWriter _displayWriter;

        public DefaultCommand(IDisplayWriter displayWriter)
        {
            this._displayWriter = displayWriter ?? throw new ArgumentNullException(nameof(displayWriter));
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] BaseSettings settings)
        {
            _displayWriter.Verbosity = settings.Verbose;
            _displayWriter.WriteSummary($"CommandApp running {this.GetType().Assembly.FullName}");
            _displayWriter.WriteSummary($"=> Executing command - {context.Name}.");
            _displayWriter.WriteIntermediate($"   Args: Verbose={Enum.GetName<VerbosityLevel>(settings.Verbose)}");

            _displayWriter.WriteDetailed();
            _displayWriter.WriteDetailed("    display text for verbose command run.");

            _displayWriter.WriteSummary();
            _displayWriter.MarkupSummary($"[green]Command completed successfully![/]");

            return 0;
        }
    }
}
