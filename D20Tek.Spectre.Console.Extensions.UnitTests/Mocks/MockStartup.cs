//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Mocks
{
    internal class MockStartup : StartupBase
    {
        public override void ConfigureServices(ITypeRegistrar registrar)
        {
            registrar.Register(typeof(IMockService), typeof(MockService));
        }

        public override IConfigurator ConfigureCommands(IConfigurator config)
        {
            return config;
        }
    }
}
