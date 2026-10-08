//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Spectre.Console.Extensions.UnitTests.Controls;

public abstract class PathPromptTestsBase
{
    protected string _tempDirectory = string.Empty;

    [TestInitialize]
    public void TestInitialize()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), $"pathprompttests{Guid.NewGuid():N}");
        Directory.CreateDirectory(_tempDirectory);
        Directory.CreateDirectory(Path.Combine(_tempDirectory, "subfolder"));
        Directory.CreateDirectory(Path.Combine(_tempDirectory, ".hiddenfolder"));
        File.WriteAllText(Path.Combine(_tempDirectory, "notes.txt"), "sample");
        File.WriteAllText(Path.Combine(_tempDirectory, "data.json"), "{}");
    }

    [TestCleanup]
    public void TestCleanup()
    {
        if (Directory.Exists(_tempDirectory))
        {
            Directory.Delete(_tempDirectory, recursive: true);
        }
    }
}
