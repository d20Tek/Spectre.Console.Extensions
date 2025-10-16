//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;

internal class MockStartupWithExceptionCommand : StartupBase
{
    public override void ConfigureServices(ITypeRegistrar registrar)
    {
        registrar.Register(typeof(IMockService), typeof(MockService));
    }

    public override IConfigurator ConfigureCommands(IConfigurator config)
    {
        config.CaseSensitivity(CaseSensitivity.None);
        config.SetApplicationName("mock-command");
        config.ValidateExamples();

        config.AddCommand<MockCommandWithException>("mock")
              .WithAlias("moc")
              .WithDescription("Default command for mock tests.")
              .WithExample(["mock"]);

        return config;
    }
}
