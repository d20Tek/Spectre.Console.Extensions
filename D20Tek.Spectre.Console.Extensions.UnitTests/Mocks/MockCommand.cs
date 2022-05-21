//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Mocks
{
    internal class MockCommand : Command
    {
        public override int Execute(CommandContext context)
        {
            return 0;
        }
    }
}
