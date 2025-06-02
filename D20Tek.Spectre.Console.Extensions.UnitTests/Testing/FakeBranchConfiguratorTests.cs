using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing;

[TestClass]
public class FakeBranchConfiguratorTests
{
    [TestMethod]
    public void WithAlias()
    {
        // arrange
        var config = new FakeBranchConfigurator();

        // act
        var result = config.WithAlias("test-alias");

        // assert
        Assert.IsNotNull(result);

    }
}
