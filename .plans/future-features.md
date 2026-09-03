# Future Features

This document captures candidate features for future releases of the D20Tek.Spectre.Console.Extensions packages. Each item notes what it adds, why it is valuable, and whether Spectre.Console already covers the capability. Items are grouped by priority tier based on impact relative to effort.

## Guiding Principle

Prioritize features that reinforce the library's existing strengths and fill genuine gaps that Spectre.Console does not already cover. Avoid thin wrappers over existing Spectre.Console.Cli APIs, since they add surface area without meaningful value.

The library's strongest, genuinely differentiating areas are:
- Testing infrastructure (no equivalent ships in Spectre.Console).
- Culture-aware, validated prompt controls (for example, CurrencyPrompt).
- Verbosity services.
- Integration points that Spectre.Console.Cli does not provide (verbosity-aware logging and configuration).

## Tier 1 - High Impact, Fills Genuine Gaps

### 1. Verbosity-Aware Logging Integration (Microsoft.Extensions.Logging) [DONE]
- What it adds: Convenience and cohesion around Microsoft.Extensions.Logging, not basic injection support. Specifically:
  - A verbosity bridge that maps the existing VerbosityLevel enum to LogLevel, so the same -v|--verbosity switch that controls prompts and output also sets the minimum log level.
  - An IAnsiConsole-backed logger provider so log output renders through Spectre (consistent styling and markup, and respects TestConsole in tests) instead of the stock AddConsole() provider writing directly to System.Console.
  - A one-liner builder hook, for example CommandAppBuilder.WithLogging(...), that wires AddLogging plus the verbosity bridge plus the console provider, so users do not have to reach through WithLifetimes().Services.
- Why it matters: The README already lists logging integration as a future goal. Without the verbosity bridge, verbosity and logging are configured independently. Without the IAnsiConsole-backed provider, log output bypasses TestConsole and breaks the testing story.
- Spectre.Console coverage: None. Spectre.Console.Cli does not ship ILogger wiring.
- Important clarification: Basic logger injection already works today with no new code. The DependencyInjectionTypeRegistrar exposes the underlying IServiceCollection via its Services property, and the resolver forwards to IServiceProvider.GetService. A consumer can already call registrar.WithLifetimes().Services.AddLogging(...) in ConfigureServices, and any command can then inject ILogger<T> through its constructor. This feature is therefore about verbosity integration, Spectre-rendered output, and a fluent builder hook, not about enabling injection.

### 2. Configuration and Options Binding (Microsoft.Extensions.Configuration)
- What it adds: Wiring for Microsoft.Extensions.Configuration (JSON, environment variables, user secrets) into the CommandAppBuilder, for example a WithConfiguration(...) method, plus binding to strongly typed settings objects.
- Why it matters: Configuration is table-stakes for production CLI tools and pairs naturally with the existing dependency injection container.
- Spectre.Console coverage: None. Spectre.Console.Cli does not ship IConfiguration wiring, so this is a real gap.

### 3. Additional Prompt Controls
Round out the "Controls" story with a themed family of culture-aware, validated prompts that follow the existing CurrencyPrompt pattern (IPrompt<T> plus IHasCulture, with a validator and presenter split).

- DatePrompt / DateRangePrompt: Culture-aware date entry with format hints and range validation.
  - Spectre.Console coverage: None dedicated. Ask<DateTime>() exists, but there is no culture-aware, format-hinted, range-validating date control. Recommended first control because of its everyday utility and close similarity to CurrencyPrompt.
- PathPrompt: Filesystem path input with existence validation and path auto-completion.
  - Spectre.Console coverage: None. Leverages the existing HistoryTextPrompt autocomplete infrastructure. Genuinely new.
- PatternPrompt (formerly proposed as MaskedPrompt): Patterned input such as phone numbers or identifiers, enforcing a format like (###) ###-####.
  - Spectre.Console coverage: Partial and easily confused. Spectre.Console provides secret masking (hiding input) but not pattern or format masking (enforcing a layout). Rename away from "Masked" to avoid ambiguity with the existing secret feature. Hold this item unless rebranded.

## Tier 2 - Minor Value-Add

### 4. CompositeCommandInterceptor
- What it adds: A helper that composes multiple ICommandInterceptor instances into a chain (for example, timing plus logging plus telemetry).
- Why it matters: Spectre.Console.Cli's SetInterceptor registers a single interceptor. Composing several currently requires custom code.
- Spectre.Console coverage: The interceptor mechanism (ICommandInterceptor, SetInterceptor) already exists. Only the multi-interceptor composition is additive, and the value is modest.

### 5. Async Cancellation Ergonomics
- What it adds: Out-of-the-box Ctrl+C wiring (Console.CancelKeyPress linked to a CancellationToken) provided through the CommandAppBuilder so long-running commands cancel cleanly.
- Why it matters: InteractiveCommandBase already accepts a CancellationToken. Providing the cancellation plumbing by default removes boilerplate.
- Spectre.Console coverage: Cancellation tokens are supported, but the default Ctrl+C linkage is left to the consumer.

## Tier 3 - Polish for a 1.0 Feel

### 6. Fluent Assertions for Testing
- What it adds: A fluent assertion helper set over CommandAppResult, for example result.ShouldSucceed().AndOutputContains(...).
- Why it matters: Complements the differentiating testing infrastructure and improves the test authoring experience.
- Spectre.Console coverage: None.

### 7. Command Registration Analyzer or Source Generator (stretch)
- What it adds: Auto-discovery of ICommandConfiguration and commands via attributes to reduce startup wiring.
- Why it matters: Cuts boilerplate for larger command sets.
- Spectre.Console coverage: None. This is a larger investment and is intentionally a stretch goal.

### 8. Documentation and Changelog Parity
- What it adds: An api-reference documentation set under docs/ to complement the existing CHANGELOG.md.
- Why it matters: Contributor guidelines require both api-reference docs and changelog entries whenever the public API changes. A public launch should include this structure. The repository now has a CHANGELOG.md following the Keep a Changelog format, but still lacks a docs/ folder.

## Recommended Splash Focus

For the initial public release, prioritize the items that fill genuine gaps and extend existing strengths:
1. Verbosity-aware logging integration (verbosity bridge, Spectre-rendered output, and a builder hook; note that basic logger injection already works today).
2. Configuration and options binding.
3. One or two new prompt controls, starting with DatePrompt, then PathPrompt.

This produces a coherent launch narrative: a complete toolkit for building, configuring, testing, and polishing Spectre.Console CLI apps.
