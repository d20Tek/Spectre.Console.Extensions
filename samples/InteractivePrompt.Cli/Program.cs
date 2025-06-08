using D20Tek.Spectre.Console.Extensions;
using InteractivePrompt.Cli;

return await new CommandAppBuilder().WithDIContainer()
                                    .WithStartup<Startup>()
                                    .WithDefaultCommand<InteractiveCommand>()
                                    .Build()
                                    .RunAsync(args);
