//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing;

[TestClass]
public class CommandAppAssertionExceptionTests
{
    [TestMethod]
    public void Constructor_Default_CreatesInstance()
    {
        // Arrange

        // Act
        var ex = new CommandAppAssertionException();

        // Assert
        Assert.IsNotNull(ex);
    }

    [TestMethod]
    public void Constructor_WithMessage_SetsMessage()
    {
        // Arrange

        // Act
        var ex = new CommandAppAssertionException("failure");

        // Assert
        Assert.AreEqual("failure", ex.Message);
    }

    [TestMethod]
    public void Constructor_WithMessageAndInnerException_SetsBoth()
    {
        // Arrange
        var inner = new InvalidOperationException("inner");

        // Act
        var ex = new CommandAppAssertionException("failure", inner);

        // Assert
        Assert.AreEqual("failure", ex.Message);
        Assert.AreSame(inner, ex.InnerException);
    }
}
