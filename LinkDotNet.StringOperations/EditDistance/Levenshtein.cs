using System;

namespace LinkDotNet.StringOperations
{
    public static partial class EditDistances
    {
        public static int GetLevenshteinDistance(this string one, string two, bool ignoreCase = false, int substitutionCost = 1, int abortCost = int.MaxValue)
        {
            AssertValuesNotNull(one, two);
            if (one == string.Empty)
            {
                return two.Length;
            }

            if (two == string.Empty)
            {
                return one.Length;
            }
            
            var matrix = CreateLevenshteinMatrix(one, two);

            for (var i = 1; i <= one.Length; i++)
            {
                for (var j = 1; j <= two.Length; j++)
                {
                    var characterEqual = CheckCharacterEqual(one, two, ignoreCase, i, j);
                    
                    var substituteCost = characterEqual ? 0 : substitutionCost;
                    var deleteCost = matrix[i - 1, j] + 1;
                    var insertCost = matrix[i, j - 1] + 1;
                    var completeSubstitutionCost = matrix[i - 1, j - 1] + substituteCost;
                    matrix[i, j] = Math.Min(Math.Min(deleteCost, insertCost), completeSubstitutionCost);
                    
                    if (matrix[i, j] >= abortCost)
                    {
                        return abortCost;
                    }

                }
            }

            return matrix[one.Length, two.Length];
        }

        private static void AssertValuesNotNull(string one, string two)
        {
            if (one == null)
            {
                throw new ArgumentNullException(nameof(one));
            }

            if (two == null)
            {
                throw new ArgumentNullException(nameof(two));
            }
        }

        private static int[,] CreateLevenshteinMatrix(string one, string two)
        {
            var matrix = new int[one.Length + 1, two.Length + 1];

            for (var i = 0; i <= one.Length; i++)
            {
                matrix[i, 0] = i;
            }

            for (var j = 0; j <= two.Length; j++)
            {
                matrix[0, j] = j;
            }

            return matrix;
        }

        private static bool CheckCharacterEqual(string one, string two, bool ignoreCase, int i, int j)
        {
            var characterEqual = ignoreCase
                ? char.ToUpperInvariant(one[i - 1]) == char.ToUpperInvariant(two[j - 1])
                : one[i - 1] == two[j - 1];
            return characterEqual;
        }
    }
}