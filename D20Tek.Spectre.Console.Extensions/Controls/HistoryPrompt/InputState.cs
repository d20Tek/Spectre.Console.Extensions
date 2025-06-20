using System.Text;

namespace D20Tek.Spectre.Console.Extensions.Controls.HistoryPrompt;

internal sealed record InputState(
    ReadLineRequest Request,
    StringBuilder Buffer,
    int CursorIndex,
    bool InsertMode,
    List<string> CompletionItems,
    int HistoryIndex,
    string? SavedHistory,
    bool Handled,
    bool Done)
{
    public static InputState Initialize(ReadLineRequest request) =>
        new(request, new StringBuilder(), 0, true, [.. request.Items], -1, null, false, false);
}
