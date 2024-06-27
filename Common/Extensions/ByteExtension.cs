using Common.Exceptions;

namespace Common.Extensions;

public static class ByteExtension
{
    public static bool IsEqual(this byte[] orig, byte[] other)
    {
        if (orig.Length != other.Length)
            return false;

        for (var x = 0; x < orig.Length; ++x)
            if (orig[x] != other[x])
                return false;

        return true;
    }

    public static byte ToByte(this byte? x, string number)
    {
        return x ?? throw new InnerException($"{number}");
    }
}
