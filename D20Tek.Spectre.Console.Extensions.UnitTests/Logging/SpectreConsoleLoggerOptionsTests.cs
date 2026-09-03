//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Logging;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Logging;

[TestClass]
public class SpectreConsoleLoggerOptionsTests
{
    [TestMethod]
    public void Constructor_WithDefaults_SetsExpectedValues()
    {
        // Arrange

        // Act
        var options = new SpectreConsoleLoggerOptions();

        // Assert
        Assert.IsTrue(options.IncludeLevelLabel);
        Assert.IsFalse(options.IncludeCategory);
        Assert.IsFalse(options.IncludeTimestamp);
        Assert.AreEqual("HH:mm:ss", options.TimestampFormat);
    }

    [TestMethod]
    public void Properties_WhenSet_RetainValues()
    {
        // Arrange
        var options = new SpectreConsoleLoggerOptions
        {
            // Act
            IncludeLevelLabel = false,
            IncludeCategory = true,
            IncludeTimestamp = true,
            TimestampFormat = "yyyy-MM-dd"
        };

        // Assert
        Assert.IsFalse(options.IncludeLevelLabel);
        Assert.IsTrue(options.IncludeCategory);
        Assert.IsTrue(options.IncludeTimestamp);
        Assert.AreEqual("yyyy-MM-dd", options.TimestampFormat);
    }
}
