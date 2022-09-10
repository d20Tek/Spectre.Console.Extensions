//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Services
{
    /// <summary>
    /// Extension methods to register services from this library.
    /// </summary>
    public static class TypeRegistrarExtensions
    {
        /// <summary>
        /// Registers the IVerbosityWriter service with the current registrar.
        /// </summary>
        /// <param name="registrar">Registrar to initialize.</param>
        public static void WithConsoleVerbosityWriter(this ITypeRegistrar registrar)
        {
            registrar.Register(typeof(IVerbosityWriter), typeof(ConsoleVerbosityWriter));
        }
    }
}
