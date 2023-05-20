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

        public bool ShowOptionDefaultValues { get; set; }
        
        public bool TrimTrailingPeriod { get; set; }
        
        public bool ConvertFlagsToRemainingArguments { get; set; }

        public FakeCommandAppSettings(ITypeRegistrar registrar)
        {
            Registrar = new FrontendTypeRegistrar(registrar);
            CaseSensitivity = CaseSensitivity.All;
        }
    }
}
