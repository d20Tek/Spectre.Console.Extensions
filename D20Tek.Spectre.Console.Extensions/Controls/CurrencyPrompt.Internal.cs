using Spectre.Console;
using System.Globalization;

namespace D20Tek.Spectre.Console.Extensions.Controls;

public partial class CurrencyPrompt
{
    private decimal ShowInternal(IAnsiConsole console)
    {
        ArgumentNullException.ThrowIfNull(console);

        var textPrompt = new TextPrompt<string>(BuildPromptMessage())
            .DefaultValue(_defaultValue.HasValue ? _defaultValue.Value.ToString("C", _culture) : string.Empty)
            .Culture(_culture)
            .Validate(_validator)
            .AllowEmpty();

        var input = console.Prompt(textPrompt);
        return decimal.Parse(ResultOrDefault(input), NumberStyles.Currency, _culture);
    }

    private string BuildPromptMessage() =>
        string.IsNullOrWhiteSpace(_exampleHint) ? _promptLabel : $"{_promptLabel} (e.g., {_exampleHint})";

    internal ValidationResult ValidateCurrency(string input) =>
        new CurrencyValidator(_defaultValue, _minValue, _maxValue, _errorMessage, _culture)
            .Validate(input);

    private string ResultOrDefault(string? result) => string.IsNullOrWhiteSpace(result) ? "0" : result;
}
