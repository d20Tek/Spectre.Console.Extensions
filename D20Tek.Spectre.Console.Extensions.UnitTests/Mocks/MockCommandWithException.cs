﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;
using System;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Mocks
{
    internal class MockCommandWithException : Command
    {
        public override int Execute(CommandContext context)
        {
            throw new ArgumentOutOfRangeException(nameof(context));
        }
    }
}
