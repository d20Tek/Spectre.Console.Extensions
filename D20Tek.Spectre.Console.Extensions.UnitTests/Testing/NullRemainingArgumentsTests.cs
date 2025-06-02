using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing;

[TestClass]
public class NullRemainingArgumentsTests
{
    [TestMethod]
    public void NoArguments_ReturnsEmptyList()
    {
        // arrange
        var remaining = NullRemainingArguments.Instance;

        // act

        // assert
        Assert.IsNotNull(remaining);
        Assert.AreEqual(0, remaining.Raw.Count);
        Assert.AreEqual(0, remaining.Parsed.Count);
    }
}
