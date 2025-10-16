//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing;

[TestClass]
public class CommandMetadataTests
{
    [TestMethod]
    public void Create_FromBranchUntyped()
    {
        // arrange

        // act
        var m = CommandMetadata.FromBranch(typeof(EmptyCommandSettings), "test-untyped");

        // assert
        Assert.AreEqual("test-untyped", m.Name);
        Assert.AreEqual(typeof(EmptyCommandSettings), m.SettingsType);
        Assert.IsNull(m.AsyncDelegate);
    }

    [TestMethod]
    public void Create_FromType_WithSettings()
    {
        // arrange

        // act
        var m = CommandMetadata.FromType<MockCommandWithSettings>("test-settings");

        // assert
        Assert.AreEqual("test-settings", m.Name);
        Assert.AreEqual(typeof(MockCommandWithSettings), m.CommandType);
        Assert.AreEqual(typeof(MockCommandWithSettings.MockSettings), m.SettingsType);
    }

    internal interface IMyCommand : ICommand
    {
    }

    [ExcludeFromCodeCoverage]
    internal class MyCommand : IMyCommand
    {
        public Task<int> Execute(CommandContext context, CommandSettings settings)
        {
            throw new NotImplementedException();
        }

        public ValidationResult Validate(CommandContext context, CommandSettings settings)
        {
            throw new NotImplementedException();
        }
    }

    [TestMethod]
    public void Create_FromDerivedType()
    {
        // arrange

        // act
        var m = CommandMetadata.FromType<MyCommand>("test-my");

        // assert
        Assert.AreEqual("test-my", m.Name);
        Assert.AreEqual(typeof(MyCommand), m.CommandType);
        Assert.IsNull(m.SettingsType);
    }
}
