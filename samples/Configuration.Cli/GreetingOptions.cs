//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace Configuration.Cli;

internal sealed class GreetingOptions
{
    public const string SectionName = "Greeting";

    [Required]
    [MinLength(1)]
    public string Message { get; set; } = string.Empty;

    public string Punctuation { get; set; } = "!";

    [Range(1, 10)]
    public int MaxRepeat { get; set; } = 1;
}
