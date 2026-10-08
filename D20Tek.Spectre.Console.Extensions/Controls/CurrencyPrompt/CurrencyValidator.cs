using Spectre.Console;
using System.Globalization;

namespace D20Tek.Spectre.Console.Extensions.Controls;

internal class CurrencyValidator(
    decimal? defaultValue,
    decimal? minValue,
    decimal? maxValue,
    string? errorMessage,
    CultureInfo culture)
{
    private class Errors
    {
        public static string InputValue(string input) => $"[red]'{input}' is not a valid currency value.[/]";

        public static string Min(decimal min, CultureInfo culture) =>
            $"[red]Value must be at least {min.ToString("C", culture)}.[/]";
        
        public static string Max(decimal max, CultureInfo culture) =>
            $"[red]Value must not exceed {max.ToString("C", culture)}.[/]";
    }

    public ValidationResult Validate(string input)
    {
        if (HasDefaultValue(input)) return ValidationResult.Success();

        return !decimal.TryParse(input, NumberStyles.Currency, culture, out var value) ?
            ValidationResult.Error(errorMessage ?? Errors.InputValue(input)) :
            ValidateRange(value);
    }

    private bool HasDefaultValue(string input) => string.IsNullOrWhiteSpace(input) && defaultValue.HasValue;

    private ValidationResult ValidateRange(decimal value) =>
        value switch
        {
            var v when minValue.HasValue && v < minValue.Value =>
                ValidationResult.Error(Errors.Min(minValue.Value, culture)),
            var v when maxValue.HasValue && v > maxValue.Value =>
                ValidationResult.Error(Errors.Max(maxValue.Value, culture)),
            _ => ValidationResult.Success()
        };
}
