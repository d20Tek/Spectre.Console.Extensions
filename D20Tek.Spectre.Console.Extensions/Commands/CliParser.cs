﻿namespace D20Tek.Spectre.Console.Extensions.Commands;

internal static class CliParser
{
    public static IEnumerable<string> SplitCommandLine(string commandLine)
    {
        var memory = commandLine.AsMemory();

        var startTokenIndex = 0;

        var pos = 0;

        var seeking = Boundary.TokenStart;
        var seekingQuote = Boundary.QuoteStart;

        while (pos < memory.Length)
        {
            var c = memory.Span[pos];

            if (char.IsWhiteSpace(c))
            {
                if (seekingQuote == Boundary.QuoteStart)
                {
                    switch (seeking)
                    {
                        case Boundary.WordEnd:
                            yield return CurrentToken();
                            startTokenIndex = pos;
                            seeking = Boundary.TokenStart;
                            break;

                        case Boundary.TokenStart:
                            startTokenIndex = pos;
                            break;
                    }
                }
            }
            else if (c == '\"')
            {
                if (seeking == Boundary.TokenStart)
                {
                    switch (seekingQuote)
                    {
                        case Boundary.QuoteEnd:
                            yield return CurrentToken();
                            startTokenIndex = pos;
                            seekingQuote = Boundary.QuoteStart;
                            break;

                        case Boundary.QuoteStart:
                            startTokenIndex = pos + 1;
                            seekingQuote = Boundary.QuoteEnd;
                            break;
                    }
                }
                else
                {
                    switch (seekingQuote)
                    {
                        case Boundary.QuoteEnd:
                            seekingQuote = Boundary.QuoteStart;
                            break;

                        case Boundary.QuoteStart:
                            seekingQuote = Boundary.QuoteEnd;
                            break;
                    }
                }
            }
            else if (seeking == Boundary.TokenStart && seekingQuote == Boundary.QuoteStart)
            {
                seeking = Boundary.WordEnd;
                startTokenIndex = pos;
            }

            Advance();

            if (IsAtEndOfInput())
            {
                switch (seeking)
                {
                    case Boundary.TokenStart:
                        break;
                    default:
                        yield return CurrentToken();
                        break;
                }
            }
        }

        void Advance() => pos++;

        string CurrentToken()
        {
            return memory.Slice(startTokenIndex, IndexOfEndOfToken()).ToString().Replace("\"", "");
        }

        int IndexOfEndOfToken() => pos - startTokenIndex;

        bool IsAtEndOfInput() => pos == memory.Length;
    }

    private enum Boundary
    {
        TokenStart,
        WordEnd,
        QuoteStart,
        QuoteEnd
    }
}