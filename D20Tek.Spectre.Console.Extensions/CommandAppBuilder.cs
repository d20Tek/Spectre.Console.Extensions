//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions;

/// <summary>
/// Builder pattern for creating a CommandApp.
/// </summary>
public class CommandAppBuilder
{
    internal CommandApp? App { get; set; } = null;
    
    internal Action? SetDefaultCommand { get; set; }

    internal ITypeRegistrar? Registrar { get; set; }

    internal StartupBase? Startup { get; set; }

    internal Action? SetCustomConfig { get; set; }

    /// <summary>
    /// Sets up the Startup class to use in this builder.
    /// </summary>
    /// <typeparam name="TStartup">Type of the Startup class in this project,
    /// which must derive from StartupBase.</typeparam>
    /// <returns>Returns the CommandAppBuilder.</returns>
    public CommandAppBuilder WithStartup<TStartup>()
        where TStartup : StartupBase, new()
    {
        // Create the startup class instance from the app project.
        Startup = new TStartup();
        return this;
    }

    /// <summary>
    /// Allows extension methods to set different registrars based on configuration.
    /// </summary>
    /// <param name="registrar">Registrar to set before building.</param>
    /// <returns>Returns the CommandAppBuilder.</returns>
    public CommandAppBuilder SetRegistrar(ITypeRegistrar registrar)
    {
        ArgumentNullException.ThrowIfNull(registrar);
        Registrar = registrar;
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDefault"></typeparam>
    /// <returns></returns>
    public CommandAppBuilder WithDefaultCommand<TDefault>()
        where TDefault : class, ICommand
    {
        SetDefaultCommand = () => App!.SetDefaultCommand<TDefault>();
        return this;
    }

    /// <summary>
    /// Builds the CommandApp encapsulated by this builder. It ensures that the
    /// application services are configured, the CommandApp is created with the
    /// type registrar, and the app Commands are configured.
    /// </summary>
    /// <returns>Returns the CommandAppBuilder.</returns>
    public CommandAppBuilder Build()
    {
        ArgumentNullException.ThrowIfNull(Startup);

        if (Registrar != null)
        {
            // Configure all services with this DI framework.
            Startup.ConfigureServices(Registrar);
        }

        // Create the CommandApp with the type registrar.
        App = new CommandApp(Registrar);
        Registrar?.RegisterInstance(typeof(ICommandApp), App);

        // If a default command was specified, then add it to the CommandApp now.
        SetDefaultCommand?.Invoke();

        // Configure any commands in the application.
        App.Configure(config => Startup.ConfigureCommands(config));

        // Configure any custom set configuration if it's available.
        SetCustomConfig?.Invoke();

        return this;
    }

    /// <summary>
    /// Runs the CommandApp asynchronously.
    /// </summary>
    /// <param name="args">Command line arguments run with.</param>
    /// <returns>Return value from the application.</returns>
    public async Task<int> RunAsync(string[] args)
    {
        ArgumentNullException.ThrowIfNull(App);
        ArgumentNullException.ThrowIfNull(args);

        return await App.RunAsync(args);
    }

    /// <summary>
    /// Runs the CommandApp.
    /// </summary>
    /// <param name="args">Command line arguments run with.</param>
    /// <returns>Return value from the application.</returns>
    public int Run(string[] args)
    {
        ArgumentNullException.ThrowIfNull(App);
        ArgumentNullException.ThrowIfNull(args);

        return App.Run(args);
    }
}
