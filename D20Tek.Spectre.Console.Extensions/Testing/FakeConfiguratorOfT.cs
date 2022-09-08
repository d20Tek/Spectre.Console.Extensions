//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Testing
{
    internal class FakeConfigurator<TSettings> : IConfigurator<TSettings>
        where TSettings : CommandSettings
    {
        private readonly CommandMetadata _command;
        private readonly ITypeRegistrar? _registrar;

        public FakeConfigurator(CommandMetadata command, ITypeRegistrar? registrar)
        {
            _command = command;
            _registrar = registrar;
        }

        public void SetDescription(string description)
        {
            _command.Description = description;
        }

        public void AddExample(string[] args)
        {
            _command.Examples.Add(args);
        }

        public void HideBranch()
        {
            _command.IsHidden = true;
        }

        public ICommandConfigurator AddCommand<TCommand>(string name)
            where TCommand : class, ICommandLimiter<TSettings>
        {
            var command = CommandMetadata.FromType<TCommand>(name);
            var configurator = new FakeCommandConfigurator(command);
            _command.Children.Add(command);

            return configurator;
        }

        public ICommandConfigurator AddDelegate<TDerivedSettings>(
            string name, Func<CommandContext, TDerivedSettings, int> func)
            where TDerivedSettings : TSettings
        {
            var command = CommandMetadata.FromDelegate<TDerivedSettings>(
                name,
                (context, settings) => func(context, (TDerivedSettings)settings));
            _command.Children.Add(command);

            return new FakeCommandConfigurator(command);
        }

        public void AddBranch<TDerivedSettings>(
            string name, Action<IConfigurator<TDerivedSettings>> action)
            where TDerivedSettings : TSettings
        {
            var command = CommandMetadata.FromBranch<TDerivedSettings>(name);
            action(new FakeConfigurator<TDerivedSettings>(command, _registrar));

            _command.Children.Add(command);
        }
    }
}
