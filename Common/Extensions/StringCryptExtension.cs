using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Common.Exceptions;
using Common.Helpers;

namespace Common.Extensions;

public static class StringCryptExtension
{
    public static int BitsCountInByte { get; } = 8;

    public static byte[] ToBytes(this string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    public static string DesEncrypt(this string str)
    {
        return str.DesEncrypt(AppConstants.Key, AppConstants.Iv);
    }

    public static string DesDecrypt(this string str)
    {
        return str.DesDecrypt(AppConstants.Key, AppConstants.Iv);
    }

    public static string DesEncrypt(this string str, byte[] keySource, byte[] ivSource)
    {
        return str.Encrypt(DES.Create(), keySource, ivSource);
    }

    public static string DesDecrypt(this string str, byte[] keySource, byte[] ivSource)
    {
        return str.Decrypt(DES.Create(), keySource, ivSource);
    }

    public static string Encrypt(this string str, SymmetricAlgorithm algorithm, byte[] keySource, byte[] ivSource)
    {
        var (key, iv) = SliceKeys(algorithm, keySource, ivSource);
        return str.SymmetricTransform(algorithm.CreateEncryptor, Encoding.Unicode.GetBytes, Convert.ToBase64String, key, iv);
    }

    public static string Decrypt(this string str, SymmetricAlgorithm algorithm, byte[] keySource, byte[] ivSource)
    {
        var (key, iv) = SliceKeys(algorithm, keySource, ivSource);
        return str.SymmetricTransform(algorithm.CreateDecryptor, Convert.FromBase64String, Encoding.Unicode.GetString, key, iv);
    }

    public static (byte[] Key, byte[] Iv) SliceKeys(SymmetricAlgorithm algorithm, byte[] keySource, byte[] ivSource)
    {
        var keyMinSize = algorithm.LegalKeySizes.FirstOrDefault()?.MinSize / BitsCountInByte
                         ?? throw new InnerException($"2662.");

        var ivMinSize = algorithm.LegalBlockSizes.FirstOrDefault()?.MinSize / BitsCountInByte
                        ?? throw new InnerException($"2663.");

        if (keySource.Length < keyMinSize)
            throw new InnerException($"2661. Длина ключа не достаточна. Необходимая длина = {keyMinSize}.");

        if (ivSource.Length < ivMinSize)
            throw new InnerException($"2664. Длина блока не достаточна. Необходимая длина = {ivMinSize}.");

        return (keySource.Take(keyMinSize).ToArray(), ivSource.Take(ivMinSize).ToArray());
    }

    public static string SymmetricTransform(this string str, Func<byte[], byte[], ICryptoTransform> createTransformer,
        Func<string, byte[]> strToBytes, Func<byte[], string> bytesToStr, byte[] key, byte[] iv)
    {
        var @in = strToBytes(str);
        var @out = createTransformer(key, iv).TransformFinalBlock(@in, 0, @in.Length);

        return bytesToStr(@out);
    }
}
