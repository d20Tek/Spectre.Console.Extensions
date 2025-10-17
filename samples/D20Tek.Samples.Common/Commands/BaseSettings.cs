//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Samples.Common.Services;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace D20Tek.Samples.Common.Commands;

public class BaseSettings : CommandSettings
{
    [CommandOption("-v|--verbose <VERBOSE-LEVEL>")]
    [Description("The verbosity level for this operation (low, med, high).")]
    [DefaultValue(VerbosityLevel.med)]
    public VerbosityLevel Verbose { get; set; } = VerbosityLevel.low;
}
