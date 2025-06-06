using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Injection;

/// <summary>
/// Extension methods for the ITypeRegistrar interface.
/// </summary>
public static class TypeRegistrarExtensions
{
    /// <summary>
    /// Validates that this ITypeRegistrar supports registering by specific instance lifetimes.
    /// </summary>
    /// <param name="registrar">Registrar to check and extend.</param>
    /// <returns>Registrar converted to ISupportLifetimes inteface.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static ISupportLifetimes WithLifetimes(this ITypeRegistrar registrar)
    {
        if (registrar is ISupportLifetimes lifetimes)
            return lifetimes;

        throw new InvalidOperationException("Registrar does not support lifetimes.");
    }
}
