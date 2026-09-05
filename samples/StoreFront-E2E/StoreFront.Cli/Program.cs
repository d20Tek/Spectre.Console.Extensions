//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using StoreFront.Cli;
using System.Diagnostics.CodeAnalysis;

// The full builder pipeline lives in StoreApp.RunAsync so that the end-to-end tests can
// invoke the exact same composition through CommandAppE2ERunner.
return await StoreApp.RunAsync(args);

// Top-level statements compile into a generated Program class. This partial declaration
// applies ExcludeFromCodeCoverage so the entry-point shim is not counted in coverage.
[ExcludeFromCodeCoverage]
public partial class Program
{
}


