//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace D20Tek.Spectre.Console.Extensions.Testing;

/// <summary>
/// This class contains the metadata configured for a Command.
/// </summary>
public class CommandMetadata
{
    /// <summary>
    /// Gets the command name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the list of command aliases.
    /// </summary>
    public HashSet<string> Aliases { get; }

    /// <summary>
    /// Gets the command description, if one exists.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets optional command data object.
    /// </summary>
    public object? Data { get; set; }

    /// <summary>
    /// Gets the Type for this command.
    /// </summary>
    public Type? CommandType { get; }

    /// <summary>
    /// Gets the settings Type for this command.
    /// </summary>
    public Type? SettingsType { get; }

    /// <summary>
    /// Gets the delegate for this command, if one exists.
    /// Only available for delegate commands.
    /// </summary>
    public Func<CommandContext, CommandSettings, int>? Delegate { get; }

    /// <summary>
    /// Gets the async delegate for this command, if one exists.
    /// Only available for async delegate commands.
    /// </summary>
    public Func<CommandContext, CommandSettings, Task<int>>? AsyncDelegate { get; }

    /// <summary>
    /// Gets whether or not this command is the default command for the console app.
    /// </summary>
    public bool IsDefaultCommand { get; }

    /// <summary>
    /// Gets whether or not this command is hidden.
    /// </summary>
    public bool IsHidden { get; set; }

    /// <summary>
    /// Gets the child commands for this command... if it was configured with nested commands.
    /// </summary>
    public IList<CommandMetadata> Children { get; }

    /// <summary>
    /// Gets the list of examples for this command.
    /// </summary>
    public IList<string[]> Examples { get; }

    /// <summary>
    /// Constuctor that builds the CommandMetadata with appropriate initial values.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="commandType"></param>
    /// <param name="settingsType"></param>
    /// <param name="delegate"></param>
    /// <param name="isDefaultCommand"></param>
    private CommandMetadata(
        string name,
        Type? commandType,
        Type? settingsType,
        Func<CommandContext, CommandSettings, int>? @delegate,
        bool isDefaultCommand)
    {
        Name = name;
        Aliases = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        CommandType = commandType;
        SettingsType = settingsType;
        Delegate = @delegate;
        IsDefaultCommand = isDefaultCommand;

        Children = [];
        Examples = [];
    }

    /// <summary>
    /// Constuctor that builds the CommandMetadata with appropriate initial values.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="settingsType"></param>
    /// <param name="delegate"></param>
    private CommandMetadata(
        string name,
        Type? settingsType,
        Func<CommandContext, CommandSettings, Task<int>>? @delegate)
    {
        Name = name;
        Aliases = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        SettingsType = settingsType;
        AsyncDelegate = @delegate;
        IsDefaultCommand = false;

        Children = [];
        Examples = [];
    }

    /// <summary>
    /// Creates the command metadata from a branch with multiple commands.
    /// </summary>
    /// <param name="settings">Command settings type.</param>
    /// <param name="name">Command name.</param>
    /// <returns>Newly created command metadata.</returns>
    public static CommandMetadata FromBranch(Type settings, string name) => new(name, null, settings, null, false);

    /// <summary>
    /// Creates the command metadata from a branch with multiple commands.
    /// </summary>
    /// <typeparam name="TSettings">Command settings type.</typeparam>
    /// <param name="name">Command name.</param>
    /// <returns>Newly created command metadata.</returns>
    public static CommandMetadata FromBranch<TSettings>(string name)
        where TSettings : CommandSettings =>
        new(name, null, typeof(TSettings), null, false);

    /// <summary>
    /// Creates the command metadata from a command type.
    /// </summary>
    /// <typeparam name="TCommand">Command type.</typeparam>
    /// <param name="name">Command name.</param>
    /// <param name="isDefaultCommand">Whether this is the default command.</param>
    /// <returns>Newly created command metadata.</returns>
    public static CommandMetadata FromType<TCommand>(string name, bool isDefaultCommand = false)
        where TCommand : class, ICommand
    {
        var settingsType = GetSettingsType(typeof(TCommand));
        return new(name, typeof(TCommand), settingsType, null, isDefaultCommand);
    }

    /// <summary>
    /// Creates the command metadata from command that uses a delegate.
    /// </summary>
    /// <typeparam name="TSettings">Command settings type.</typeparam>
    /// <param name="name">Command name.</param>
    /// <param name="delegate">Delegate method.</param>
    /// <returns>Newly created command metadata.</returns>
    public static CommandMetadata FromDelegate<TSettings>(
        string name,
        Func<CommandContext, CommandSettings, int>? @delegate = null)
        where TSettings : CommandSettings =>
        new(name, null, typeof(TSettings), @delegate, false);

    /// <summary>
    /// Creates the command metadata from command that uses an async delegate.
    /// </summary>
    /// <typeparam name="TSettings">Command settings type.</typeparam>
    /// <param name="name">Command name.</param>
    /// <param name="asyncDelegate">Delegate method.</param>
    /// <returns>Newly created command metadata.</returns>
    public static CommandMetadata FromAsyncDelegate<TSettings>(
        string name,
        Func<CommandContext, CommandSettings, Task<int>>? asyncDelegate = null)
        where TSettings : CommandSettings =>
        new(name, typeof(TSettings), asyncDelegate);

    private static Type? GetSettingsType(Type commandType)
    {
        if (typeof(ICommand).GetTypeInfo().IsAssignableFrom(commandType) &&
            GetGenericTypeArguments(commandType, typeof(ICommand<>), out var result))
        {
            return result[0];
        }

        return null;
    }

    private static bool GetGenericTypeArguments(
        Type? type,
        Type genericType,
        [NotNullWhen(true)] out Type[]? genericTypeArguments)
    {
        while (type != null)
        {
            foreach (var @interface in type.GetTypeInfo().GetInterfaces())
            {
                if (!@interface.GetTypeInfo().IsGenericType || @interface.GetGenericTypeDefinition() != genericType)
                {
                    continue;
                }

                genericTypeArguments = @interface.GenericTypeArguments;
                return true;
            }

            type = type.GetTypeInfo().BaseType;
        }

        genericTypeArguments = null;
        return false;
    }
}
