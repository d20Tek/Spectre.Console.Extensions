using D20Tek.Spectre.Console.Extensions.Injection;
using D20Tek.Spectre.Console.Extensions.Testing;
using D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.UnitTests;

[TestClass]
public class CommandConfigurationTests
{
    [TestMethod]
    public void ApplyConfiguration_WithCommandConfig_SetsConfiguratorMetadata()
    {
        // arrange
        var registrar = new DependencyInjectionTypeRegistrar(new ServiceCollection());
        var configurator = new FakeConfigurator(registrar);

        // act
        configurator.ApplyConfiguration(new TestConfiguration());

        // assert
        Assert.AreEqual(1, configurator.Commands.Count);
        var command = configurator.Commands[0];
        Assert.AreEqual(typeof(MockCommand), command.CommandType);
        Assert.AreEqual("config-command-test", command.Name);
    }

    internal class TestConfiguration : ICommandConfiguration
    {
        public void Configure(IConfigurator config)
        {
            config.AddCommand<MockCommand>("config-command-test");
        }
    }
}
