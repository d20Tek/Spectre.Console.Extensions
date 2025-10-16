//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing;

[TestClass]
public class TestConsoleInputTests
{
    [TestMethod]
    public void Create()
    {
        // arrange

        // act
        var input = new TestConsoleInput();

        // assert
        Assert.IsNotNull(input);
        Assert.IsFalse(input.IsKeyAvailable());
    }

    [TestMethod]
    public void PushTextWithEnter()
    {
        // arrange
        var input = new TestConsoleInput();
        var expected = new ConsoleKeyInfo('t', (ConsoleKey)'t', false, false, false);

        // act
        input.PushTextWithEnter("test");

        // assert
        Assert.IsTrue(input.IsKeyAvailable());
        Assert.AreEqual(expected, input.ReadKey(false));
    }

    [TestMethod]
    public void PushText_WithNull()
    {
        // arrange
        var input = new TestConsoleInput();

        // act
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () => input.PushTextWithEnter(null));
    }


    [TestMethod]
    public void ReadKey_ToTheEnd()
    {
        // arrange
        var input = new TestConsoleInput();
        input.PushText("test");

        // act
        var key = input.ReadKey(false);
        key = input.ReadKey(false);
        key = input.ReadKey(false);
        key = input.ReadKey(false);

        // assert
        Assert.IsFalse(input.IsKeyAvailable());
    }

    [TestMethod]
    public void ReadKey_PastTheEnd()
    {
        // arrange
        var input = new TestConsoleInput();
        input.PushText("1");

        // act
        _ = input.ReadKey(false);
        Assert.ThrowsExactly<InvalidOperationException>([ExcludeFromCodeCoverage] () => input.ReadKey(false));
    }

    [TestMethod]
    public async Task ReadKeyAsync()
    {
        // arrange
        var input = new TestConsoleInput();
        var cancel = new CancellationToken();
        var expected = new ConsoleKeyInfo('t', (ConsoleKey)'t', false, false, false);
        input.PushTextWithEnter("test");

        // act
        var actual = await input.ReadKeyAsync(false, cancel);

        // assert
        Assert.IsTrue(input.IsKeyAvailable());
        Assert.AreEqual(expected, actual);
    }
}
