using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal static class AnsiConsoleExtensions
{
    private static readonly List<IInputStateHandler> _handlers =
    [
        new EnterHandler(),
        new AutoCompleteHandler(),
        new BackspaceHandler(),
        new InsertHandler(),
        new LeftArrowHandler(),
        new RightArrowHandler(),
        new UpArrowHandler(),
        new DownArrowHandler(),
        new DefaultInputHandler(),
    ];

    internal static async Task<string> ReadLine(
        this IAnsiConsole console,
        ReadLineRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(console);
        var state = InputState.Initialize(request);

        while (!state.Done)
        {
            cancellationToken.ThrowIfCancellationRequested();
            state = state with { Handled = false };

            var rawKey = await console.Input.ReadKeyAsync(true, cancellationToken).ConfigureAwait(false);
            ArgumentNullException.ThrowIfNull(rawKey);

            var key = rawKey.Value;
            foreach (var handler in _handlers)
            {
                state = handler.Handle(key, state);
                if (state.Handled || state.Done) break;
            }
        }

        return state.Buffer.ToString();
    }
}
