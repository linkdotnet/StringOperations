using LinkDotNet.StringOperations.DataStructure;
using Xunit;

namespace LinkDotNet.StringOperations.UnitTests
{
    public class RopeTests
    {
        [Fact]
        public void ShouldCreateAndDisplayRope()
        {
            const string sentence = "Hello_my_name_is_Simon";
            var rope = Rope.Create(sentence, 4);

            var output = rope.ToString();
            
            Assert.Equal(sentence, output);
        }

        [Fact]
        public void ShouldConcat()
        {
            var left = Rope.Create("Hello");
            var right = Rope.Create("World");

            var concat = (left + right).ToString();
            
            Assert.Equal("HelloWorld", concat);
        }

        [Fact]
        public void ShouldConcatWithStrings()
        {
            var left = Rope.Create("Hello");
            var right = Rope.Create("World");

            var first = left + "World";
            var second = "Hello" + right;
            
            Assert.Equal("HelloWorld", first.ToString());
            Assert.Equal("HelloWorld", second.ToString());
        }

        [Fact]
        public void ShouldGetIndex()
        {
            const string text = "0123456789";
            var rope = Rope.Create(text, 2);

            Assert.Equal(text[5], rope[5]);
        }

        [Fact]
        public void ShouldGetIndexAfterRebalance()
        {
            var rope1 = Rope.Create("012");
            var rope2 = Rope.Create("345");
            var rope = rope1 + rope2;

            var index = rope[3];
            
            Assert.Equal('3', index);
        }
    }
}