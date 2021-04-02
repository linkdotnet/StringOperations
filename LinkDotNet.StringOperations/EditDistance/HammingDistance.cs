using System;

namespace LinkDotNet.StringOperations.EditDistance
{
    public static partial class EditDistances
    {
        public static int GetHammingDistance(this string one, string two, bool ignoreCase = false)
        {
            if (string.IsNullOrEmpty(one))
            {
                throw new ArgumentNullException(nameof(one));
            }
            
            if (string.IsNullOrEmpty(two))
            {
                throw new ArgumentNullException(nameof(two));
            }

            var cost = 0;
            for (var i = 0; i < one.Length; i++)
            {
                if (i >= two.Length)
                {
                    cost++;
                    continue;
                }

                var characterEqual = ignoreCase
                    ? char.ToUpperInvariant(one[i]) == char.ToUpperInvariant(two[i])
                    : one[i] == two[i];

                if (!characterEqual)
                {
                    cost++;
                }
            }

            return cost;
        }
    }
}