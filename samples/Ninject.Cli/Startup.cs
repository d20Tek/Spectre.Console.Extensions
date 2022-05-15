//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Samples.Common.Commands;
using D20Tek.Samples.Common.Services;
using D20Tek.Spectre.Console.Extensions;
using D20Tek.Spectre.Console.Extensions.Injection;
using Spectre.Console.Cli;

namespace Ninject.Cli
{
    internal class Startup : StartupBase<StandardKernel>
    {
        public override ITypeRegistrar ConfigureServices(StandardKernel kernel)
        {
            // Create a type registrar and register any dependencies.
            // A type registrar is an adapter for a DI framework.

            // register services here...
            kernel.Bind<IDisplayWriter>().To<ConsoleDisplayWriter>();

            return new NinjectTypeRegistrar(kernel);
        }

        public override IConfigurator ConfigureCommands(IConfigurator config)
        {
            config.CaseSensitivity(CaseSensitivity.None);
            config.SetApplicationName("Ninject.Cli");
            config.ValidateExamples();

            config.AddCommand<DefaultCommand>("default")
                .WithDescription("Default command that displays some text.")
                .WithExample(new[] { "default", "--verbose", "high" });

            return config;
        }
    }
}
