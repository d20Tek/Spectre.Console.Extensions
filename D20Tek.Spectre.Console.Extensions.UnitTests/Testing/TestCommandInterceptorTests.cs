//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Spectre.Console.Cli;
using System;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Testing
{
    [TestClass]
    public class TestCommandInterceptorTests
    {
        private readonly CommandContext _context;
        private readonly CommandSettings _settings;

        public TestCommandInterceptorTests()
        {
            var remaining = new Mock<IRemainingArguments>().Object;
            _context = new CommandContext(remaining, "test", null);
            _settings = new EmptyCommandSettings();
        }

        [TestMethod]
        public void Create()
        {
            // arrange

            // act
            var i = new TestCommandInterceptor();

            // assert
            Assert.IsNotNull(i);
            Assert.IsNull(i.Context);
            Assert.IsNull(i.Settings);
        }

        [TestMethod]
        public void Intercept()
        {
            // arrange
            var i = new TestCommandInterceptor();

            // act
            i.Intercept(_context, _settings);

            // assert
            Assert.IsNotNull(i);
            Assert.AreEqual(_context, i.Context);
            Assert.AreEqual(_settings, i.Settings);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Intercept_WithNullContext()
        {
            // arrange
            var i = new TestCommandInterceptor();

            // act
            i.Intercept(null, _settings);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Intercept_WithNullSettings()
        {
            // arrange
            var i = new TestCommandInterceptor();

            // act
            i.Intercept(_context, null);
        }
    }
}
