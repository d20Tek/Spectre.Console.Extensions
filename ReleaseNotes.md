# Release Notes

## Release v1.0.2
* Implemented SimpleInjector type registrar and resolver with unit tests.
* Added unit tests for SimpleInjector registrar and resolver.
* Created sample console project for SimpleInjector integration.
* Created CommandAppBuilder as new mechanism for creating, setting up, and running Spectre.Console CommandApps.
* Migrated all sample apps from DI factories to the new builder pattern.
* BREAKING CHANGE: Removed DI framework specific factory classes and old StartupBase<T> class.
* Added WithDefaultCommand method to replace Build<T> method... remove duplicate Build methods.
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
