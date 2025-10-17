//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Testing;

internal class FakeCommandConfigurator(CommandMetadata command) : ICommandConfigurator
{
    public CommandMetadata Command { get; } = command;

    public ICommandConfigurator WithExample(string[] args)
    {
        Command.Examples.Add(args);
        return this;
    }

    public ICommandConfigurator WithAlias(string alias)
    {
        Command.Aliases.Add(alias);
        return this;
    }

    public ICommandConfigurator WithDescription(string description)
    {
        Command.Description = description;
        return this;
    }

    public ICommandConfigurator WithData(object data)
    {
        Command.Data = data;
        return this;
    }

    public ICommandConfigurator IsHidden()
    {
        Command.IsHidden = true;
        return this;
    }
}
