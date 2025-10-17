//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Samples.Common.Services;

public interface IDisplayWriter
{
    public VerbosityLevel Verbosity { get; set; }

    public void MarkupSummary(string text = "");

    public void MarkupIntermediate(string text = "");

    public void MarkupDetailed(string text = "");

    public void WriteSummary(string text = "");

    public void WriteIntermediate(string text = "");

    public void WriteDetailed(string text = "");
}
