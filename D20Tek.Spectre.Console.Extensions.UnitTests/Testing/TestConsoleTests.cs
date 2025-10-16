//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing
{
    [TestClass]
    public class TestConsoleTests
    {
        [TestMethod]
        public void Create()
        {
            // arranage

            // act
            using var c = new TestConsole();

            // assert
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.Profile);
            Assert.IsNotNull(c.ExclusivityMode);
            Assert.IsNotNull(c.Input);
            Assert.IsInstanceOfType(c.Input, typeof(TestConsoleInput));
            Assert.IsNotNull(c.Pipeline);
            Assert.IsNotNull(c.Cursor);
            Assert.IsNotNull(c.TestInput);
            Assert.AreEqual(string.Empty, c.Output);
            Assert.IsNotNull(c.Lines);
            Assert.IsFalse(c.EmitAnsiSequences);
        }

        [TestMethod]
        public void Write()
        {
            // arranage
            using var c = new TestConsole();

            // act
            c.WriteLine("Test line 1");
            c.WriteLine("Test line 2");

            // assert
            Assert.AreEqual("Test line 1\nTest line 2\n", c.Output);
            Assert.IsNotNull(c.Lines);
            Assert.HasCount(2, c.Lines);
        }

        [TestMethod]
        public void Clear()
        {
            // arranage
            using var c = new TestConsole();
            c.WriteLine("test");

            // act
            c.Clear();

            // assert
            Assert.IsNotNull(c.Output);
            Assert.IsNotNull(c.Lines);
        }

        [TestMethod]
        public void Write_WithEmitAnsi()
        {
            // arranage
            using var c = new TestConsole();
            c.EmitAnsiSequences = true;

            // act
            c.WriteLine("test");

            // assert
            StringAssert.Contains(c.Output, "test");
        }

        [TestMethod]
        public void Write_WithControlCodes()
        {
            // arranage
            using var c = new TestConsole();

            // act
            c.WriteLine("test\n\u001b[2J\u001b[3J\u001b[1;1H");

            // assert
            StringAssert.Contains(c.Output, "test");
        }

        [TestMethod]
        public void SetCursor()
        {
            // arranage
            var testCursor = new Mock<IAnsiConsoleCursor>().Object;
            using var c = new TestConsole();
            c.WriteLine("test");

            // act
            c.SetCursor(testCursor);

            // assert
            Assert.AreEqual(testCursor, c.Cursor);
        }
    }
}
