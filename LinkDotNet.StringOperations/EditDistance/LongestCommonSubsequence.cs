using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkDotNet.StringOperations.EditDistance;

public static partial class EditDistances
{
    public static string GetClosestWord(this string input, bool ignoreCase, params string[] words) =>
        input.GetClosestWords(1, ignoreCase, words).FirstOrDefault();
        
    public static IEnumerable<string> GetClosestWords(this string input, int count, bool ignoreCase,
        params string[] words)
    {
        if (input == null)
        {
            return Array.Empty<string>();
        }

        if (words == null || !words.Any())
        {
            return Array.Empty<string>();
        }

        var wordToSimilarity = new Dictionary<string, int>();
        foreach (var word in words.Distinct().Where(w => w != null))
        {
            wordToSimilarity[word] = word.GetLongestCommonSubsequence(input, ignoreCase).Length;
        }
            
        var sortedWords = wordToSimilarity.ToList();
        sortedWords.Sort((a, b) => b.Value.CompareTo(a.Value));

        return sortedWords.Select(s => s.Key).Take(count);
    }
    public static string GetLongestCommonSubsequence(this string one, string two, bool ignoreCase = false)
    {
        if (one == null || two == null)
        {
            return null;
        }

        var lcsMatrix = CreateLongestCommonSubsequenceMatrix(one, two, ignoreCase);
        return GetLongestCommonSubsequenceBackTrack(lcsMatrix, one, two, one.Length, two.Length, ignoreCase);
    }

    private static int[,] CreateLongestCommonSubsequenceMatrix(string one, string two, bool ignoreCase)
    {
        var lcsMatrix = new int[one.Length + 1, two.Length + 1];
            
        for (var i = 1; i <= one.Length; i++)
        {
            for (var j = 1; j <= two.Length; j++)
            {
                var characterEqual = ignoreCase
                    ? char.ToUpperInvariant(one[i - 1]) == char.ToUpperInvariant(two[j - 1])
                    : one[i - 1] == two[j - 1];
                if (characterEqual)
                {
                    lcsMatrix[i, j] = lcsMatrix[i - 1, j - 1] + 1;
                }
                else
                {
                    lcsMatrix[i, j] = Math.Max(lcsMatrix[i - 1, j], lcsMatrix[i, j - 1]);
                }
            }
        }
            
        return lcsMatrix;
    }

    private static string GetLongestCommonSubsequenceBackTrack(int[,] lcsMatrix, string one, string two,
        int oneLength, int twoLength, bool ignoreCase)
    {
        if (oneLength == 0 || twoLength == 0)
        {
            return string.Empty;
        }

        var characterEqual = ignoreCase
            ? char.ToUpperInvariant(one[oneLength - 1]) == char.ToUpperInvariant(two[twoLength - 1])
            : one[oneLength - 1] == two[twoLength - 1];
        if (characterEqual)
        {
            return GetLongestCommonSubsequenceBackTrack(lcsMatrix, one, two, oneLength - 1, twoLength - 1,
                ignoreCase) + one[oneLength - 1];
        }

        if (lcsMatrix[oneLength, twoLength - 1] > lcsMatrix[oneLength - 1, twoLength])
        {
            return GetLongestCommonSubsequenceBackTrack(lcsMatrix, one, two, oneLength, twoLength - 1, ignoreCase);
        }

        return GetLongestCommonSubsequenceBackTrack(lcsMatrix, one, two, oneLength - 1, twoLength, ignoreCase);
    }
}