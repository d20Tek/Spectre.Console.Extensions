using Spectre.Console;
using System.Globalization;

namespace D20Tek.Spectre.Console.Extensions.Controls;

internal class CurrencyValidator
{
    private readonly decimal? _defaultValue;
    private readonly decimal? _minValue;
    private readonly decimal? _maxValue;
    private readonly string? _errorMessage;
    private readonly CultureInfo _culture;

    public CurrencyValidator(
        decimal? defaultValue,
        decimal? minValue,
        decimal? maxValue,
        string? errorMessage,
        CultureInfo culture)
    {
        _defaultValue = defaultValue;
        _minValue = minValue;
        _maxValue = maxValue;
        _errorMessage = errorMessage;
        _culture = culture;
    }

    public ValidationResult Validate(string input)
    {
        if (HasDefaultValue(input)) return ValidationResult.Success();

        return !decimal.TryParse(input, NumberStyles.Currency, _culture, out var value) ?
            ValidationResult.Error(_errorMessage ?? $"[red]'{input}' is not a valid currency value.[/]") :
            ValidateRange(value);
    }

    private bool HasDefaultValue(string input) => string.IsNullOrWhiteSpace(input) && _defaultValue.HasValue;

    private ValidationResult ValidateRange(decimal value) =>
        value switch
        {
            var v when _minValue.HasValue && v < _minValue.Value =>
                ValidationResult.Error($"[red]Value must be at least {_minValue.Value.ToString("C", _culture)}.[/]"),
            var v when _maxValue.HasValue && v > _maxValue.Value =>
                ValidationResult.Error($"[red]Value must not exceed {_maxValue.Value.ToString("C", _culture)}.[/]"),
            _ => ValidationResult.Success()
        };
}
