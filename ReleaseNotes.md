# Release Notes

## Release v1.53.1
* Upgraded Spectre dependencies to latest version 0.53.
* Handled breaking changes to Command/AsyncCommand Execute methods.
* Added AnsiConsoleExtensions.WriteMessages & WriteMessagesConditional to write multi-line markup text.

## Release v1.52.1
* Upgraded Spectre dependencies to latest version 0.52.
* Modernized code in Spectre.Console.Extensions, unit tests, and sample projects.
* Upgraded to MsTest 4.0, which required updates to some tests and how Assert is called.

## Release v1.50.1.7
* Implemented CurrencyPrompt control, a TextPrompt (culture-aware) for currency input validation and conversion to decimal value.
* Implemented CurrencyPresenter control to show currency in a culture-aware way, including abbreviations for large values.
* Ensured CurrencyPresenter abbreviations worked in various locale configurations.
* Added unit tests.

## Release v1.50.1.6
* Updated dependent packages to the latest versions.

## Release v1.50.1.4
* Implemented Table extension AddSeparatorRow to add a separator based on the knowns column widths.
* Minor library cleanup.

## Release v1.50.1.4
* Implemented ApplyConfiguration pattern to make it easier to build large CommandApps with many defined commands.
* ICommandConfiguration allows you do define groups of commands that are configured more cleanly together.
* The ApplyConfiguration extension method lets to run each ICommandConfiguration implementation.
* Implemented HistoryTextPrompt&lt;T&gt; that collects console input and converts it to type of T. Adds the ability to remember past command entries similar to command prompt history behavior.
	* Replicated basic TextPrompt&lt;T&gt; to enter text and process special keys like arrow left, right, backspace, insert.
	* Added support for setting and accepting default text when no text is entered.
	* Added support for choice list and picking a value from that, including auto-complete from the list items.
	* Added history functionality to use arrow up key to move to past entries that were entered.
	* Added arrow down key to move the next entry it previous entries history list.
 
## Release v1.50.1.3
* Implemented InteractiveCommandBase to build an interactive command prompt that parses input string and runs other registered commands.
* Added sample to show how to build an interactive prompt.
* Added TypeResolverExtensions methods (GetService&lt;T&gt; and GetRequiredService&lt;T&gt;) to simplify resolution and align it with IServiceProvider extensions.
* Added unit tests for new functionality.

## Release v1.50.1.2
* Added ISupportLifetimes and ITypeRegistrar extension method WithLifetimes to incorporate strongly-typed registration extension methods.
* Implemented ISupportLifetimes on DependencyInjectionTypeRegistrar and LamarTypeRegistrar.
* Implemented LifetimeExtenion generic methods RegisterSingleton, RegisterScoped, and Register Transient to allows services to be added with different ServiceLifetimes.
* Added ability to set default ServiceLifetime for containers that support lifetimes: DITypeRegistrar, LamarTypeRegistrar, and LightInject.
* Added unit tests for new functions.

## Release v1.50.1
* Split off non-Microsoft.Extensions.DependencyInjection frameworks to a separate package: D20Tek.Spectre.Console.Extensions.MoreContainers.
* Minimize the dependencies on the core package: D20Tek.Spectre.Console, so other DI frameworks are not included unnecessarily.
* Changing versioning to match Spectre.Console more clearly.
* Added unit tests for this change and remaining untested areas.
* Added NullRemainingArguments class to help in testing commands when user doesn't care about arguments.
* Added support to test fakes for async delegate methods.
 
## Release v1.2.10
* Updated to target .NET 9.
* Updated Spectre.Console dependencies to 0.50.0.
* Updated all other dependent packages to their latest versions.

## Release v1.2.1
* Updated to target .NET 8.
* Updated all dependencies for projects.
* Fixed breaking changes in Spectre.Console.Cli.
* Deprecated SimpleInjector type registrar.

## Release v1.1.3
* Upgraded projects to .NET 7
* Upgraded to latest Spectre.Console

## Release v1.0.7
* Implemented CommandAppTestContext to simplify running individual commands within the CommandApp infrastructure.
* Implemented CommandAppBuilderTestContext to simplify running commands for apps that are based on the CommandAppBuilder pattern.
* Implemented CommandConfigurationTestContext to validate your CommandApp's configuration matches what you expect.
* Implemented CommandAppE2ERunner to run the Main method of any command-line app with specified arguments.
* Several helper classes (TestConsole, CommandMetadata, FakeCommandConfigurator, etc) to assist in testing and validation.

## Release v1.0.6
* Added optional parameter to all With*Container extension methods to allow container to be passed to CommandAppBuilder.
* If no optional container passed, we still create a new instance of the appropriate container.
* New unit tests to validate providing optional containers.

## Release v1.0.5
* Implemented Lamar type registrar and resolver with unit tests.
* Implement CommandAppBuilder extension method for Lamar container with unit test.
* Sample project for Lamar DI integration.

## Release v1.0.4
* Implemented Autofac type registrar and resolver with unit tests.
* Implement CommandAppBuilder extension method for Autofac.
* Extension method unit tests.
* Sample project for Autofac integration.
* Implemented LightInject type registrar and resolver with unit tests.
* Implement CommandAppBuilder extension method for LightInject with unit test.
* Sample project for LightInject integration.

## Release v1.0.3
* Implemented SimpleInjector type registrar and resolver with unit tests.
* Created sample console project for SimpleInjector integration.
* Created CommandAppBuilder as new mechanism for creating, configuring, and running Spectre.Console CommandApps.
* Migrated all sample apps from DI factories to the new builder pattern.
* BREAKING CHANGE: Removed DI framework specific factory classes and old StartupBase&lt;T&gt; class.
* Added WithDefaultCommand method to replace Build&lt;T&gt; method... remove duplicate Build methods.
* Updated all samples to use WithDefaultCommand.
* Added test coverage for CommandAppBuilder and its extension methods.

## Release v1.0.2
* Created Ninject DI integration for type registrar and resolver.
* Added sample project for Ninject.Cli to show how to integrate.
* Added unit tests to cover NinjectTypeRegistrar and NinjectTypeResolver.
* Refactored StartupBase to be generic for different types of DI frameworks.
* Added NinjectFactory to encapsulate boilerplate startup code, with unit tests.
* Created generic CommonDIFactory to encapsulate the common creation and configuration logic.
* Updated both DI factories to use the common class.

## Release v1.0.1
* Initial project for Spectre.Console extensions.
* First extension for dependency injection using Microsoft.Extensions.DependencyInjection framework.
* Added basic unit test projects.
* Added DependencyInjectionFactory to simply create configured CommandApp instances.
* Added sample projects that use DI: Basic.Cli and DependencyInjection.Cli.
* Build actions for CI/CD, official build in main, and package releases.
