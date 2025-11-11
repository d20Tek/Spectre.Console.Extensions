using D20Tek.Spectre.Console.Extensions.Injection;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;
using Spectre.Console.Rendering;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

    [TestMethod]
    public void AddAsyncDelegate()
    {
        // arrange
        var config = CreateConfigurator();

        // act
        var result = config.AddAsyncDelegate<EmptyCommandSettings>("test-async-delegate", TestAsyncDelegate);

        // assert
        Assert.IsNotNull(result);
        Assert.HasCount(1, config.Commands);
        Assert.AreEqual("test-async-delegate", config.Commands.First().Name);
    }

    [TestMethod]
    public async Task CallAddedAsyncDelegate()
    {
        // arrange
        var config = CreateConfigurator();

        // act
        var result = config.AddAsyncDelegate<EmptyCommandSettings>("test-async-delegate", TestAsyncDelegate);

        // assert
        var configurator = result as FakeCommandConfigurator;
        Assert.IsNotNull(configurator);
        Assert.IsNotNull(configurator.Command.AsyncDelegate);
        CommandContext context = new([], NullRemainingArguments.Instance, "test", null);
        Assert.AreEqual(0, await configurator.Command.AsyncDelegate(context, new EmptyCommandSettings()));
    }

    [TestMethod]
    public void CallAddedDelegate()
    {
        // arrange
        var config = CreateConfigurator();

        // act
        var result = config.AddDelegate<EmptyCommandSettings>("test-delegate", TestDelegate);

        // assert
        var configurator = result as FakeCommandConfigurator;
        Assert.IsNotNull(configurator);
        Assert.IsNotNull(configurator.Command.Delegate);
        CommandContext context = new([], NullRemainingArguments.Instance, "test", null);
        Assert.AreEqual(0, configurator.Command.Delegate(context, new EmptyCommandSettings()));
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

    [ExcludeFromCodeCoverage]
    private int TestDelegate(CommandContext arg1, EmptyCommandSettings arg2, CancellationToken cancellation) => 0;

    [ExcludeFromCodeCoverage]
    private Task<int> TestAsyncDelegate(CommandContext arg1, EmptyCommandSettings arg2, CancellationToken cancellation) =>
        Task.FromResult(0);
}
