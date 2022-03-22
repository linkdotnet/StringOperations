using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace LinkDotNet.StringOperations.Compression;

public static class LempelZivWelch
{
    private const int AlphabetSize = 256;

    public static ImmutableArray<int> Encode(ReadOnlySpan<char> text)
    {
        if (text.IsEmpty)
        {
            return new ImmutableArray<int>();
        }

        var table = CreateEncodeTable();

        var code = AlphabetSize;
        var outputArray = new List<int>();
        var p = text[0].ToString();
        var c = string.Empty;

        for (var i = 0; i < text.Length; i++)
        {
            if (i != text.Length - 1)
            {
                c += text[i + 1];
            }

            var isPatternKnown = table.ContainsKey(p + c);
            if (isPatternKnown)
            {
                p += c;
            }
            else
            {
                AddNewCombinationToDictionary();
            }

            c = string.Empty;
        }

        outputArray.Add(table[p]);

        return outputArray.ToImmutableArray();

        void AddNewCombinationToDictionary()
        {
            outputArray.Add(table[p]);
            table[p + c] = code;
            code++;
            p = c;
        }
    }

    public static string Decode(ImmutableArray<int> decodedText)
    {
        var table = CreateDecodeTable();
        var decodedTextSpan = decodedText.AsSpan();
        var current = decodedTextSpan[0];
        var outputString = new StringBuilder();
        var decodedSubString = table[current];
        outputString.Append(decodedSubString);
        var c = decodedSubString[0].ToString();
        var count = AlphabetSize;

        for (var i = 0; i < decodedTextSpan.Length - 1; i++)
        {
            var code = decodedTextSpan[i + 1];

            if (!table.ContainsKey(code))
            {
                decodedSubString = table[current] + c;
            }
            else
            {
                decodedSubString = table[code];
            }

            outputString.Append(decodedSubString);
            c = decodedSubString[0].ToString();
            table[count] = table[current] + c;
            count++;
            current = code;
        }

        return outputString.ToString();
    }

    private static Dictionary<string, int> CreateEncodeTable()
    {
        var dictionary = new Dictionary<string, int>();

        for (var i = 0; i < AlphabetSize; i++)
        {
            dictionary[((char)i).ToString()] = i;
        }

        return dictionary;
    }

    private static Dictionary<int,string> CreateDecodeTable()
    {
        var dictionary = new Dictionary<int, string>();

        for (var i = 0; i < AlphabetSize; i++)
        {
            dictionary[i] = ((char)i).ToString();
        }

        return dictionary;
    }
}