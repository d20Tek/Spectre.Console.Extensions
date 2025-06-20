using D20Tek.Spectre.Console.Extensions.Controls;
using D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console;
using System.Text;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

[TestClass]
public class RecordTests
{

    [TestMethod]
    public void ReadLineRequest_WithChanges_ReturnUpdate()
    {
        // arrange
        var request = new ReadLineRequest(null, null, false, null, [], []);

        // act
        var result = request with
        {
            AnsiConsole = new TestConsole(),
            PromptStyle = new Style(Color.White),
            IsSecret = true,
            Mask = '$',
            Items = [ "choice 1", "choice 2"],
            History = [ "foo", "bar", "baz" ]
        };

        // assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void InputState_WithChanges_ReturnUpdate()
    {
        // arrange
        var request = new ReadLineRequest(null, null, false, null, [], []);
        var state = new InputState(null, null, -1, true, null, -1, null, false, false);

        // act
        var result = state with
        {
            Request = request,
            Buffer = new StringBuilder(),
            CursorIndex = 0,
            CompletionItems = ["foo", "bar"],
            HistoryIndex = 0,
            Handled = true,
            Done = true,
            InsertMode = true,
            SavedHistory = string.Empty
        };

        // assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void DefaultPromptValue_WithChanges_ReturnsUpdate()
    {
        // arrange
        var defValue = new DefaultPromptValue<string>("");

        // act
        var result = defValue with { Value = "test" };

        // assert
        Assert.AreEqual("test", result.Value);
    }
}
