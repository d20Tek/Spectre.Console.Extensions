using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions;

/// <summary>
/// Interface definition for command configuration classes. These classes encapsulate all configuration
/// for a grouped set of commands. This makes it easier to provide command configuration in a clean,
/// structured way.
/// </summary>
public interface ICommandConfiguration
{
    /// <summary>
    /// Configures the commands within this method to the specified IConfigurator for this CommandApp.
    /// </summary>
    /// <param name="config"></param>
    void Configure(IConfigurator config);
}
