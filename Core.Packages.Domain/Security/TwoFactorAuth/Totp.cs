using System.Security.Cryptography;

namespace Core.Packages.Domain.Security.TwoFactorAuth;

public class Totp
{
    private readonly byte[] _secretKey;
    private readonly int _step;
    private readonly int _digits;
    private readonly HMACSHA1 _hmac;

    public Totp(byte[] secretKey, int step = 30, int digits = 6)
    {
        _secretKey = secretKey;
        _step = step;
        _digits = digits;
        _hmac = new HMACSHA1(secretKey);
    }

    public string GenerateTotp()
    {
        return GenerateTotp(GetCurrentTimeStep());
    }

    public bool VerifyTotp(string code, out long timeStepMatched, VerificationWindow window)
    {
        timeStepMatched = 0;
        if (string.IsNullOrEmpty(code) || code.Length != _digits)
            return false;

        long currentTimeStep = GetCurrentTimeStep();

        for (long i = currentTimeStep - window.PastTimeSteps; i <= currentTimeStep + window.FutureTimeSteps; i++)
        {
            string generatedCode = GenerateTotp(i);
            if (generatedCode == code)
            {
                timeStepMatched = i;
                return true;
            }
        }

        return false;
    }

    private string GenerateTotp(long timeStep)
    {
        byte[] timeBytes = BitConverter.GetBytes(timeStep);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(timeBytes);

        byte[] hash;
        lock (_hmac)
        {
            hash = _hmac.ComputeHash(timeBytes);
        }

        int offset = hash[^1] & 0x0F;
        int binary =
            ((hash[offset] & 0x7F) << 24) |
            ((hash[offset + 1] & 0xFF) << 16) |
            ((hash[offset + 2] & 0xFF) << 8) |
            (hash[offset + 3] & 0xFF);

        int password = binary % (int)Math.Pow(10, _digits);
        return password.ToString().PadLeft(_digits, '0');
    }

    private long GetCurrentTimeStep()
    {
        var unixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        return unixTime / _step;
    }
} 