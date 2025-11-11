using D20Tek.Spectre.Console.Extensions.Controls;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class AnsiConsoleExtensionsTests
{
    private readonly TestConsole _console = new();

    [TestMethod]
    public void WriteMessages_WithSingleMessage_WritesToConsole()
    {
        // arrange

        // act
        _console.WriteMessages("test message");

        // assert
        Assert.Contains("test message", _console.Output);
    }

    [TestMethod]
    public void WriteMessages_WithMultipleMessages_WritesToConsole()
    {
        // arrange

        // act
        _console.WriteMessages("test message1", "test message2", "test message3");

        // assert
        Assert.Contains("test message1", _console.Output);
        Assert.Contains("test message2", _console.Output);
        Assert.Contains("test message3", _console.Output);
    }

    [TestMethod]
    public void WriteMessages_WithNoMessages_WriteEmptyLine()
    {
        // arrange

        // act
        _console.WriteMessages();

        // assert
        Assert.HasCount(1, _console.Lines);
    }

    [TestMethod]
    public void WriteMessagesConditional_WithTrueCondition_WritesToConsole()
    {
        // arrange

        // act
        _console.WriteMessagesConditional(true, "test message1", "test message2", "test message3");

        // assert
        Assert.Contains("test message1", _console.Output);
        Assert.Contains("test message2", _console.Output);
        Assert.Contains("test message3", _console.Output);
    }

    [TestMethod]
    public void WriteMessagesConditional_WithFalseCondition_DoesNotWriteToConsole()
    {
        // arrange

        // act
        _console.WriteMessagesConditional(false, "test message1", "test message2", "test message3");

        // assert
        Assert.DoesNotContain("test message1", _console.Output);
        Assert.DoesNotContain("test message2", _console.Output);
        Assert.DoesNotContain("test message3", _console.Output);
    }
}
