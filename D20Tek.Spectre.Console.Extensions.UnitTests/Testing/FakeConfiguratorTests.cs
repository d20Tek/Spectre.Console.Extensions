using D20Tek.Spectre.Console.Extensions.Injection;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console.Cli.Help;
using Spectre.Console.Rendering;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing;

[TestClass]
public class FakeConfiguratorTests
{
    [TestMethod]
    public void SetHelpProvider()
    {
        // arrange
        var config = CreateConfigurator();
        var provider = new TestHelpProvider();

        // act
        var result = config.SetHelpProvider(provider);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(provider, config.HelperProvider);
    }

    [TestMethod]
    public void SetHelpProviderTyped()
    {
        // arrange
        var config = CreateConfigurator();

        // act
        var result = config.SetHelpProvider<TestHelpProvider>();

        // assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(config.HelperProvider);
    }

    private FakeConfigurator CreateConfigurator()
    {
        var registrar = new DependencyInjectionTypeRegistrar(new ServiceCollection());
        return new FakeConfigurator(registrar);
    }

    [ExcludeFromCodeCoverage]
    internal class TestHelpProvider : IHelpProvider
    {
        public IEnumerable<IRenderable> Write(ICommandModel model, ICommandInfo command) => 
            Enumerable.Empty<IRenderable>();
    }
}
