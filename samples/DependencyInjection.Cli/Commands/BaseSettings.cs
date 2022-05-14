//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;
using System.ComponentModel;

namespace DependencyInjection.Cli.Commands
{
    public class BaseSettings : CommandSettings
    {
        [CommandOption("-v|--verbose <VERBOSE-LEVEL>")]
        [Description("The verbosity level for this operation (low, med, high).")]
        [DefaultValue(VerbosityLevel.med)]
        public VerbosityLevel Verbose { get; set; } = VerbosityLevel.low;
    }
}
