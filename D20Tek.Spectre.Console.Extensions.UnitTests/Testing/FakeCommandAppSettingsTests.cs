using D20Tek.Spectre.Console.Extensions.Injection;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console.Cli.Help;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing;

[TestClass]
public class FakeCommandAppSettingsTests
{
    [TestMethod]
    public void Create_WithProperties()
    {
        // arrange
        var registrar = new DependencyInjectionTypeRegistrar(new ServiceCollection());
        var interceptor = new TestCommandInterceptor();
        var helpStyle = new HelpProviderStyle();

        // act
        var appSettings = new FakeCommandAppSettings(registrar)
        {
            Interceptor = interceptor,
            ExceptionHandler = [ExcludeFromCodeCoverage] (ex) => ex.HResult,
            ShowOptionDefaultValues = true,
            TrimTrailingPeriod = true,
            ConvertFlagsToRemainingArguments = true,
            Culture = System.Globalization.CultureInfo.CurrentUICulture,
            MaximumIndirectExamples = 5,
            HelpProviderStyles = helpStyle,
            CancellationExitCode = 0,
        };

        // assert
        Assert.IsNotNull(appSettings);
        Assert.AreEqual(interceptor, appSettings.Interceptor);
        Assert.IsNotNull(appSettings.ExceptionHandler);
        Assert.IsTrue(appSettings.ShowOptionDefaultValues);
        Assert.IsTrue(appSettings.TrimTrailingPeriod);
        Assert.IsTrue(appSettings.ConvertFlagsToRemainingArguments);
        Assert.AreEqual(System.Globalization.CultureInfo.CurrentUICulture, appSettings.Culture);
        Assert.AreEqual(5, appSettings.MaximumIndirectExamples);
        Assert.AreEqual(helpStyle, appSettings.HelpProviderStyles);
        Assert.AreEqual(0, appSettings.CancellationExitCode);
    }
}
