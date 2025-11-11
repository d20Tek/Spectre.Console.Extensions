//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;

internal class MockAsyncCommandWithException : AsyncCommand
{
    public override Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellation)
    {
        throw new ArgumentOutOfRangeException(nameof(context));
    }
}
