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

### 2. Configuration and Options Binding (Microsoft.Extensions.Configuration) [DONE]
- What it adds: A new separate package (D20Tek.Spectre.Console.Extensions.Configuration) that wires Microsoft.Extensions.Configuration and Options into the CommandAppBuilder. Two builder hooks:
  - WithConfiguration(...): builds an IConfiguration (appsettings.json plus environment variables by default, with an optional configure delegate) and registers it in the container.
  - WithOptions&lt;T&gt;(sectionName): binds a configuration section to a strongly typed options class, resolvable as IOptions&lt;T&gt;.
- Why it matters: Configuration is table-stakes for production CLI tools and pairs naturally with the existing dependency injection container. Any command can then inject IConfiguration or IOptions&lt;T&gt; through its constructor, exactly like ILogger&lt;T&gt; today.
- Spectre.Console coverage: None. Spectre.Console.Cli does not ship IConfiguration wiring, so this is a real gap.
- Packaging decision: Separate package. The code surface is small (roughly two extension methods), but it pulls in 4-5 additional Microsoft.Extensions.Configuration/Options dependencies. Keeping it out of the core package preserves the core's minimal-dependency goal, consistent with the MoreContainers split.
- Design decision: Keep CommandSettings (CLI args) and IOptions&lt;T&gt; (config) separate; commands decide precedence explicitly. Config-backed defaults for command options can be added later if needed.
- Implementation note: The builder hooks need access to the container. DONE - CommandAppBuilder now exposes a public ITypeRegistrar? Registrar getter and a public GetServiceCollection() helper that returns the registrar's IServiceCollection (throwing if no DI container is configured). Add-on extension packages (logging, configuration, and future ones) should call GetServiceCollection() rather than reaching through WithLifetimes().Services. The existing WithLogging hook was refactored to use this accessor.
- Status: DONE - Package implemented with WithConfiguration and WithOptions&lt;T&gt; (data-annotation validated), covered by unit tests, and demonstrated by the Configuration.Cli sample.

### 3. Generic Host Integration (Microsoft.Extensions.Hosting) [DONE]
- What it adds: A new separate package (D20Tek.Spectre.Console.Extensions.Hosting) that bridges the .NET Generic Host (HostApplicationBuilder / IHostBuilder) to Spectre.Console.Cli. Consumers configure configuration, options, logging, and services through the standard host model, then run a CommandApp whose types resolve from the host's already-built IServiceProvider. This is a sibling to CommandAppBuilder for teams that want the full .NET app model.
- Why it matters: Generic Host is the standard .NET app-composition model and unlocks hosted services, host lifetime, and the layered configuration/logging defaults with no bespoke wiring. It complements the existing lean CommandAppBuilder rather than replacing it.
- Spectre.Console coverage: None. Spectre.Console.Cli does not ship Generic Host wiring.
- Packaging decision: Separate package that references the core package (reuses the builder pattern and DI-bridge conventions, consistent with the MoreContainers and Configuration splits). Adds a Microsoft.Extensions.Hosting dependency, so it stays out of the core package.
- Key design point: Spectre registers its own types (command types, IAnsiConsole, its config) at Run(), which is after host.Build(). Because the host provider is already immutable by then, the bridge does not try to mutate it. Instead:
  - HostTypeRegistrar accepts Spectre's run-time Register/RegisterInstance/RegisterLazy calls into an internal registration map (it does not throw after build).
  - HostTypeResolver fuses the two sources: it first tries host.Services.GetService(type); if that is null and the type is in the map, it constructs the instance with ActivatorUtilities.CreateInstance(host.Services, impl) so command constructor dependencies (IOptions<T>, ILogger<T>, IConfiguration, user services) are injected from the host provider. Instance and factory registrations are honored directly from the map.
- API surface: Both a low-level path (HostTypeRegistrar plus an IHost.RunCommandAppAsync(args, configure) extension) and a fluent HostCommandAppBuilder that mirrors CommandAppBuilder (WithDefaultCommand<T>, ConfigureCommands, Build/RunAsync).
- Sample: A GenericHost.Cli sample using HostApplicationBuilder as the active code path, with the equivalent IHostBuilder (Host.CreateDefaultBuilder) style shown as comments in Program.cs.
- Deliverables: New package project, HostTypeResolver, HostTypeRegistrar, HostCommandAppExtensions, HostCommandAppBuilder, exhaustive unit tests with fakes, the GenericHost.Cli sample, and README / CHANGELOG / future-features updates.
- Status: DONE - Package implemented with HostTypeRegistrar, HostTypeResolver, HostRegistration, HostCommandAppExtensions, and the HostCommandAppBuilder fluent builder. Covered by exhaustive unit tests (48 tests, all passing) and demonstrated by the GenericHost.Cli sample. README and CHANGELOG updated.

### 4. Additional Prompt Controls
Round out the "Controls" story with a themed family of culture-aware, validated prompts that follow the existing CurrencyPrompt pattern (IPrompt<T> plus IHasCulture, with a validator and presenter split).

- DatePrompt / DateRangePrompt: Culture-aware date entry with format hints and range validation.
  - Spectre.Console coverage: None dedicated. Ask<DateTime>() exists, but there is no culture-aware, format-hinted, range-validating date control. Recommended first control because of its everyday utility and close similarity to CurrencyPrompt.
- PathPrompt: Filesystem path input with existence validation and path auto-completion.
  - Spectre.Console coverage: None. Leverages the existing HistoryTextPrompt autocomplete infrastructure. Genuinely new.
- PatternPrompt (formerly proposed as MaskedPrompt): Patterned input such as phone numbers or identifiers, enforcing a format like (###) ###-####.
  - Spectre.Console coverage: Partial and easily confused. Spectre.Console provides secret masking (hiding input) but not pattern or format masking (enforcing a layout). Rename away from "Masked" to avoid ambiguity with the existing secret feature. Hold this item unless rebranded.

## Tier 2 - Minor Value-Add

### 5. CompositeCommandInterceptor
- What it adds: A helper that composes multiple ICommandInterceptor instances into a chain (for example, timing plus logging plus telemetry).
- Why it matters: Spectre.Console.Cli's SetInterceptor registers a single interceptor. Composing several currently requires custom code.
- Spectre.Console coverage: The interceptor mechanism (ICommandInterceptor, SetInterceptor) already exists. Only the multi-interceptor composition is additive, and the value is modest.

### 6. Async Cancellation Ergonomics
- What it adds: Out-of-the-box Ctrl+C wiring (Console.CancelKeyPress linked to a CancellationToken) provided through the CommandAppBuilder so long-running commands cancel cleanly.
- Why it matters: InteractiveCommandBase already accepts a CancellationToken. Providing the cancellation plumbing by default removes boilerplate.
- Spectre.Console coverage: Cancellation tokens are supported, but the default Ctrl+C linkage is left to the consumer.

## Tier 3 - Polish for a 1.0 Feel

### 7. Fluent Assertions for Testing
- What it adds: A fluent assertion helper set over CommandAppResult, for example result.ShouldSucceed().AndOutputContains(...).
- Why it matters: Complements the differentiating testing infrastructure and improves the test authoring experience.
- Spectre.Console coverage: None.

### 8. Command Registration Analyzer or Source Generator (stretch)
- What it adds: Auto-discovery of ICommandConfiguration and commands via attributes to reduce startup wiring.
- Why it matters: Cuts boilerplate for larger command sets.
- Spectre.Console coverage: None. This is a larger investment and is intentionally a stretch goal.

### 9. Documentation and Changelog Parity
- What it adds: An api-reference documentation set under docs/ to complement the existing CHANGELOG.md.
- Why it matters: Contributor guidelines require both api-reference docs and changelog entries whenever the public API changes. A public launch should include this structure. The repository now has a CHANGELOG.md following the Keep a Changelog format, but still lacks a docs/ folder.

## Recommended Splash Focus

For the initial public release, prioritize the items that fill genuine gaps and extend existing strengths:
1. Verbosity-aware logging integration (verbosity bridge, Spectre-rendered output, and a builder hook; note that basic logger injection already works today).
2. Configuration and options binding.
3. Generic Host integration (a sibling to CommandAppBuilder for teams that want the full .NET app model).
4. One or two new prompt controls, starting with DatePrompt, then PathPrompt.

This produces a coherent launch narrative: a complete toolkit for building, configuring, testing, and polishing Spectre.Console CLI apps.
