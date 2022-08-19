//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Injection;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Testing
{
    /// <summary>
    /// An appropriate TestContext for testing CommandApp configuration, startup, and type registration.
    /// These services can be configured with Mock or test classes. Then, specific commands can be
    /// run to test their behavior.
    /// </summary>
    public class CommandAppTestContext
    {
        /// <summary>
        /// Gets the TypeRegistrar for this test context.
        /// </summary>
        public ITypeRegistrar Registrar { get; }

        /// <summary>
        /// Gets the TypeResolver for this test context.
        /// </summary>
        public ITypeResolver Resolver => Registrar.Build();

        /// <summary>
        /// Gets the command configurator for this test context.
        /// </summary>
        public ITestConfigurator Configurator { get; }

        /// <summary>
        /// Default constructor that initialize this TestContext with a default TypeRegistrar
        /// and a fake IConfigurator implemenation for testing and validation purposed.
        /// </summary>
        public CommandAppTestContext()
        {
            Registrar = new DependencyInjectionTypeRegistrar(new ServiceCollection());
            Configurator = new FakeConfigurator(Registrar);
        }
    }
}
