using Xunit;

namespace LinkDotNet.StringOperations.UnitTests
{
    public class EditDistancesTests
    {
        [Theory]
        [InlineData("Hello", "Hallo", false, "Hllo")]
        [InlineData("HeLlO", "hallo", true, "HLlO")]
        [InlineData("abc", "cbe", false, "b")]
        [InlineData("", "", false, "")]
        [InlineData("Test", "", false, "")]
        public void Test1(string one, string two, bool ignoreCase, string expected)
        {
            var actual = one.GetLargestCommonSubsequence(two, ignoreCase);
            
            Assert.Equal(expected, actual);
        }
    }
}