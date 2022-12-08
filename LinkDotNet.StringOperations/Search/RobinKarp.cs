using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkDotNet.StringOperations.Search;

public static class RobinKarp
{
    private const int Base = 7;

    public static bool HasPattern(string text, string word, bool ignoreCase = false) =>
        FindAll(text, word).Any();

    public static IEnumerable<int> FindAll(string text, string pattern)
    {
        var n = text.Length;
        var m = pattern.Length;

        var patternHash = GetPatternHash(pattern, m);
        var textHash = GetTextHash(text, m);

        // Check if the hash of the pattern and the first m characters of the text match
        if (patternHash == textHash)
        {
            yield return 0;
        }

        // Hash the remaining characters of the text
        for (var i = m; i < n; i++)
        {
            // Remove the first character from the previous hash
            textHash -= text[i - m] * (int)Math.Pow(Base, m - 1);

            // Add the next character to the hash
            textHash += text[i] * (int)Math.Pow(Base, m - 1);

            // Check if the hash of the pattern and the current m characters of the text match
            if (patternHash == textHash)
            {
                yield return i - m + 1;
            }
        }

        // Return -1 if the pattern was not found
        yield return -1;
    }

    private static int GetPatternHash(string pattern, int m)
    {
        var patternHash = 0;
        for (var i = 0; i < m; i++)
        {
            patternHash += pattern[i] * (int)Math.Pow(Base, i);
        }

        return patternHash;
    }

    private static int GetTextHash(string text, int m)
    {
        var textHash = 0;
        for (var i = 0; i < m; i++)
        {
            textHash += text[i] * (int)Math.Pow(Base, i);
        }

        return textHash;
    }
}