//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;

namespace D20Tek.Spectre.Console.Extensions.Testing;

internal class FakeConfigurator(ITypeRegistrar registrar) : ITestConfigurator
{
    private const string _defaultCommandName = "__default_command";
    private readonly ITypeRegistrar _registrar = registrar;

    public IList<CommandMetadata> Commands { get; } = [];

    public ICommandAppSettings Settings { get; } = new FakeCommandAppSettings(registrar);

    public CommandMetadata? DefaultCommand { get; private set; }

    public IList<string[]> Examples { get; } = [];

    public IHelpProvider? HelperProvider { get; private set; } = null;

    ICommandAppSettings IConfigurator.Settings => Settings;

    public void SetDefaultCommand<TDefaultCommand>()
        where TDefaultCommand : class, ICommand
    {
        DefaultCommand = CommandMetadata.FromType<TDefaultCommand>(
            _defaultCommandName, true);
    }

    public ICommandConfigurator AddCommand<TCommand>(string name)
        where TCommand : class, ICommand
    {
        var command = CommandMetadata.FromType<TCommand>(name, false);
        Commands.Add(command);

        return new FakeCommandConfigurator(command);
    }

    public ICommandConfigurator AddDelegate<TSettings>(
        string name,
        Func<CommandContext, TSettings, CancellationToken, int> func)
        where TSettings : CommandSettings
    {
        var command = CommandMetadata.FromDelegate<TSettings>(
            name,
            (context, settings) => func(context, (TSettings)settings, CancellationToken.None));
        Commands.Add(command);

        return new FakeCommandConfigurator(command);
    }

    public IBranchConfigurator AddBranch<TSettings>(string name, Action<IConfigurator<TSettings>> action)
        where TSettings : CommandSettings
    {
        var command = CommandMetadata.FromBranch<TSettings>(name);
        action(new FakeConfigurator<TSettings>(command, _registrar));

        Commands.Add(command);

        return new FakeBranchConfigurator();
    }

    public ICommandConfigurator AddAsyncDelegate<TSettings>(string name, Func<CommandContext, TSettings, CancellationToken, Task<int>> func)
        where TSettings : CommandSettings
    {
        var command = CommandMetadata.FromAsyncDelegate<TSettings>(
            name,
            (context, settings) => func(context, (TSettings)settings, CancellationToken.None));
        Commands.Add(command);

        return new FakeCommandConfigurator(command);
    }

    public IConfigurator SetHelpProvider(IHelpProvider helpProvider)
    {
        HelperProvider = helpProvider;
        return this;
    }

    public IConfigurator SetHelpProvider<T>() where T : IHelpProvider
    {
        HelperProvider = Activator.CreateInstance<T>();
        return this;
    }

    public IConfigurator AddExample(params string[] args)
    {
        Examples.Add(args);
        return this;
    }
}
