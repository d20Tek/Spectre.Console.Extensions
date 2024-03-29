﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Mocks
{
    internal class MockCommand : Command
    {
        private readonly IAnsiConsole _writer;

        public MockCommand(IAnsiConsole console)
        {
            _writer = console;
        }

        public override int Execute(CommandContext context)
        {
            _writer.WriteLine("Success!");
            return 0;
        }
    }
}
