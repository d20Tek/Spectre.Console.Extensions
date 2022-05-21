//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Mocks
{
    internal interface IMockService
    {
        public void Print();
    }

    [ExcludeFromCodeCoverage]
    internal class MockService : IMockService
    {
        public void Print()
        {
        }
    }
}
