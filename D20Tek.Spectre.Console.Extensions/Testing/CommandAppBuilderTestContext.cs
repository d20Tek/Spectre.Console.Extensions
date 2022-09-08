//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Testing
{
    /// <summary>
    /// A TestContext for setting up the test environment that uses the CommandAppBuilder pattern
    /// and validates the resulting CommandApp behavior.
    /// </summary>
    public class CommandAppBuilderTestContext
    {
        private TestCommandInterceptor _commandIntercept;

        /// <summary>
        /// Gets the console for this test context.
        /// </summary>
        public TestConsole Console { get; }

        /// <summary>
        /// Gets the command app builder in this test context.
        /// </summary>
        public CommandAppBuilder Builder { get; }

        /// <summary>
        /// Default constructor that initialize this TestContext with a default TypeRegistrar
        /// and a fake IConfigurator implemenation for testing and validation purposed.
        /// </summary>
        public CommandAppBuilderTestContext()
        {
            Builder = new CommandAppBuilder();
            Console = new TestConsole();
            _commandIntercept = new TestCommandInterceptor();
        }

        /// <summary>
        /// Runs the command app sychronously and returns the results from the specified program.
        /// </summary>
        /// <param name="args">Command line arguments represented as list of split strings.</param>
        /// <returns>Returns CommandAppResult with information about the run command.</returns>
        public CommandAppResult Run(string[] args) =>
            RunAsync(args).GetAwaiter().GetResult();

        /// <summary>
        /// Runs the command app asychronously and returns the results from the specified program.
        /// </summary>
        /// <param name="args">Command line arguments represented as list of split strings.</param>
        /// <returns>Returns CommandAppResult with information about the run command.</returns>
        public async Task<CommandAppResult> RunAsync(string[] args)
        {
            Builder.WithTestConfiguration(c => {
                c.ConfigureConsole(Console);
                c.SetInterceptor(_commandIntercept);
            });

            var exitCode = await Builder.Build().RunAsync(args);

            return new CommandAppResult(
                exitCode, Console.Output, _commandIntercept.Context, _commandIntercept.Settings);
        }

        /// <summary>
        /// Runs the command app sychronously and returns the results from the specified program.
        /// </summary>
        /// <param name="args">Command line arguments represented as list of split strings.</param>
        /// <returns>Returns CommandAppResult with information about the run command.</returns>
        public CommandAppResult RunWithException<T>(string[] args)
            where T : Exception =>
            RunWithExceptionAsync<T>(args).GetAwaiter().GetResult();

        /// <summary>
        /// Runs the command app asychronously and returns the results from the specified program.
        /// </summary>
        /// <param name="args">Command line arguments represented as list of split strings.</param>
        /// <returns>Returns CommandAppResult with information about the run command.</returns>
        public async Task<CommandAppResult> RunWithExceptionAsync<T>(string[] args)
            where T : Exception
        {
            Builder.WithTestConfiguration(c => {
                c.ConfigureConsole(Console);
                c.SetInterceptor(_commandIntercept);
                c.PropagateExceptions();
            });

            try
            {
                var exitCode = await Builder.Build().RunAsync(args);
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
    }
}
