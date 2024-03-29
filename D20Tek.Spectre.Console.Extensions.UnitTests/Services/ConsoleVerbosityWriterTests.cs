﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Services;
using D20Tek.Spectre.Console.Extensions.Settings;
using D20Tek.Spectre.Console.Extensions.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Services
{
    [TestClass]
    public class ConsoleVerbosityWriterTests
    {
        [TestMethod]
        public void CreateDefault()
        {
            // arrange
            var console = new TestConsole();

            // act
            var writer = new ConsoleVerbosityWriter(console);

            // assert
            Assert.IsNotNull(writer);
            Assert.AreEqual(VerbosityLevel.Normal, writer.Verbosity);
            Assert.AreEqual(string.Empty, console.Output);
        }

        [TestMethod]
        public void MarkupDetailed_VerbosityOff()
        {
            // arrange
            var console = new TestConsole();
            var writer = new ConsoleVerbosityWriter(console)
            {
                Verbosity = VerbosityLevel.N
            };

            // act
            writer.MarkupNormal("normal");
            writer.MarkupDetailed("testing");

            // assert
            StringAssert.Contains(console.Output, "normal");
            Assert.IsFalse(console.Output.Contains("testing"));
        }

        [TestMethod]
        public void MarkupDiagnostic_VerbosityOn()
        {
            // arrange
            var console = new TestConsole();
            var writer = new ConsoleVerbosityWriter(console)
            {
                Verbosity = VerbosityLevel.Diag
            };

            // act
            writer.MarkupSummary("minimal");
            writer.MarkupDiagnostics("testing");

            // assert
            StringAssert.Contains(console.Output, "minimal");
            StringAssert.Contains(console.Output, "testing");
        }

        [TestMethod]
        public void WriteDetailed_VerbosityOff()
        {
            // arrange
            var console = new TestConsole();
            var writer = new ConsoleVerbosityWriter(console);

            // act
            writer.WriteNormal("normal");
            writer.WriteDetailed("testing");

            // assert
            StringAssert.Contains(console.Output, "normal");
            Assert.IsFalse(console.Output.Contains("testing"));
        }


        [TestMethod]
        public void WriteDiagnostic_VerbosityOn()
        {
            // arrange
            var console = new TestConsole();
            var writer = new ConsoleVerbosityWriter(console)
            {
                Verbosity = VerbosityLevel.Diagnostic
            };

            // act
            writer.WriteSummary("minimal");
            writer.WriteDiagnostics("testing");

            // assert
            StringAssert.Contains(console.Output, "minimal");
            StringAssert.Contains(console.Output, "testing");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [ExcludeFromCodeCoverage]
        public void Create_WithNullConsole()
        {
            // arrange

            // act
            _ = new ConsoleVerbosityWriter(null);
        }
    }
}
