using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions;

/// <summary>
/// Extensions for the Spectre.Console.Cli IConfigurator class.
/// </summary>
public static class ConfiguratorExtensions
{
    /// <summary>
    /// Applys the specified command configuration instance to the IConfigurator class for this CommandApp.
    /// </summary>
    /// <param name="configurator">IConfigurator instance to extend.</param>
    /// <param name="config">Command configuration class.</param>
    /// <returns></returns>
    public static IConfigurator ApplyConfiguration(this IConfigurator configurator, ICommandConfiguration config)
    {
        config.Configure(configurator);
        return configurator;
    }
}
