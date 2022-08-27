//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Injection;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Testing
{
    /// <summary>
    /// A TestContext for setting up the test environment to run CommandApp commands
    /// to validate their behavior.
    /// </summary>
    public class CommandAppTestContext
    {
        private Action<IConfigurator>? _configureAction;
        private TestCommandInterceptor _commandIntercept;

        /// <summary>
        /// Gets the TypeRegistrar for this test context.
        /// </summary>
        public ITypeRegistrar Registrar { get; }

        /// <summary>
        /// Gets the console for this test context.
        /// </summary>
        public TestConsole Console { get; }

        /// <summary>
        /// Default constructor that initialize this TestContext with a default TypeRegistrar
        /// and a fake IConfigurator implemenation for testing and validation purposed.
        /// </summary>
        public CommandAppTestContext()
        {
            Registrar = new DependencyInjectionTypeRegistrar(new ServiceCollection());
            Console = new TestConsole();
            _commandIntercept = new TestCommandInterceptor();
        }

        /// <summary>
        /// Configures the command app
        /// </summary>
        /// <param name="action">Configuration action</param>
        public void Configure(Action<IConfigurator> action)
        {
            if (_configureAction != null)
            {
                throw new InvalidOperationException("Command app has already been configured.");
            }

            _configureAction = action;
        }

        /// <summary>
        /// Runs the command app sychronously and returns the results from the specified program.
        /// </summary>
        /// <param name="args">Command line arguments represented as list of split strings.</param>
        /// <returns>Returns CommandAppBasicResult with information about the run command.</returns>
        public CommandAppResult Run(string[] args)
        {
            var app = CreateConfiguredApp();
            var exitCode = app.Run(args);

            return new CommandAppResult(
                exitCode, Console.Output, _commandIntercept.Context, _commandIntercept.Settings);
        }

        /// <summary>
        /// Runs the command app sychronously and returns the results from the specified program.
        /// </summary>
        /// <param name="builder">The configured CommandApp builder to run.</param>
        /// <param name="args">Command line arguments represented as list of split strings.</param>
        /// <returns>Returns CommandAppBasicResult with information about the run command.</returns>
        public CommandAppResult Run(CommandAppBuilder builder, string[] args)
        {
            builder.WithTestConfiguration(c => {
                c.ConfigureConsole(Console);
                c.SetInterceptor(_commandIntercept);
            });

            var exitCode = builder.Build().Run(args);

            return new CommandAppResult(
                exitCode, Console.Output, _commandIntercept.Context, _commandIntercept.Settings);
        }

        /// <summary>
        /// Runs the command app sychronously and returns the results from the specified program.
        /// </summary>
        /// <param name="args">Command line arguments represented as list of split strings.</param>
        /// <returns>Returns CommandAppBasicResult with information about the run command.</returns>
        public CommandAppResult RunWithException<T>(string[] args)
            where T : Exception
        {
            var app = CreateConfiguredApp(true);

            try
            {
                var exitCode = app.Run(args);
            }
            catch (T ex)
            {
                Console.WriteLine(ex.Message);
                return new CommandAppResult(
                    -1, Console.Output, _commandIntercept.Context, _commandIntercept.Settings);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Expected an exception of type '{typeof(T).FullName}' to be thrown, " + 
                    $"but instead {ex.GetType().FullName} exception was thrown.");
            }

            throw new InvalidOperationException("Exception expected, but command ran without error.");
        }

        /// <summary>
        /// Runs the command app sychronously and returns the results from the specified program.
        /// </summary>
        /// <param name="builder">The configured CommandApp builder to run.</param>
        /// <param name="args">Command line arguments represented as list of split strings.</param>
        /// <returns>Returns CommandAppBasicResult with information about the run command.</returns>
        public CommandAppResult RunWithException<T>(CommandAppBuilder builder, string[] args)
            where T : Exception
        {
            builder.WithTestConfiguration(c => {
                c.ConfigureConsole(Console);
                c.SetInterceptor(_commandIntercept);
                c.PropagateExceptions();
            });

            try
            {
                var exitCode = builder.Build().Run(args);
            }
            catch (T ex)
            {
                Console.WriteLine(ex.Message);
                return new CommandAppResult(
                    -1, Console.Output, _commandIntercept.Context, _commandIntercept.Settings);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Expected an exception of type '{typeof(T).FullName}' to be thrown, " +
                    $"but instead {ex.GetType().FullName} exception was thrown.");
            }

            throw new InvalidOperationException("Exception expected, but command ran without error.");
        }

        private CommandApp CreateConfiguredApp(bool propagateExceptions = false)
        {
            var app = new CommandApp(Registrar);

            if (_configureAction != null)
            {
                app.Configure(_configureAction);
            }

            app.Configure(c =>
            {
                c.ConfigureConsole(Console);
                c.SetInterceptor(_commandIntercept);
                if (propagateExceptions)
                {
                    c.PropagateExceptions();
                }
            });

            return app;
        }
    }
}
