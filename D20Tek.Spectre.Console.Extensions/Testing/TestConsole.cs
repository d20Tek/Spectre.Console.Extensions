﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console;
using Spectre.Console.Rendering;

namespace D20Tek.Spectre.Console.Extensions.Testing;

/// <summary>
/// Test console that collects output from CommandApp and can be used
/// to verify output text.
/// </summary>
public class TestConsole : IAnsiConsole, IDisposable
{
    private readonly IAnsiConsole _console;
    private readonly StringWriter _writer;
    private IAnsiConsoleCursor? _cursor;

    /// <inheritdoc/>
    public Profile Profile => _console.Profile;

    /// <inheritdoc/>
    public IExclusivityMode ExclusivityMode => _console.ExclusivityMode;

    /// <inheritdoc/>
    public IAnsiConsoleInput Input => TestInput;

    /// <inheritdoc/>
    public RenderPipeline Pipeline => _console.Pipeline;

    /// <inheritdoc/>
    public IAnsiConsoleCursor Cursor => _cursor ?? _console.Cursor;

    /// <summary>
    /// Gets or sets the test console input.
    /// </summary>
    public TestConsoleInput TestInput { get; set; }

    /// <summary>
    /// Gets the test console output.
    /// </summary>
    public string Output => _writer.ToString();

    /// <summary>
    /// Gets the console output lines.
    /// </summary>
    public IReadOnlyList<string> Lines => Output.TrimEnd('\n').Split(['\n']);

    /// <summary>
    /// Gets or sets a value indicating whether or not VT/ANSI sequences
    /// should be emitted to the console.
    /// </summary>
    public bool EmitAnsiSequences { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestConsole"/> class.
    /// </summary>
    public TestConsole()
    {
        _writer = new StringWriter();
        EmitAnsiSequences = false;
        TestInput = new TestConsoleInput();

        _console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Ansi = AnsiSupport.Yes,
            ColorSystem = (ColorSystemSupport)ColorSystem.TrueColor,
            Out = new AnsiConsoleOutput(_writer),
            Interactive = InteractionSupport.No,
            Enrichment = new ProfileEnrichment
            {
                UseDefaultEnrichers = false,
            },
        });

        _console.Profile.Width = 80;
        _console.Profile.Height = 24;
        _console.Profile.Capabilities.Ansi = true;
        _console.Profile.Capabilities.Unicode = true;
    }

    private bool isDisposed = false;

    /// <inheritdoc/>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        if (isDisposed == false)
        {
            _writer.Dispose();
            isDisposed = true;
        }
    }

    /// <inheritdoc/>
    public void Clear(bool home) => _console.Clear(home);

    /// <inheritdoc/>
    public void Write(IRenderable renderable)
    {
        if (EmitAnsiSequences)
        {
            _console.Write(renderable);
        }
        else
        {
            foreach (var segment in renderable.GetSegments(this))
            {
                if (segment.IsControlCode is false)
                {
                    Profile.Out.Writer.Write(segment.Text);
                }
            }
        }
    }

    internal void SetCursor(IAnsiConsoleCursor? cursor) => _cursor = cursor;
}
