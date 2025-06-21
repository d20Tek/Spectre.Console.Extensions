using Spectre.Console;

namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal static class PromptValidationExtensions
{
    internal static bool ValidateResult<T>(HistoryTextPrompt<T> prompt, T value, out string message)
    {
        if (prompt.Validator is null)
        {
            message = string.Empty;
            return true;
        }

        var result = prompt.Validator.Invoke(value);
        message = GetErrorMessageOnFailed(prompt, result);
        return result.Successful;
    }

    private static string GetErrorMessageOnFailed<T>(HistoryTextPrompt<T> prompt, ValidationResult result) =>
        result is { Successful: false } ?
            result.Message ?? prompt.ValidationErrorMessage :
            string.Empty;
}
