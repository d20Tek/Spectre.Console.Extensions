//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Mocks
{
    internal class MockStartup : StartupBase
    {
        public override ITypeRegistrar ConfigureServices(IServiceCollection services)
        {
            return new Mock<ITypeRegistrar>().Object;
        }

        public override IConfigurator ConfigureCommands(IConfigurator config)
        {
            return config;
        }
    }
}
