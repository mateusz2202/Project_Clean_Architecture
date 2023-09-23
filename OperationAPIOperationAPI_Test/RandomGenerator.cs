using System;
using System.Linq;
using System.Text;

namespace OperationAPIOperationAPI_Test;

public static class RandomGenerator
{
    private static readonly Random _random = new();
    public static int RandomNumber(int min, int max)
        => _random.Next(min, max);

    public static string RandomString(int size)
    {
        var builder = new StringBuilder(size);

        char offset;
        const int lettersOffset = 26;

        for (var i = 0; i < size; i++)
        {
            if (_random.Next(2) == 0)
                offset = (char)65;
            else
                offset = (char)97;

            var @char = (char)_random.Next(offset, offset + lettersOffset);
            builder.Append(@char);
        }

        return builder.ToString();
    }

    public static string RandomPassword()
        => new StringBuilder().Append(RandomString(4))
        .Append(string.Join("", Enumerable.Repeat(() => RandomNumber(1000, 9999), 4)))
        .Append(RandomString(2))
        .ToString();

}
