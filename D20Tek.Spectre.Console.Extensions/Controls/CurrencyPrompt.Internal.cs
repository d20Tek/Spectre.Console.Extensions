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
            .Validate(ValidateCurrency);

        var input = console.Prompt(textPrompt);
        return ConvertResultOrDefault(input);
    }

    private string BuildPromptMessage() =>
        string.IsNullOrWhiteSpace(_exampleHint) ? _promptLabel : $"{_promptLabel} (e.g., {_exampleHint})";

    private ValidationResult ValidateCurrency(string input)
    {
        // Default value handling
        if (string.IsNullOrWhiteSpace(input) && _defaultValue.HasValue)
            return ValidationResult.Success();

        // Currency parsing
        if (!decimal.TryParse(input, NumberStyles.Currency, _culture, out var value))
            return ValidationResult.Error(_errorMessage ?? $"'{input}' is not a valid currency value.");

        // Range validation
        if (_minValue.HasValue && value < _minValue.Value)
            return ValidationResult.Error($"Value must be at least {_minValue.Value.ToString("C", _culture)}.");

        if (_maxValue.HasValue && value > _maxValue.Value)
            return ValidationResult.Error($"Value must not exceed {_maxValue.Value.ToString("C", _culture)}.");

        return ValidationResult.Success();
    }

    private decimal ConvertResultOrDefault(string? result) =>
        string.IsNullOrWhiteSpace(result) && _defaultValue.HasValue
                ? _defaultValue.Value
                : decimal.Parse(result ?? string.Empty, NumberStyles.Currency, _culture);
}
