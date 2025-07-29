using Spectre.Console;
using System.Globalization;

namespace D20Tek.Spectre.Console.Extensions.Controls;

/// <summary>
/// A text prompt that manages input of currency values. Allows users to input the text, validate that
/// it matches culture-specific tokens, and converts the result into a decimal.
/// </summary>
public partial class CurrencyPrompt : IPrompt<decimal>, IHasCulture
{
    private readonly string _promptLabel;
    private decimal? _defaultValue;
    private decimal? _minValue;
    private decimal? _maxValue;
    private string? _exampleHint;
    private string? _errorMessage;
    private CultureInfo _culture;
    private Func<string, ValidationResult> _validator;

    /// <inheritdoc/>
    public CultureInfo? Culture
    {
        get => _culture;
        set => _culture = value ?? CultureInfo.CurrentCulture;
    }

    /// <summary>
    /// Constructor that take a prompt label string.
    /// </summary>
    /// <param name="promptLabel">Prompt label text.</param>
    /// <exception cref="ArgumentNullException">An exception when no string is provided.</exception>
    public CurrencyPrompt(string promptLabel)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(promptLabel, nameof(promptLabel));
        _promptLabel = promptLabel;
        _culture = CultureInfo.CurrentCulture;
        _validator = ValidateCurrency;
    }

    /// <summary>
    /// Sets a different culture (other than the system's current culture) to use for the control.
    /// </summary>
    /// <param name="culture">Culture to use in this prompt.</param>
    /// <returns>Current prompt</returns>
    public CurrencyPrompt WithCulture(CultureInfo culture)
    {
        Culture = culture;
        return this;
    }

    /// <summary>
    /// Sets a default value to use for the prompt.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Current prompt</returns>
    public CurrencyPrompt WithDefaultValue(decimal value)
    {
        _defaultValue = value;
        return this;
    }

    /// <summary>
    /// Sets a minimum currency value allowed in the prompt.
    /// </summary>
    /// <param name="min">Minimum value allowed</param>
    /// <returns>Current prompt</returns>
    public CurrencyPrompt WithMinValue(decimal min)
    {
        _minValue = min;
        return this;
    }

    /// <summary>
    /// Sets a maximum currency value allowed in the prompt.
    /// </summary>
    /// <param name="max">Maximum value allowed</param>
    /// <returns>Current prompt</returns>
    public CurrencyPrompt WithMaxValue(decimal max)
    {
        _maxValue = max;
        return this;
    }

    /// <summary>
    /// Sets the hint text for this prompt.
    /// </summary>
    /// <param name="value">Value to use in hint.</param>
    /// <returns>Current prompt</returns>
    public CurrencyPrompt WithExampleHint(decimal value)
    {
        _exampleHint = value.ToString("C", _culture);
        return this;
    }

    /// <summary>
    /// Sets a custom error message to use when the prompt has input errors.
    /// </summary>
    /// <param name="message">Custom error message to show.</param>
    /// <returns>Current prompt</returns>
    public CurrencyPrompt WithErrorMessage(string message)
    {
        _errorMessage = message;
        return this;
    }

    /// <inheritdoc/>
    public decimal Show(IAnsiConsole console) => ShowInternal(console);

    /// <inheritdoc/>
    public Task<decimal> ShowAsync(IAnsiConsole console, CancellationToken cancellationToken) =>
        Task.FromResult(ShowInternal(console));
}
