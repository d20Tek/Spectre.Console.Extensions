//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Spectre.Console.Extensions.UnitTests.Hosting.Fakes;

internal interface IGreetingService
{
    string Greet(string name);
}

internal sealed class GreetingService : IGreetingService
{
    public string Greet(string name) => $"Hello, {name}!";
}
