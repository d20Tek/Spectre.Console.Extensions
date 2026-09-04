//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Configuration.Fakes;

internal sealed class SampleOptions
{
    [Required]
    [MinLength(1)]
    public string Name { get; set; } = string.Empty;

    public int Count { get; set; }
}
