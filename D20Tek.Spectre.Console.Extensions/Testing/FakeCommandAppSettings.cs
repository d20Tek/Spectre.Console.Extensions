//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;
using System.Globalization;

namespace D20Tek.Spectre.Console.Extensions.Testing;

internal class FakeCommandAppSettings(ITypeRegistrar registrar) : ICommandAppSettings
{
    public string? ApplicationName { get; set; }

    public string? ApplicationVersion { get; set; }
    
    public IAnsiConsole? Console { get; set; }
    
    public ICommandInterceptor? Interceptor { get; set; }

    public ITypeRegistrarFrontend Registrar { get; } = new FrontendTypeRegistrar(registrar);

    public CaseSensitivity CaseSensitivity { get; set; } = CaseSensitivity.All;

    public bool PropagateExceptions { get; set; }
    
    public bool ValidateExamples { get; set; }
    
    public bool StrictParsing { get; set; }

    public Func<Exception, int>? ExceptionHandler { get; set; }

    public bool ShowOptionDefaultValues { get; set; }
    
    public bool TrimTrailingPeriod { get; set; }
    
    public bool ConvertFlagsToRemainingArguments { get; set; }

    public CultureInfo? Culture { get; set; }
    
    public int MaximumIndirectExamples { get; set; }
    
    public HelpProviderStyle? HelpProviderStyles { get; set; }
    
    Func<Exception, ITypeResolver?, int>? ICommandAppSettings.ExceptionHandler { get; set; }
}
