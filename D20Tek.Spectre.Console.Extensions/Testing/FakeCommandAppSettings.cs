//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Testing
{
    internal class FakeCommandAppSettings : ICommandAppSettings
    {
        public string? ApplicationName { get; set; }

        public string? ApplicationVersion { get; set; }
        
        public IAnsiConsole? Console { get; set; }
        
        public ICommandInterceptor? Interceptor { get; set; }
        
        public ITypeRegistrarFrontend Registrar { get; set; }
        
        public CaseSensitivity CaseSensitivity { get; set; }
        
        public bool PropagateExceptions { get; set; }
        
        public bool ValidateExamples { get; set; }
        
        public bool StrictParsing { get; set; }

        public Func<Exception, int>? ExceptionHandler { get; set; }

        public FakeCommandAppSettings(ITypeRegistrar registrar)
        {
            Registrar = new FrontendTypeRegistrar(registrar);
            CaseSensitivity = CaseSensitivity.All;
        }

        public bool IsTrue(Func<ICommandAppSettings, bool> func, string environmentVariableName)
        {
            if (func(this))
            {
                return true;
            }

            var environmentVariable = Environment.GetEnvironmentVariable(environmentVariableName);
            if (!string.IsNullOrWhiteSpace(environmentVariable))
            {
                if (environmentVariable.Equals("True", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
