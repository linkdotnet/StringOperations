using System;

namespace LinkDotNet.StringOperations.Search
{
    public static class BoyerMoore
    {
        private const int AlphabetSize = 256;
        
        public static void FindAll(ReadOnlySpan<char> text, ReadOnlySpan<char> word, bool ignoreCase = false)
        {
            var wordLength = word.Length;
            var textLength = text.Length;

            var badCharacterTable = GetBadCharacterTable(text, ignoreCase);

            var shift = 0;
            while (shift <= textLength - wordLength)
            {
                var index = word.Length - 1;
                
                index = ReduceIndexWhileMatchAtShift(text, word, ignoreCase, index, shift);

                if (index < 0)
                {
                    Console.WriteLine($"Found at {shift}");
                    shift = ShiftPatternToNextCharacterWithLastOccurrenceOfPattern(text, shift, wordLength, textLength, badCharacterTable, ignoreCase);
                }
                else
                {
                    shift = ShiftPatternAfterBadCharacter(text, shift, index, badCharacterTable, ignoreCase);
                }
            }
        }

        private static Span<int> GetBadCharacterTable(ReadOnlySpan<char> text, bool ignoreCase)
        {
            var table = new Span<int>(new int[AlphabetSize]);
            table.Fill(-1);

            for (var i = 0; i < text.Length; i++)
            {
                var character = ignoreCase ? char.ToUpperInvariant(text[i]) : text[i];
                table[character] = i;
            }

            return table;
        }

        private static int ShiftPatternAfterBadCharacter(ReadOnlySpan<char> text, int shift, int index, Span<int> badCharacterTable, bool ignoreCase)
        {
            var character = ignoreCase ? char.ToUpperInvariant(text[shift + index]) : text[shift + index];
            return shift + Math.Max(1, index - badCharacterTable[character]);
        }

        private static int ReduceIndexWhileMatchAtShift(ReadOnlySpan<char> text, ReadOnlySpan<char> word, bool ignoreCase, int index, int shift)
        {
            while (index >= 0 && CharacterEqual(text, word, ignoreCase, shift + index, index))
            {
                index--;
            }

            return index;
        }

        private static bool CharacterEqual(ReadOnlySpan<char> text, ReadOnlySpan<char> pattern, bool ignoreCase, int positionInText,
            int positionInPattern)
        {
            var characterEqual = ignoreCase
                ? char.ToUpperInvariant(text[positionInText]) == char.ToUpperInvariant(pattern[positionInPattern])
                : text[positionInText] == pattern[positionInPattern];
            return characterEqual;
        }

        private static int ShiftPatternToNextCharacterWithLastOccurrenceOfPattern(ReadOnlySpan<char> text, int shift,
            int wordLength, int textLength, Span<int> badCharacterTable, bool ignoreCase)
        {
            var character = ignoreCase ? char.ToUpperInvariant(text[shift + wordLength]) : text[shift + wordLength];
            
            return shift + (shift + wordLength < textLength
                ? wordLength - badCharacterTable[character]
                : 1);
        }
    }
}