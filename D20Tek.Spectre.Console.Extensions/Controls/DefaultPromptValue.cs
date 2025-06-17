namespace D20Tek.Spectre.Console.Extensions.Controls;

internal sealed class DefaultPromptValue<T>
{
    public T Value { get; }

    public DefaultPromptValue(T value)
    {
        Value = value;
    }
}