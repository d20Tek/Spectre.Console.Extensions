//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Testing;

internal class FakeBranchConfigurator : IBranchConfigurator
{
    public IBranchConfigurator WithAlias(string name) => this;
}
