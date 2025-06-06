using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.Spectre.Console.Extensions.Injection;

/// <summary>
/// Interface to extend ITypeRegistrar based containers to support registering for 
/// specific instance lifetimes.
/// </summary>
public interface ISupportLifetimes
{
    /// <summary>
    /// Gets ServiceCollection to allow for extension methods.
    /// </summary>
    IServiceCollection Services { get; }
}