using Spectre.Console;
using System.Globalization;
using System.Text;

namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal static class PromptDisplayExtensions
{
    private const string _choiceSeparator = "/";
    private const string _defaultChoiceColor = "blue";
    private const string _choiceFormat = " [{0}][[{1}]][/]";
    private const string _defaultTextColor = "green";
    private const string _defaultFormat = " [{0}]({1})[/]";
    private static string GetSuffixSeparator(bool appendSuffix) => (appendSuffix ? ": " : " ");

    internal static void WritePrompt<T>(HistoryTextPrompt<T> prompt, IAnsiConsole console, string promptText)
    {
        ArgumentNullException.ThrowIfNull(console);

        var builder = new StringBuilder().Append(promptText.TrimEnd());
        var appendSuffix = prompt.ShowChoicesIfExists(builder) | prompt.ShowDefaultIfExists(builder);
        var markup = builder.ToString().Trim();

        console.Markup(markup + GetSuffixSeparator(appendSuffix));
    }

    private static bool ShowChoicesIfExists<T>(this HistoryTextPrompt<T> prompt, StringBuilder builder)
    {
        var appendSuffix = false;

        if (prompt.ShowChoices && prompt.Choices.Count > 0)
        {
            appendSuffix = true;
            var choices = string.Join(_choiceSeparator, prompt.Choices.Select(choice => prompt.Converter(choice)));
            var choicesStyle = prompt.ChoicesStyle?.ToMarkup() ?? _defaultChoiceColor;
            builder.AppendFormat(CultureInfo.InvariantCulture, _choiceFormat, choicesStyle, choices);
        }

        return appendSuffix;
    }

    private static bool ShowDefaultIfExists<T>(this HistoryTextPrompt<T> prompt, StringBuilder builder)
    {
        var appendSuffix = false;

        if (prompt.ShowDefaultValue && prompt.DefaultValue is not null)
        {
            appendSuffix = true;
            var style = prompt.DefaultValueStyle?.ToMarkup() ?? _defaultTextColor;
            builder.AppendFormat(CultureInfo.InvariantCulture, _defaultFormat, style, prompt.GetDefaultValueDisplay());
        }

        return appendSuffix;
    }
}
