//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing;

[TestClass]
public class CommandAppBasicResultTests
{
    [TestMethod]
    public void Create()
    {
        // arrange

        // act
        var r = new CommandAppBasicResult(0, "test done");

        // assert
        Assert.IsNotNull(r);
        Assert.AreEqual(0, r.ExitCode);
        Assert.AreEqual("test done", r.Output);
    }

    [TestMethod]
    public void Create_WithNullOutput()
    {
        // arrange

        // act
        var r = new CommandAppBasicResult(0, null);

        // assert
        Assert.IsNotNull(r);
        Assert.AreEqual(0, r.ExitCode);
        Assert.AreEqual(string.Empty, r.Output);
    }
}
