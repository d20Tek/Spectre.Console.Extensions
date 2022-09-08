//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Testing
{
    internal class FakeConfigurator : ITestConfigurator
    {
        private const string _defaultCommandName = "__default_command";
        private readonly ITypeRegistrar _registrar;

        public IList<CommandMetadata> Commands { get; }

        public ICommandAppSettings Settings { get; }
        
        public CommandMetadata? DefaultCommand { get; private set; }
        
        public IList<string[]> Examples { get; }

        ICommandAppSettings IConfigurator.Settings => Settings;

        public FakeConfigurator(ITypeRegistrar registrar)
        {
            _registrar = registrar;

            Commands = new List<CommandMetadata>();
            Settings = new FakeCommandAppSettings(registrar);
            Examples = new List<string[]>();
        }

        public void AddExample(string[] args)
        {
            Examples.Add(args);
        }

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

        public ICommandConfigurator AddDelegate<TSettings>(string name, Func<CommandContext, TSettings, int> func)
            where TSettings : CommandSettings
        {
            var command = CommandMetadata.FromDelegate<TSettings>(
                name,
                (context, settings) => func(context, (TSettings)settings));
            Commands.Add(command);

            return new FakeCommandConfigurator(command);
        }

        public void AddBranch<TSettings>(string name, Action<IConfigurator<TSettings>> action)
            where TSettings : CommandSettings
        {
            var command = CommandMetadata.FromBranch<TSettings>(name);
            action(new FakeConfigurator<TSettings>(command, _registrar));

            Commands.Add(command);
        }
    }
}
