//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console.Cli;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing;

[TestClass]
public class CommandConfigurationTestContextTests
{
    [TestMethod]
    public void CreateDefault()
    {
        // arrange

        // act 
        var context = new CommandConfigurationTestContext();

        // assert
        Assert.IsNotNull(context);
        Assert.IsNotNull(context.Registrar);
        Assert.IsNotNull(context.Resolver);
        Assert.IsNotNull(context.Configurator);
        Assert.IsInstanceOfType(context.Registrar, typeof(ITypeRegistrar));
        Assert.IsInstanceOfType(context.Configurator, typeof(IConfigurator));
    }

    [TestMethod]
    public void ConfigureServices()
    {
        // arrange
        var context = new CommandConfigurationTestContext();
        var startup = new MockStartupWithConfig();

        // act
        startup.ConfigureServices(context.Registrar);

        // assert
        Assert.IsInstanceOfType<MockService>(context.Resolver.Resolve(typeof(IMockService)));
    }

    [TestMethod]
    public void ConfigureCommands()
    {
        // arrange
        var context = new CommandConfigurationTestContext();
        var startup = new MockStartupWithConfig();

        // act
        var result = startup.ConfigureCommands(context.Configurator);

        // assert
        Assert.AreEqual(context.Configurator, result);
        Assert.AreEqual("mock-command", context.Configurator.Settings.ApplicationName);
        Assert.AreEqual(1, context.Configurator.Commands.Count());
        Assert.AreEqual("mock", context.Configurator.Commands.First().Name);
        Assert.HasCount(1, context.Configurator.Commands.First().Aliases);
        Assert.HasCount(1, context.Configurator.Commands.First().Examples);
        Assert.IsEmpty(context.Configurator.Examples);
    }

    [TestMethod]
    public void Configurator_WithFullMetadata()
    {
        // arrange
        var context = new CommandConfigurationTestContext();

        // act
        context.Configurator.AddCommand<MockCommand>("test")
                            .WithAlias("1")
                            .WithData("data")
                            .WithExample(new string[] { "test" })
                            .WithDescription("description");

        // assert
        Assert.AreEqual(1, context.Configurator.Commands.Count());
        var command = context.Configurator.Commands.First();
        Assert.AreEqual("test", command.Name);
        Assert.AreEqual("description", command.Description);
        Assert.AreEqual("data", command.Data);
        Assert.HasCount(1, command.Aliases);
        Assert.HasCount(1, command.Examples);
        Assert.AreEqual(typeof(MockCommand), command.CommandType);
        Assert.AreEqual(typeof(EmptyCommandSettings), command.SettingsType);
        Assert.IsFalse(command.IsHidden);
        Assert.IsFalse(command.IsDefaultCommand);
        Assert.IsNull(command.Delegate);
    }

    [TestMethod]
    public void Configurator_WithHiddenCommand()
    {
        // arrange
        var context = new CommandConfigurationTestContext();

        // act
        context.Configurator.AddCommand<MockCommand>("test")
                            .IsHidden();

        // assert
        Assert.HasCount(1, context.Configurator.Commands);
        var command = context.Configurator.Commands.First();
        Assert.AreEqual("test", command.Name);
        Assert.IsTrue(command.IsHidden);
    }

    [TestMethod]
    public void Configurator_AddExample()
    {
        // arrange
        var context = new CommandConfigurationTestContext();

        // act
        context.Configurator.AddExample(["test"]);

        // assert
        Assert.HasCount(1, context.Configurator.Examples);
    }

    [TestMethod]
    public void Configurator_SetDefaultCommand()
    {
        // arrange
        var context = new CommandConfigurationTestContext();
        FakeConfigurator config = (FakeConfigurator)context.Configurator;

        // act
        config.SetDefaultCommand<MockCommand>();

        // assert
        var result = context.Configurator.DefaultCommand;
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsDefaultCommand);
        Assert.AreEqual("__default_command", result.Name);
    }

    [TestMethod]
    public void Configurator_WithBranch()
    {
        // arrange
        var context = new CommandConfigurationTestContext();

        // act
        context.Configurator.AddBranch<EmptyCommandSettings>("branch1", c => c.AddCommand<MockCommand>("test"));

        // assert
        Assert.HasCount(1, context.Configurator.Commands);
        Assert.AreEqual("branch1", context.Configurator.Commands.First().Name);
        Assert.HasCount(0, context.Configurator.Commands.First().Aliases);
        Assert.HasCount(0, context.Configurator.Commands.First().Examples);
        Assert.IsEmpty(context.Configurator.Examples);
        Assert.AreEqual("test", context.Configurator.Commands.First().Children.First().Name);
    }


    [TestMethod]
    public void Configurator_WithBranch2()
    {
        // arrange
        var context = new CommandConfigurationTestContext();

        // act
        context.Configurator.AddBranch("branch1", c => c.AddCommand<MockCommand>("test"));

        // assert
        Assert.HasCount(1, context.Configurator.Commands);
        Assert.AreEqual("branch1", context.Configurator.Commands.First().Name);
        Assert.HasCount(0, context.Configurator.Commands.First().Aliases);
        Assert.HasCount(0, context.Configurator.Commands.First().Examples);
        Assert.IsEmpty(context.Configurator.Examples);
        Assert.AreEqual("test", context.Configurator.Commands.First().Children.First().Name);
    }

    [TestMethod]
    public void Configurator_WithDelegate()
    {
        // arrange
        var context = new CommandConfigurationTestContext();

        // act
        context.Configurator.AddDelegate<EmptyCommandSettings>("test-delegate", DelegateCall);

        // assert
        Assert.HasCount(1, context.Configurator.Commands);
        Assert.AreEqual("test-delegate", context.Configurator.Commands.First().Name);
    }

    [ExcludeFromCodeCoverage]
    private int DelegateCall(CommandContext context, EmptyCommandSettings settings) => 0;

    [TestMethod]
    public void Configurator_WithCommandAppSettings()
    {
        // arrange
        var context = new CommandConfigurationTestContext();
        var console = new TestConsole();
        var interceptor = new TestCommandInterceptor();

        // act
        context.Configurator.Settings.ApplicationName = "test-app";
        context.Configurator.Settings.ApplicationVersion = "1.0.0";
        context.Configurator.Settings.Console = console;
        context.Configurator.Settings.StrictParsing = true;
        context.Configurator.Settings.CaseSensitivity = CaseSensitivity.None;
        context.Configurator.Settings.PropagateExceptions = true;
        context.Configurator.Settings.ValidateExamples = true;
        context.Configurator.Settings.ExceptionHandler = HandlerMethod;

        // assert
        Assert.AreEqual("test-app", context.Configurator.Settings.ApplicationName);
        Assert.AreEqual("1.0.0", context.Configurator.Settings.ApplicationVersion);
        Assert.AreEqual(console, context.Configurator.Settings.Console);
        Assert.AreEqual(CaseSensitivity.None, context.Configurator.Settings.CaseSensitivity);
        Assert.IsTrue(context.Configurator.Settings.StrictParsing);
        Assert.IsTrue(context.Configurator.Settings.PropagateExceptions);
        Assert.IsTrue(context.Configurator.Settings.ValidateExamples);
        Assert.IsNotNull(context.Configurator.Settings.ExceptionHandler);
        Assert.IsNotNull(context.Configurator.Settings.Registrar);
    }

    [ExcludeFromCodeCoverage]
    private int HandlerMethod(Exception ex, ITypeResolver resolver) => 0;
}
