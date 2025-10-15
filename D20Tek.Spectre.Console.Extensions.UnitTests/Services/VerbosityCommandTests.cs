//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Services;
using D20Tek.Spectre.Console.Extensions.Testing;
using D20Tek.Spectre.Console.Extensions.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Services
{
    [TestClass]
    public class VerbosityCommandTests
    {
        [TestMethod]
        [DataRow("Quiet", "", DisplayName = "Quiet")]
        [DataRow("Q", "", DisplayName = "Q")]
        [DataRow("quiet", "", DisplayName = "quiet")]
        [DataRow("q", "", DisplayName = "q")]
        [DataRow("Minimal", "Minimal\n", DisplayName = "Minimal")]
        [DataRow("M", "Minimal\n", DisplayName = "M")]
        [DataRow("minimal", "Minimal\n", DisplayName = "minimal")]
        [DataRow("m", "Minimal\n", DisplayName = "m")]
        [DataRow("Normal", "Minimal\nNormal\n", DisplayName = "Normal")]
        [DataRow("normal", "Minimal\nNormal\n", DisplayName = "normal")]
        [DataRow("N", "Minimal\nNormal\n", DisplayName = "N")]
        [DataRow("n", "Minimal\nNormal\n", DisplayName = "n")]
        [DataRow("Detailed", "Minimal\nNormal\nDetailed\n", DisplayName = "Detailed")]
        [DataRow("detailed", "Minimal\nNormal\nDetailed\n", DisplayName = "detailed")]
        [DataRow("D", "Minimal\nNormal\nDetailed\n", DisplayName = "D")]
        [DataRow("d", "Minimal\nNormal\nDetailed\n", DisplayName = "d")]
        [DataRow("Diagnostic", "Minimal\nNormal\nDetailed\nDiagnostics\n", DisplayName = "Diagnostic")]
        [DataRow("diagnostic", "Minimal\nNormal\nDetailed\nDiagnostics\n", DisplayName = "diagnostic")]
        [DataRow("Diag", "Minimal\nNormal\nDetailed\nDiagnostics\n", DisplayName = "Diag")]
        [DataRow("diag", "Minimal\nNormal\nDetailed\nDiagnostics\n", DisplayName = "diag")]
        public void ExecuteWriteCommand(string verbosity, string expectedOutput)
        {
            // arrange
            var context = new CommandAppTestContext();
            context.Registrar.WithConsoleVerbosityWriter();
            context.Configure(c => c.AddCommand<MockVerbosityCommand>("speak"));

            var args = new string[]
            {
                "speak",
                "-v",
                verbosity,
            };

            // act
            var result = context.Run(args);

            // assert
            Assert.AreEqual(0, result.ExitCode);
            Assert.AreEqual(expectedOutput, result.Output);
        }
    }
}
