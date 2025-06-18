using D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;
using Spectre.Console;
using System.Globalization;
using System.Text;

namespace D20Tek.Spectre.Console.Extensions.Controls;

public sealed partial class HistoryTextPrompt<T>
{
    private Func<T, string> SafeConverter => Converter ?? TypeConverterHelper.ConvertToString;
    private readonly string _prompt;
    private readonly StringComparer? _comparer;

    private async Task<T> ShowInternalAsync(IAnsiConsole console, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(console);

        return await console.RunExclusive(async () =>
        {
            WritePrompt(console);

            while (true)
            {
                var input = await console.ReadLine(
                    PromptStyle ?? Style.Plain,
                    IsSecret,
                    Mask,
                    [.. Choices.Select(choice => SafeConverter(choice))],
                    History,
                    cancellationToken).ConfigureAwait(false);

                // Nothing entered?
                if (string.IsNullOrWhiteSpace(input))
                {
                    if (DefaultValue is not null)
                    {
                        console.Write(GetDefaultValueDisplay());
                        console.WriteLine();
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
                    var choiceMap = Choices.ToDictionary(choice => SafeConverter(choice), choice => choice, _comparer);
                    if (choiceMap.TryGetValue(input, out result) && result != null)
                    {
                        return result;
                    }
                    else
                    {
                        console.MarkupLine(InvalidChoiceMessage);
                        WritePrompt(console);
                        continue;
                    }
                }
                else if (!TypeConverterHelper.TryConvertFromStringWithCulture<T>(input, Culture, out result) || result == null)
                {
                    console.MarkupLine(ValidationErrorMessage);
                    WritePrompt(console);
                    continue;
                }

                // Run all validators
                if (!ValidateResult(result, out var validationMessage))
                {
                    console.MarkupLine(validationMessage);
                    WritePrompt(console);
                    continue;
                }

                return result;
            }
        }).ConfigureAwait(false);
    }

    private void WritePrompt(IAnsiConsole console)
    {
        ArgumentNullException.ThrowIfNull(console);

        var builder = new StringBuilder().Append(_prompt.TrimEnd());
        var appendSuffix = ShowChoicesIfExists(builder) | ShowDefaultIfExists(builder);
        var markup = builder.ToString().Trim();

        console.Markup(markup + (appendSuffix ? ": " : " "));
    }

    private bool ShowChoicesIfExists(StringBuilder builder)
    {
        var appendSuffix = false;

        if (ShowChoices && Choices.Count > 0)
        {
            appendSuffix = true;
            var choices = string.Join("/", Choices.Select(choice => SafeConverter(choice)));
            var choicesStyle = ChoicesStyle?.ToMarkup() ?? "blue";
            builder.AppendFormat(CultureInfo.InvariantCulture, " [{0}][[{1}]][/]", choicesStyle, choices);
        }

        return appendSuffix;
    }

    private bool ShowDefaultIfExists(StringBuilder builder)
    {
        var appendSuffix = false;

        if (ShowDefaultValue && DefaultValue is not null)
        {
            appendSuffix = true;
            var defaultValueStyle = DefaultValueStyle?.ToMarkup() ?? "green";
            builder.AppendFormat(CultureInfo.InvariantCulture, " [{0}]({1})[/]", defaultValueStyle, GetDefaultValueDisplay());
        }

        return appendSuffix;
    }

    private string GetDefaultValueDisplay()
    {
        if (DefaultValue is null) return string.Empty;
        var defaultValue = SafeConverter(DefaultValue.Value);
        return IsSecret ? defaultValue.Mask(Mask) : defaultValue;
    }

    private bool ValidateResult(T value, out string message)
    {
        var result = Validator?.Invoke(value);
        message = GetErrorMessageOnFailed(result);
        return result is { Successful: true };
    }

    private string GetErrorMessageOnFailed(ValidationResult? result) =>
        result is { Successful: false } ?
            result?.Message ?? ValidationErrorMessage :
            string.Empty;
}
