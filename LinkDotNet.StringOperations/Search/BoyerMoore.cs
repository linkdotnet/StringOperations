using System;
using System.Collections.Generic;

namespace LinkDotNet.StringOperations.Search;

public static class BoyerMoore
{
    private const int AlphabetSize = 256;

    public static IEnumerable<int> FindAll(string text, string word, bool ignoreCase = false, bool abortOnFirstOccurrence = false)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(word))
        {
            yield break;
        }

        if (text.Length < word.Length)
        {
            yield break;
        }

        var wordLength = word.Length;
        var textLength = text.Length;

        var badCharacterTable = GetBadCharacterTable(word, ignoreCase);

        var shift = 0;
        while (shift <= textLength - wordLength)
        {
            var index = word.Length - 1;

            index = ReduceIndexWhileMatchAtShift(text, word, ignoreCase, index, shift);

            if (index < 0)
            {
                yield return shift;
                if (abortOnFirstOccurrence)
                {
                    yield break;
                }

                shift = ShiftPatternToNextCharacterWithLastOccurrenceOfPattern(text, shift, wordLength, textLength, badCharacterTable, ignoreCase);
            }
            else
            {
                shift = ShiftPatternAfterBadCharacter(text, shift, index, badCharacterTable, ignoreCase);
            }
        }
    }

    private static int[] GetBadCharacterTable(string text, bool ignoreCase)
    {
        var table = new int[AlphabetSize];
        Array.Fill(table, -1);

        for (var i = 0; i < text.Length; i++)
        {
            var character = ignoreCase ? char.ToUpperInvariant(text[i]) : text[i];
            table[character] = i;
        }

        return table;
    }

    private static int ReduceIndexWhileMatchAtShift(string text, string word, bool ignoreCase, int index, int shift)
    {
        while (index >= 0 && CharacterEqual(text, word, ignoreCase, shift + index, index))
        {
            index--;
        }

        return index;
    }

    private static int ShiftPatternToNextCharacterWithLastOccurrenceOfPattern(string text, int shift,
        int wordLength, int textLength, Span<int> badCharacterTable, bool ignoreCase)
    {
        return shift + (shift + wordLength < textLength
            ? wordLength - badCharacterTable[GetCharacter()]
            : 1);

        char GetCharacter()
        {
            return ignoreCase ? char.ToUpperInvariant(text[shift + wordLength]) : text[shift + wordLength];
        }
    }

    private static int ShiftPatternAfterBadCharacter(string text, int shift, int index, int[] badCharacterTable, bool ignoreCase)
    {
        var character = ignoreCase ? char.ToUpperInvariant(text[shift + index]) : text[shift + index];
        return shift + Math.Max(1, index - badCharacterTable[character]);
    }

    private static bool CharacterEqual(ReadOnlySpan<char> text, ReadOnlySpan<char> pattern, bool ignoreCase, int positionInText,
        int positionInPattern)
    {
        var characterEqual = ignoreCase
            ? char.ToUpperInvariant(text[positionInText]) == char.ToUpperInvariant(pattern[positionInPattern])
            : text[positionInText] == pattern[positionInPattern];
        return characterEqual;
    }
}