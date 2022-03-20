using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkDotNet.StringOperations.Search;

public static class ZAlgorithm
{
    public static bool HasPattern(string text, string word, bool ignoreCase = false) =>
        FindAll(text, word, ignoreCase, true).Any();

    public static IEnumerable<int> FindAll(string text, string pattern,
        bool ignoreCase = false, bool abortOnFirstOccurence = false)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern))
        {
            yield break;
        }

        if (text.Length < pattern.Length)
        {
            yield break;
        }

        var concat = ignoreCase
            ? (pattern + "$" + text).ToUpper()
            : pattern + "$" + text;

        var zArray = CreateZArray(concat.AsSpan());

        for (var i = 0; i < concat.Length; i++)
        {
            if (zArray[i] == pattern.Length)
            {
                yield return i - pattern.Length - 1;

                if (abortOnFirstOccurence)
                {
                    yield break;
                }
            }
        }

    }

    private static int[] CreateZArray(ReadOnlySpan<char> concat)
    {
        var zArray = new int[concat.Length];
        var left = 0;
        var right = 0;

        for (var current = 1; current < concat.Length; current++)
        {
            if (current > right)
            {
                left = right = current;

                while (right < concat.Length && concat[right - left] == concat[right])
                {
                    right++;
                }

                zArray[current] = right - left;
                right--;
            }
            else
            {
                var k = current - left;

                if (zArray[k] < right - current + 1)
                {
                    zArray[current] = zArray[k];
                }
                else
                {
                    left = current;
                    while (right < current && concat[right - left] == concat[right])
                    {
                        right++;
                    }

                    zArray[current] = right - left;
                    right--;
                }
            }
        }

        return zArray;
    }
}