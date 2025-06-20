using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Controls;

/// <summary>
/// Extension helper methods to assist in configuring the HistoryTextPrompt. These extension methods
/// correspond to similar methods in the Spectre.Console TextPrompt.
/// </summary>
public static class HistoryTextPromptExtensions
{
    private const char _defaultMask = '*';

    /// <summary>
    /// Allow empty input.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static HistoryTextPrompt<T> AllowEmpty<T>(this HistoryTextPrompt<T> obj)
    {
        ArgumentNullException.ThrowIfNull(obj);
        obj.AllowEmpty = true;
        return obj;
    }

    /// <summary>
    /// Sets the prompt style.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="style">The prompt style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static HistoryTextPrompt<T> PromptStyle<T>(this HistoryTextPrompt<T> obj, Style style)
    {
        ArgumentNullException.ThrowIfNull(obj);
        ArgumentNullException.ThrowIfNull(style);
        obj.PromptStyle = style;
        return obj;
    }

    /// <summary>
    /// Show or hide choices.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="show">Whether or not choices should be visible.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static HistoryTextPrompt<T> ShowChoices<T>(this HistoryTextPrompt<T> obj, bool show)
    {
        ArgumentNullException.ThrowIfNull(obj);
        obj.ShowChoices = show;
        return obj;
    }

    /// <summary>
    /// Shows choices.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static HistoryTextPrompt<T> ShowChoices<T>(this HistoryTextPrompt<T> obj) => ShowChoices(obj, true);

    /// <summary>
    /// Hides choices.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static HistoryTextPrompt<T> HideChoices<T>(this HistoryTextPrompt<T> obj) => ShowChoices(obj, false);

    /// <summary>
    /// Show or hide the default value.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="show">Whether or not the default value should be visible.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static HistoryTextPrompt<T> ShowDefaultValue<T>(this HistoryTextPrompt<T> obj, bool show = true)
    {
        ArgumentNullException.ThrowIfNull(obj);
        obj.ShowDefaultValue = show;
        return obj;
    }

    /// <summary>
    /// Hides the default value.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static HistoryTextPrompt<T> HideDefaultValue<T>(this HistoryTextPrompt<T> obj) =>
        ShowDefaultValue(obj, false);

    /// <summary>
    /// Sets the validation error message for the prompt.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="message">The validation error message.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static HistoryTextPrompt<T> ValidationErrorMessage<T>(this HistoryTextPrompt<T> obj, string message)
    {
        ArgumentNullException.ThrowIfNull(obj);
        obj.ValidationErrorMessage = message;
        return obj;
    }

    /// <summary>
    /// Sets the "invalid choice" message for the prompt.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="message">The "invalid choice" message.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static HistoryTextPrompt<T> InvalidChoiceMessage<T>(this HistoryTextPrompt<T> obj, string message)
    {
        ArgumentNullException.ThrowIfNull(obj);
        obj.InvalidChoiceMessage = message;
        return obj;
    }

    /// <summary>
    /// Sets the default value of the prompt.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="value">The default value.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static HistoryTextPrompt<T> DefaultValue<T>(this HistoryTextPrompt<T> obj, T value)
    {
        ArgumentNullException.ThrowIfNull(obj);
        obj.DefaultValue = new DefaultPromptValue<T>(value);
        return obj;
    }

    /// <summary>
    /// Sets the validation criteria for the prompt.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="validator">The validation criteria.</param>
    /// <param name="message">The validation error message.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static HistoryTextPrompt<T> Validate<T>(
        this HistoryTextPrompt<T> obj,
        Func<T, bool> validator,
        string? message = null)
    {
        ArgumentNullException.ThrowIfNull(obj);
        obj.Validator = result =>
            validator(result) ? ValidationResult.Success() : ValidationResult.Error(message);
        return obj;
    }

    /// <summary>
    /// Sets the validation criteria for the prompt.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="validator">The validation criteria.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static HistoryTextPrompt<T> Validate<T>(this HistoryTextPrompt<T> obj, Func<T, ValidationResult> validator)
    {
        ArgumentNullException.ThrowIfNull(obj);
        obj.Validator = validator;
        return obj;
    }

    /// <summary>
    /// Adds a choice to the prompt.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="choice">The choice to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static HistoryTextPrompt<T> AddChoice<T>(this HistoryTextPrompt<T> obj, T choice)
    {
        ArgumentNullException.ThrowIfNull(obj);
        obj.Choices.Add(choice);
        return obj;
    }

    /// <summary>
    /// Adds multiple choices to the prompt.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="choices">The choices to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static HistoryTextPrompt<T> AddChoices<T>(this HistoryTextPrompt<T> obj, IEnumerable<T> choices)
    {
        ArgumentNullException.ThrowIfNull(obj);
        ArgumentNullException.ThrowIfNull(choices);
        obj.Choices.AddRange(choices);
        return obj;
    }

    /// <summary>
    /// Replaces prompt user input with mask in the console.
    /// </summary>
    /// <typeparam name="T">The prompt type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="mask">The masking character to use for the secret.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static HistoryTextPrompt<T> Secret<T>(this HistoryTextPrompt<T> obj, char? mask = _defaultMask)
    {
        ArgumentNullException.ThrowIfNull(obj);
        obj.IsSecret = true;
        obj.Mask = mask;
        return obj;
    }

    /// <summary>
    /// Sets the function to create a display string for a given choice.
    /// </summary>
    /// <typeparam name="T">The prompt type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="displaySelector">The function to get a display string for a given choice.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static HistoryTextPrompt<T> WithDisplayConverter<T>(this HistoryTextPrompt<T> obj, Func<T, string> displaySelector)
    {
        ArgumentNullException.ThrowIfNull(obj);
        obj.Converter = displaySelector;
        return obj;
    }

    /// <summary>
    /// Sets the style in which the default value is displayed.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="style">The default value style or <see langword="null"/> to use the default style (green).</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static HistoryTextPrompt<T> DefaultValueStyle<T>(this HistoryTextPrompt<T> obj, Style? style)
    {
        ArgumentNullException.ThrowIfNull(obj);
        obj.DefaultValueStyle = style;
        return obj;
    }

    /// <summary>
    /// Sets the style in which the list of choices is displayed.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="style">The style to use for displaying the choices or <see langword="null"/> to use 
    /// the default style (blue).</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static HistoryTextPrompt<T> ChoicesStyle<T>(this HistoryTextPrompt<T> obj, Style? style)
    {
        ArgumentNullException.ThrowIfNull(obj);
        obj.ChoicesStyle = style;
        return obj;
    }

    /// <summary>
    /// Adds history to the prompt to allow arrow-key scrolling on input.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="history">The history lines to add.  The last history string will be the first shown when
    /// the user arrows up at the prompt.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static HistoryTextPrompt<T> AddHistory<T>(this HistoryTextPrompt<T> obj, IEnumerable<string> history)
    {
        ArgumentNullException.ThrowIfNull(obj);
        ArgumentNullException.ThrowIfNull(history);
        obj.History.AddRange(history);
        return obj;
    }
}
