//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace GenericHost.Cli;

internal sealed class GreetingOptions
{
    public const string SectionName = "Greeting";

    public string Message { get; set; } = "Hello";

    public string Punctuation { get; set; } = "!";

    public int MaxRepeat { get; set; } = 1;
}
