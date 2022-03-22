using LinkDotNet.StringOperations.Compression;
using Xunit;

namespace LinkDotNet.StringOperations.UnitTests;

public class LempelZivWelchTests
{
    [Fact]
    public void ShouldEncodeAndDecode()
    {
        const string sentence = "Hey my name is Steven";
        var encoded = LempelZivWelch.Encode(sentence);

        var output = LempelZivWelch.Decode(encoded);

        Assert.Equal(sentence, output);
    }

    [Fact]
    public void ShouldCompressText()
    {
        const string sentence = "Here is your text, which consists out of multiple words. They, the words, can appear again";
        var output = LempelZivWelch.Encode(sentence);

        Assert.True(output.Length < sentence.Length);
    }
}