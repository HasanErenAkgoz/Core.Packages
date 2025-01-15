using System.Text;

namespace Core.Packages.Domain.Security.TwoFactorAuth;

public static class Base32Encoding
{
    private const string Base32Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

    public static string ToString(byte[] data)
    {
        if (data == null || data.Length == 0)
            return string.Empty;

        var result = new StringBuilder((data.Length * 8 + 4) / 5);
        int buffer = data[0];
        int next = 1;
        int bitsLeft = 8;

        while (bitsLeft > 0 || next < data.Length)
        {
            if (bitsLeft < 5)
            {
                if (next < data.Length)
                {
                    buffer <<= 8;
                    buffer |= data[next++];
                    bitsLeft += 8;
                }
                else
                {
                    int pad = 5 - bitsLeft;
                    buffer <<= pad;
                    bitsLeft += pad;
                }
            }

            int index = 0x1F & (buffer >> (bitsLeft - 5));
            bitsLeft -= 5;
            result.Append(Base32Alphabet[index]);
        }

        return result.ToString();
    }

    public static byte[] FromString(string base32String)
    {
        if (string.IsNullOrEmpty(base32String))
            return Array.Empty<byte>();

        base32String = base32String.Trim().ToUpperInvariant();

        var result = new List<byte>();
        int buffer = 0;
        int bitsLeft = 0;

        foreach (char c in base32String)
        {
            int value = Base32Alphabet.IndexOf(c);
            if (value < 0)
                throw new ArgumentException("Invalid Base32 character.");

            buffer <<= 5;
            buffer |= value;
            bitsLeft += 5;

            if (bitsLeft >= 8)
            {
                result.Add((byte)(buffer >> (bitsLeft - 8)));
                bitsLeft -= 8;
            }
        }

        return result.ToArray();
    }
} 