using Spectre.Console;
using System.Globalization;

namespace D20Tek.Spectre.Console.Extensions.Controls;

public partial class CurrencyPrompt
{
    private const string _currencyFormat = "C";
    private string _promptWithHint() => $"{_promptLabel} (e.g., {_exampleHint})";

    private decimal ShowInternal(IAnsiConsole console)
    {
        ArgumentNullException.ThrowIfNull(console);

        var textPrompt = CreateWithDefault(BuildPromptMessage())
            .Culture(_culture)
            .PromptStyle(_style)
            .Validate(_validator)
            .AllowEmpty();

        var input = console.Prompt(textPrompt);
        return decimal.Parse(input!, NumberStyles.Currency, _culture);
    }

    private string BuildPromptMessage() =>
        string.IsNullOrWhiteSpace(_exampleHint) ? _promptLabel : _promptWithHint();

    private TextPrompt<string> CreateWithDefault(string message)
    {
        var prompt = new TextPrompt<string>(message);
        if (_defaultValue.HasValue)
            prompt.DefaultValue(_defaultValue.Value.ToString(_currencyFormat, _culture));

        return prompt;
    }

    internal ValidationResult ValidateCurrency(string input) =>
        new CurrencyValidator(_defaultValue, _minValue, _maxValue, _errorMessage, _culture)
            .Validate(input);
}
