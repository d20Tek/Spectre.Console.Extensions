namespace D20Tek.Spectre.Console.Extensions.Commands;

internal static class Constants
{
    public const int S_OK = 0;

    public const int S_EXIT = unchecked((int)0x80000001);

    public const int E_FAIL = -1;

    public const string DefaultPromptPrefix = ">";

    public const string ExitCommand = "exit";

    public const string ExitCommandAbbr = "x";
}
