using D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;
using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Controls;

public sealed partial class HistoryTextPrompt<T>
{
    private readonly string _prompt;
    private readonly StringComparer? _comparer;

    private async Task<T> ShowInternalAsync(IAnsiConsole console, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(console);
        return await console.RunExclusive(async () =>
        {
            PromptDisplayExtensions.WritePrompt(this, console, _prompt);

            while (true)
            {
                var input = await console.ReadLine(ReadLineRequest.Create(this, console), token).ConfigureAwait(false);
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
                    if (!FindChoice(input, out result) || result is null)
                    {
                        WriteMessageAndPrompt(console, InvalidChoiceMessage);
                        continue;
                    }
                }
                else if (!TypeConverterHelper.TryConvertFromStringWithCulture<T>(input, Culture, out result)
                         || result is null)
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

    private bool FindChoice(string input, out T? result)
    {
        var choiceMap = Choices.ToDictionary(choice => Converter(choice), choice => choice, _comparer);
        return choiceMap.TryGetValue(input, out result);
    }
}
