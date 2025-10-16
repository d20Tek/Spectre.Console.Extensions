//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing;

[TestClass]
public class CommandAppE2ERunnerTests
{
    [TestMethod]
    public async Task RunAsync_DefaultOperation_WithParameters()
    {
        // arrange

        // act
        var result = await CommandAppE2ERunner.RunAsync(TestRunMethodAsync, "--help");


        // assert
        Assert.AreEqual(0, result.ExitCode);
    }

    [TestMethod]
    public void Run_DefaultOperation()
    {
        // arrange

        // act
        var result = CommandAppE2ERunner.Run(TestRunMethod, String.Empty);

        // assert
        Assert.AreEqual(0, result.ExitCode);
    }

    private int TestRunMethod(string[] args) => 0;

    private Task<int> TestRunMethodAsync(string[] args) => Task.FromResult(0);
}
