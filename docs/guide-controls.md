# Guide: Controls

The core package adds extra Spectre.Console controls: a culture-aware currency prompt and presenter, a history-enabled text prompt with recall and auto-completion, and a filesystem path prompt with existence validation and auto-completion.

## Currency Prompt and Presenter

Culture-aware controls for working with currency values: `CurrencyPrompt` for validated input and `CurrencyPresenter` for formatted display.

### Prompt for a currency value

`CurrencyPrompt` implements Spectre's `IPrompt<decimal>` and validates input against culture-specific formatting before converting it to a `decimal`. Configure it with the fluent methods:

```csharp
using D20Tek.Spectre.Console.Extensions.Controls;

var prompt = new CurrencyPrompt("Enter an amount:")
	.WithCulture(CultureInfo.GetCultureInfo("en-US"))
	.WithDefaultValue(9.99m)
	.WithMinValue(0m)
	.WithMaxValue(1000m)
	.WithExampleHint(19.95m)
	.WithErrorMessage("Please enter a valid amount.")
	.WithPromptStyle(new Style(foreground: Color.Green));

decimal amount = AnsiConsole.Prompt(prompt);
```

The fluent configuration methods are:

- `WithCulture(CultureInfo)` - set the culture used for parsing and formatting.
- `WithDefaultValue(decimal)` - value used when the user presses Enter.
- `WithMinValue(decimal)` / `WithMaxValue(decimal)` - allowed range.
- `WithExampleHint(decimal)` - example text shown with the prompt.
- `WithErrorMessage(string)` - custom validation error message.
- `WithPromptStyle(Style)` - style for the prompt label.

### Display a currency value

`CurrencyPresenter.Render` is an extension on `decimal` that formats a value in a culture-aware way, including abbreviations for large values, with optional styles for positive and negative amounts:

```csharp
using D20Tek.Spectre.Console.Extensions.Controls;

string text = 1234.56m.Render(positiveStyle: "green", negativeStyle: "red");
AnsiConsole.MarkupLine(text);
```

## History Text Prompt

`HistoryTextPrompt<T>` extends Spectre's text prompt with shell-style history navigation (arrow up/down) and tab auto-completion. It implements `IPrompt<T>`, so it works anywhere a Spectre prompt does.

### Basic usage

Create the prompt, seed it with prior entries, and prompt for a value:

```csharp
using D20Tek.Spectre.Console.Extensions.Controls;

var prompt = new HistoryTextPrompt<string>("Command:")
	.AddHistory(new[] { "build", "test", "publish" });

string value = AnsiConsole.Prompt(prompt);
```

Use the up and down arrow keys to move through the seeded history, and Tab to auto-complete against the available choices.

### Configure behavior

The prompt exposes a set of fluent extension methods:

- `AddHistory(IEnumerable<string>)` - seed the navigable history list.
- `AddChoice(T)` / `AddChoices(IEnumerable<T>)` - add auto-complete choices.
- `ShowChoices()` / `HideChoices()` - control whether choices are displayed.
- `ShowDefaultValue()` / `HideDefaultValue()` - control default value display.
- `DefaultValue(T)` - set the value used when input is empty.
- `AllowEmpty()` - permit empty input.
- `Validate(Func<T, ValidationResult>)` - add custom validation.
- `ValidationErrorMessage(string)` / `InvalidChoiceMessage(string)` - customize error text.
- `Secret(char?)` - mask input for secrets.
- `WithDisplayConverter(Func<T, string>)` - control how values are displayed.
- `PromptStyle(Style)`, `DefaultValueStyle(Style?)`, `ChoicesStyle(Style?)` - styling.

## Path Prompt

`PathPrompt` is a text prompt for filesystem path input. It validates that the entered path exists (optionally restricted to a file or a directory, and to a set of extensions), and offers Tab auto-completion against the entries of a base directory using the same low-level input infrastructure as `HistoryTextPrompt<T>`. It implements `IPrompt<string>`.

### Basic usage

```csharp
using D20Tek.Spectre.Console.Extensions.Controls;

var prompt = new PathPrompt("Enter a file path:")
	.WithBaseDirectory(Directory.GetCurrentDirectory())
	.WithPathKind(PathKind.File)
	.WithExtensions(".json", ".txt");

string path = AnsiConsole.Prompt(prompt);
```

Press Tab while typing to auto-complete against the files and directories found in the configured base directory.

### Configure behavior

The prompt exposes a set of fluent configuration methods:

- `WithBaseDirectory(string)` - base directory used to resolve relative paths and list auto-complete entries. Defaults to the current working directory.
- `WithPathKind(PathKind)` - restrict the kind of filesystem entry required (`Any`, `File`, or `Directory`) when existence is validated.
- `WithExtensions(params string[])` - restrict accepted paths (and auto-complete entries) to the specified file extensions.
- `WithDefaultValue(string)` - value used when the user presses Enter without input.
- `WithErrorMessage(string)` - custom validation error message.
- `WithPromptStyle(Style)` - style for the prompt label.
- `MustExist(bool)` - control whether the entered path must exist on disk. Enabled by default.
- `IncludeHidden(bool)` - control whether hidden files and directories are included in auto-complete entries. Disabled by default.

## Related

- [API Reference: Core](api-reference-core.md#controls)
