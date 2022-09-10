//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;
using System.ComponentModel;

namespace D20Tek.Spectre.Console.Extensions.Settings
{
    /// <summary>
    /// Base command settings that provides a common verbosity option.
    /// </summary>
    public class VerbositySettings : CommandSettings
    {
        /// <summary>
        /// Verbosity level for this command
        /// </summary>
        [CommandOption("-v|--verbosity <VERBOSITY-LEVEL>")]
        [Description("The verbosity level for this operation: q(uiet), m(inimal), n(ormal), d(etailed), and diag(nostic).")]
        [DefaultValue(VerbosityLevel.Normal)]
        public VerbosityLevel Verbosity { get; set; } = VerbosityLevel.Normal;
    }
}
