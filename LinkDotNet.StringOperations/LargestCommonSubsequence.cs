using System;

namespace LinkDotNet.StringOperations
{
    public static partial class EditDistances
    {
        public static int GetLargestCommonSubsequenceLength(this string one, string two, bool ignoreCase = false)
        {
            return one.GetLargestCommonSubsequence(two, ignoreCase).Length;
        }
        
        public static string GetLargestCommonSubsequence(this string one, string two, bool ignoreCase = false)
        {
            if (one == null || two == null)
            {
                return null;
            }

            var lcsMatrix = CreateLargestCommonSubsequenceMatrix(one, two, ignoreCase);
            return GetLongestCommonSubsequenceBackTrack(lcsMatrix, one, two, one.Length, two.Length, ignoreCase);
        }

        private static int[,] CreateLargestCommonSubsequenceMatrix(string one, string two, bool ignoreCase)
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
}