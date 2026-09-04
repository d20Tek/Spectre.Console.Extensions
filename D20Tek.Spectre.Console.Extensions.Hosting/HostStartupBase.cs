//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Hosting;

/// <summary>
/// Abstract base class for a host-aware startup that splits its two responsibilities across the
/// Generic Host lifecycle: <see cref="ConfigureServices"/> runs before the host is built (against
/// the host's <see cref="IServiceCollection"/>), and <see cref="ConfigureCommands"/> runs after
/// the host is built (against the Spectre.Console.Cli <see cref="IConfigurator"/>).
/// </summary>
/// <remarks>
/// This is the host-model counterpart to the core library's StartupBase. The core StartupBase
/// configures services through an <see cref="ITypeRegistrar"/>, which does not fit the Generic
/// Host because the host owns the container and it is immutable once built. <see cref="HostStartupBase"/>
/// instead configures services directly on the <see cref="IServiceCollection"/> during the
/// pre-build phase, so registrations participate in the host provider like any other service.
/// </remarks>
public abstract class HostStartupBase
{
    /// <summary>
    /// Override this method to register application services on the host's service collection.
    /// This runs before the host is built.
    /// </summary>
    /// <param name="services">The host service collection to add registrations to.</param>
    public abstract void ConfigureServices(IServiceCollection services);

    /// <summary>
    /// Override this method to configure the console commands for this application. This runs
    /// after the host is built, when the CommandApp is created.
    /// </summary>
    /// <param name="config">The configurator to use.</param>
    /// <returns>The configurator that was used.</returns>
    public abstract IConfigurator ConfigureCommands(IConfigurator config);
}
