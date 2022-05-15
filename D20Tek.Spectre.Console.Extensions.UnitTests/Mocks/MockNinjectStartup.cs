//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Moq;
using Ninject;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Mocks
{
    internal class MockNinjectStartup : StartupBase<StandardKernel>
    {
        public override ITypeRegistrar ConfigureServices(StandardKernel kernel)
        {
            return new Mock<ITypeRegistrar>().Object;
        }

        public override IConfigurator ConfigureCommands(IConfigurator config)
        {
            return config;
        }
    }
}
