# Release Notes

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
