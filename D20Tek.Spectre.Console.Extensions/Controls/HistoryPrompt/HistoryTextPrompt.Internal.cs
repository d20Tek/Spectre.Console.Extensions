using D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;
using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Controls;

public sealed partial class HistoryTextPrompt<T>
{
    private readonly string _prompt;
    private readonly StringComparer? _comparer;

    private async Task<T> ShowInternalAsync(IAnsiConsole console, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(console);
        return await console.RunExclusive(async () =>
        {
            PromptDisplayExtensions.WritePrompt(this, console, _prompt);

            while (true)
            {
                var input = await GetConsoleInput(console, cancellationToken);
                if (string.IsNullOrWhiteSpace(input))
                {
                    if (DefaultValue is not null)
                    {
                        console.WriteLine(GetDefaultValueDisplay());
                        return DefaultValue.Value;
                    }

                    if (!AllowEmpty)
                    {
                        continue;
                    }
                }

                console.WriteLine();
                T? result;
                if (Choices.Count > 0)
                {
                    if (FindChoice(input, out result) && result is not null)
                    {
                        return result;
                    }
                    else
                    {
                        WriteMessageAndPrompt(console, InvalidChoiceMessage);
                        continue;
                    }
                }
                else if (!TypeConverterHelper.TryConvertFromStringWithCulture<T>(input, Culture, out result)
                         || result == null)
                {
                    WriteMessageAndPrompt(console, ValidationErrorMessage);
                    continue;
                }

                if (!PromptValidationExtensions.ValidateResult(this, result, out var validationMessage))
                {
                    WriteMessageAndPrompt(console, validationMessage);
                    continue;
                }

                History.Add(input);
                return result;
            }
        }).ConfigureAwait(false);
    }

    internal string GetDefaultValueDisplay()
    {
        var defaultValue = Converter(DefaultValue!.Value);
        return IsSecret ? defaultValue.Mask(Mask) : defaultValue;
    }

    private void WriteMessageAndPrompt(IAnsiConsole console, string message)
    {
        console.MarkupLine(message);
        PromptDisplayExtensions.WritePrompt(this, console, _prompt);
    }

    private async Task<string> GetConsoleInput(IAnsiConsole console, CancellationToken cancellationToken) =>
        await console.ReadLine(
            new ReadLineRequest(
                console,
                PromptStyle ?? Style.Plain,
                IsSecret,
                Mask,
                [.. Choices.Select(choice => Converter(choice))],
                History),
            cancellationToken).ConfigureAwait(false);

    private bool FindChoice(string input, out T? result)
    {
        var choiceMap = Choices.ToDictionary(choice => Converter(choice), choice => choice, _comparer);
        return choiceMap.TryGetValue(input, out result);
    }
}
