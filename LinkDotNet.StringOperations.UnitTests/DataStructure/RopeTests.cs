using System;
using LinkDotNet.StringOperations.DataStructure;
using Xunit;

namespace LinkDotNet.StringOperations.UnitTests.DataStructure;

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

    [Theory]
    [InlineData("HelloWorld", 4, "Hello", "World")]
    [InlineData("HelloWorld", 5, "HelloW", "orld")]
    [InlineData("HelloWorld", 6, "HelloWo", "rld")]
    [InlineData("0123456789", 2, "012", "3456789")]
    [InlineData("0123456789", 8, "012345678", "9")]
    [InlineData("0123456789", 0, "0", "123456789")]
    public void ShouldSplitRope(string word, int indexToSplit, string expectedLeftSide, string expectedRightSide)
    {
        var rope = Rope.Create(word);

        var splitPair = rope.Split(indexToSplit);

        Assert.Equal(expectedLeftSide, splitPair.Item1.ToString());
        Assert.Equal(expectedRightSide, splitPair.Item2.ToString());
    }

    [Fact]
    public void ShouldThrowExceptionWhenNegativeIndex()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Rope.Create("a").Split(-1));
    }

    [Fact]
    public void ShouldReturnLeftPartWhenCompleteLength()
    {
        var rope = Rope.Create("01234567");

        var pair = rope.Split(7);
            
        Assert.Equal("01234567", pair.Item1.ToString());
        Assert.Null(pair.Item2);
    }

    [Fact]
    public void ShouldInsertRope()
    {
        var rope1 = Rope.Create("Hello World");
        var rope2 = Rope.Create(" dear");

        var newRope = rope1.Insert(rope2, 4);
            
        Assert.Equal("Hello dear World", newRope.ToString());
    }

    [Fact]
    public void ShouldSplitAfterConcat()
    {
        var split = (Rope.Create("Hello") + "World").Split(6);
            
        Assert.Equal("HelloWo", split.Item1.ToString());
        Assert.Equal("rld", split.Item2.ToString());
    }

    [Fact]
    public void ShouldDelete()
    {
        var rope = Rope.Create("0123456789");
            
        var newRope = rope.Delete(3, 3);
            
        Assert.Equal("0126789", newRope.ToString());
    }
        
    [Fact]
    public void ShouldHavePositiveIndexAndLength()
    {
        var rope = Rope.Create("1");

        Assert.Throws<ArgumentOutOfRangeException>(() => rope.Delete(-1, 2));
        Assert.Throws<ArgumentOutOfRangeException>(() => rope.Delete(1, 0));
    }
}