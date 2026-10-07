using D20Tek.Spectre.Console.Extensions.Controls;
using Spectre.Console;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Commands;

/// <summary>
/// Base class for building interactive command prompts that is able to execute
/// other commands configured with the ICommandApp. This allows us to reuse Commands
/// in interactive prompt mode as well as independently run commands.
/// </summary>
public abstract class InteractiveCommandBase : AsyncCommand
{
    private readonly IAnsiConsole _console;
    private readonly ICommandApp _commandApp;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="app">ICommandApp for this application.</param>
    /// <param name="console">Console to write into.</param>
    public InteractiveCommandBase(ICommandApp app, IAnsiConsole console)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(console);

        _commandApp = app;
        _console = console;
    }

    /// <inheritdoc />
    public override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellation)
    {
        ShowWelcomeMessage(_console);
        var historyPrompt = new HistoryTextPrompt<string>(GetAppPromptPrefix());

        while (true)
        {
            _console.WriteLine();
            var commandText = _console.Prompt(historyPrompt);

            if (IsExitCommand(commandText))
            {
                ShowExitMessage(_console);
                return Constants.S_OK;
            }

            if (commandText.Equals(Constants.StartCommand, StringComparison.OrdinalIgnoreCase))
                continue;

            var args = CliParser.SplitCommandLine(commandText);
            var result = await _commandApp.RunAsync(args);

            if (CanContinue(result) is false)
            {
                return result;
            }
        }
    }

    /// <summary>
    /// Override this method to use the IAnsiConsole to display any welcome message for the app.
    /// </summary>
    /// <param name="console">Console to write to.</param>
    protected abstract void ShowWelcomeMessage(IAnsiConsole console);

    /// <summary>
    /// Gets the application prompt to use in the command prompt display.
    /// </summary>
    /// <returns>String for the prompt prefix.</returns>
    protected virtual string GetAppPromptPrefix() => Constants.DefaultPromptPrefix;

    /// <summary>
    /// Tests whether the commandText is requesting to exit the interactive command prompt.
    /// The default implementation is for (exit or x), but can be overridden.
    /// </summary>
    /// <param name="commandText">Command text to check.</param>
    /// <returns>True means commandText should exit; false means command processing can continue.</returns>
    protected virtual bool IsExitCommand(string commandText) =>
        commandText.Equals(Constants.ExitCommand, StringComparison.OrdinalIgnoreCase) ||
        commandText.Equals(Constants.ExitCommandAbbr, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Shows a message when the application exits successfully.
    /// </summary>
    /// <param name="console">IAnsiConsole to use for output.</param>
    protected virtual void ShowExitMessage(IAnsiConsole console) { }

    /// <summary>
    /// Called after processing a Command, it uses the result value to decide whether the
    /// interactive prompt is allowed to contine. Defaults to always continue.
    /// </summary>
    /// <param name="result">Result from command run.</param>
    /// <returns>
    ///     True means continue in the interactive prompt; false means exit immediately 
    ///     with the error result code.
    /// </returns>
    protected virtual bool CanContinue(int result) => true;
}
