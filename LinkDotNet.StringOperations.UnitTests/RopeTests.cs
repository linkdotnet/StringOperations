using System;
using LinkDotNet.StringOperations.DataStructure;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

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
        
        public void ShouldGetIndex()
        {
            var text = "That is a very nice text";
            var rope = Rope.Create(text, 5);

            Assert.Equal(text[5], rope[5]);
        }
    }
}