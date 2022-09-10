//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Settings
{
    [TestClass]
    public class VerbositySettingsTests
    {
        [TestMethod]
        public void CreateDefault()
        {
            // arrange

            // act
            var settings = new VerbositySettings();

            // assert
            Assert.IsNotNull(settings);
            Assert.AreEqual(VerbosityLevel.Normal, settings.Verbosity);
        }

        [TestMethod]
        public void CreateMinimal()
        {
            // arrange

            // act
            var settings = new VerbositySettings { Verbosity = VerbosityLevel.Minimal };

            // assert
            Assert.IsNotNull(settings);
            Assert.AreEqual(VerbosityLevel.Minimal, settings.Verbosity);
        }
    }
}
