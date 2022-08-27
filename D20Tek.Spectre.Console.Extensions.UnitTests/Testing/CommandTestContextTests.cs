//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing
{
    [TestClass]
    public class CommandTestContextTests
    {
        [TestMethod]
        public void ExecuteCommand()
        {
            // arrange
            var context = new CommandTestContext();
            var console = new TestConsole();
            var command = new MockCommand(console);

            // act
            var result = command.Execute(context.CommandContext);

            // assert
            Assert.AreEqual(0, result);
        }
    }
}
