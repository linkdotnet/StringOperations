using System.Collections.Generic;
using System.Linq;

namespace LinkDotNet.StringOperations.Search
{
    public static class KnuthMorrisPratt
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
            
            var positionInText = 0;
            var positionInPattern = 0;

            var knuthMorrisPrattTable = CreateTable(text, ignoreCase);

            while (positionInText < text.Length)
            {
                var characterEqual = CharacterEqual(text, pattern, ignoreCase, positionInText, positionInPattern);

                if (characterEqual)
                {
                    positionInText++;
                    positionInPattern++;

                    if (positionInPattern == pattern.Length)
                    {
                        var index = positionInText - positionInPattern;
                        yield return index;
                        
                        positionInPattern = knuthMorrisPrattTable[positionInPattern];

                        if (abortOnFirstOccurence)
                        {
                            yield break;
                        }
                    }
                }
                else
                {
                    positionInPattern = knuthMorrisPrattTable[positionInPattern];
                    if (positionInPattern < 0)
                    {
                        positionInText++;
                        positionInPattern++;
                    }
                }
            }
        }

        private static int[] CreateTable(string text, bool ignoreCase)
        {
            var table = new int[text.Length];
            table[0] = -1;
            var position = 1;
            var candidate = 0;

            while (position < text.Length)
            {
                var characterEqual = CharacterEqual(text, text, ignoreCase, position, candidate);
                if (characterEqual)
                {
                    table[position] = table[candidate];
                }
                else
                {
                    table[position] = candidate;
                    while (candidate >= 0 && !CharacterEqual(text, text, ignoreCase, position, candidate))
                    {
                        candidate = table[candidate];
                    }
                }

                position++;
                candidate++;
            }

            table[position - 1] = candidate;
            return table;
        }

        private static bool CharacterEqual(string text, string pattern, bool ignoreCase, int positionInText,
            int positionInPattern)
        {
            var characterEqual = ignoreCase
                ? char.ToUpperInvariant(text[positionInText]) == char.ToUpperInvariant(pattern[positionInPattern])
                : text[positionInText] == pattern[positionInPattern];
            return characterEqual;
        }
    }
}