//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Injection;
using D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace D20Tek.Spectre.Console.Extensions.UnitTests
{
    [TestClass]
    public class CommandAppBuilderTests
    {
        [TestMethod]
        public void Constructor()
        {
            // arrange

            // act
            var builder = new CommandAppBuilder();

            // assert
            Assert.IsNotNull(builder);
            Assert.IsNull(builder.Registrar);
            Assert.IsNull(builder.Startup);
        }

        [TestMethod]
        public void WithStartup()
        {
            // arrange
            var builder = new CommandAppBuilder();

            // act
            builder.WithStartup<MockStartup>();

            // assert
            Assert.IsNotNull(builder);
            Assert.IsNull(builder.Registrar);
            Assert.IsNotNull(builder.Startup);
        }
    }
}
