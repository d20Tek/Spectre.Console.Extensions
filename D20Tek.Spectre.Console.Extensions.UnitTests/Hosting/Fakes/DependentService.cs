//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Hosting.Fakes;

internal interface IDependentService
{
    string Describe();
}

[ExcludeFromCodeCoverage]
internal sealed class DependentService(IGreetingService greetingService) : IDependentService
{
    private readonly IGreetingService _greetingService =
        greetingService ?? throw new ArgumentNullException(nameof(greetingService));

    public string Describe() => _greetingService.Greet("dependency");
}
