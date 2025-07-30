using Spectre.Console;
using System.Globalization;

namespace D20Tek.Spectre.Console.Extensions.Controls;

public partial class CurrencyPrompt
{
    private const string _defaultResult = "0";
    private const string _currencyFormat = "C";
    private string _promptWithHint() => $"{_promptLabel} (e.g., {_exampleHint})";

    private decimal ShowInternal(IAnsiConsole console)
    {
        ArgumentNullException.ThrowIfNull(console);

        var textPrompt = new TextPrompt<string>(BuildPromptMessage())
            .DefaultValue(BuildDefaultValue())
            .Culture(_culture)
            .Validate(_validator)
            .AllowEmpty();

        var input = console.Prompt(textPrompt);
        return decimal.Parse(GetResultOrDefault(input), NumberStyles.Currency, _culture);
    }

    private string BuildPromptMessage() =>
        string.IsNullOrWhiteSpace(_exampleHint) ? _promptLabel : _promptWithHint();

    private string BuildDefaultValue() =>
        _defaultValue.HasValue ? _defaultValue.Value.ToString(_currencyFormat, _culture) : string.Empty;

    internal ValidationResult ValidateCurrency(string input) =>
        new CurrencyValidator(_defaultValue, _minValue, _maxValue, _errorMessage, _culture)
            .Validate(input);

    private string GetResultOrDefault(string? result) =>
        string.IsNullOrWhiteSpace(result) ? _defaultResult : result;
}
