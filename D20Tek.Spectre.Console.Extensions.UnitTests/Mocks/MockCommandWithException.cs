//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;
using System;
using System.Threading;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;

internal class MockCommandWithException : Command
{
    protected override int Execute(CommandContext context, CancellationToken cancellation)
    {
        throw new ArgumentOutOfRangeException(nameof(context));
    }
}
