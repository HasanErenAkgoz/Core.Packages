using System.Security.Cryptography;
using Core.Packages.Application.CrossCuttingConcerns.Email;
using Core.Packages.Application.Services.Auth;
using Core.Packages.Domain.Enums;
using Core.Packages.Domain.Security.TwoFactorAuth;
using Core.Packages.Security.TwoFactorAuth.Models;
using Core.Packages.Security.TwoFactorAuth.Storage;
using Core.Packages.Security.TwoFactorAuth.Storage.Constants;
using Core.Packages.Security.TwoFactorAuth.Storage.Models;
using Microsoft.Extensions.Options;

namespace Core.Packages.Security.TwoFactorAuth.Services;

public class TwoFactorAuthService : ITwoFactorAuthService
{
    private readonly TwoFactorAuthOptions _options;
    private readonly ITwoFactorStore _store;
    private readonly IEmailService _emailService;

    public TwoFactorAuthService(
        IOptions<TwoFactorAuthOptions> options,
        ITwoFactorStore store,
        IEmailService emailService)
    {
        _options = options.Value;
        _store = store;
        _emailService = emailService;
    }

    public async Task<(string SecretKey, string QrCodeUri)> GenerateTotpSecretAsync(string userId, string email)
    {
        var secretKey = GenerateSecretKey();
        await _store.SaveTwoFactorDataAsync(new TwoFactorData
        {
            UserId = userId,
            SecretKey = secretKey,
            Type = TwoFactorType.Authenticator,
            IsEnabled = false
        });

        var base32Secret = Base32Encoding.ToString(secretKey);
        var qrCodeUri = GenerateQrCodeUri(email, base32Secret);

        return (base32Secret, qrCodeUri);
    }

    public async Task<bool> VerifyTotpCodeAsync(string userId, string code)
    {
        var data = await _store.GetTwoFactorDataAsync(userId);
        if (data?.SecretKey == null || !data.IsEnabled || data.Type != TwoFactorType.Authenticator)
            return false;

        if (await IsLockedOutAsync(userId))
            return false;

        var totp = new Totp(data.SecretKey, _options.CodeValidityPeriod);
        var isValid = totp.VerifyTotp(code, out _, new VerificationWindow(1, 1));

        if (!isValid)
            await IncrementFailedAttemptsAsync(userId);

        return isValid;
    }

    public async Task<bool> EnableTotpAsync(string userId, string code)
    {
        if (!await VerifyTotpCodeAsync(userId, code))
            return false;

        var data = await _store.GetTwoFactorDataAsync(userId);
        if (data == null) return false;

        data.IsEnabled = true;
        await _store.SaveTwoFactorDataAsync(data);
        return true;
    }

    public async Task<bool> DisableTotpAsync(string userId, string code)
    {
        if (!await VerifyTotpCodeAsync(userId, code))
            return false;

        await _store.DeleteAllDataAsync(userId);
        return true;
    }

    public async Task<List<string>> GenerateBackupCodesAsync(string userId)
    {
        var codes = Enumerable.Range(0, _options.BackupCodesCount)
            .Select(_ => GenerateRandomCode())
            .ToList();

        foreach (var code in codes)
        {
            await _store.SaveCodeAsync(new TwoFactorCode
            {
                UserId = userId,
                Code = code,
                ExpiresAt = DateTime.MaxValue,
                Type = TwoFactorType.Authenticator,
                Purpose = TwoFactorCodePurpose.Backup,
                IsUsed = false
            });
        }

        return codes;
    }

    public async Task<List<string>> GenerateRecoveryCodesAsync(string userId)
    {
        var codes = Enumerable.Range(0, _options.RecoveryCodesCount)
            .Select(_ => GenerateRandomCode())
            .ToList();

        foreach (var code in codes)
        {
            await _store.SaveCodeAsync(new TwoFactorCode
            {
                UserId = userId,
                Code = code,
                ExpiresAt = DateTime.MaxValue,
                Type = TwoFactorType.Authenticator,
                Purpose = TwoFactorCodePurpose.Recovery,
                IsUsed = false
            });
        }

        return codes;
    }

    public async Task<bool> ValidateBackupCodeAsync(string userId, string code)
    {
        var storedCode = await _store.GetCodeAsync(userId, code, TwoFactorCodePurpose.Backup);
        if (storedCode == null || storedCode.IsUsed)
            return false;

        await _store.MarkCodeAsUsedAsync(userId, code);
        return true;
    }

    public async Task<bool> ValidateRecoveryCodeAsync(string userId, string code)
    {
        var storedCode = await _store.GetCodeAsync(userId, code, TwoFactorCodePurpose.Recovery);
        if (storedCode == null || storedCode.IsUsed)
            return false;

        await _store.MarkCodeAsUsedAsync(userId, code);
        return true;
    }

    public async Task<bool> IsTwoFactorEnabledAsync(string userId)
    {
        var data = await _store.GetTwoFactorDataAsync(userId);
        return data?.IsEnabled == true;
    }

    public async Task<bool> IsLockedOutAsync(string userId)
    {
        var data = await _store.GetTwoFactorDataAsync(userId);
        if (data?.LockoutEnd == null)
            return false;

        if (DateTime.UtcNow < data.LockoutEnd)
            return true;

        // Reset lockout if expired
        await _store.UpdateLockoutAsync(userId, null, 0);
        return false;
    }

    public async Task ResetLockoutAsync(string userId)
    {
        await _store.UpdateLockoutAsync(userId, null, 0);
    }

    public async Task<string> GenerateEmailCodeAsync(string userId, string email)
    {
        var code = GenerateRandomNumericCode();
        await _store.SaveCodeAsync(new TwoFactorCode
        {
            UserId = userId,
            Code = code,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_options.EmailCodeExpirationMinutes),
            Type = TwoFactorType.Email,
            Purpose = TwoFactorCodePurpose.Email,
            IsUsed = false
        });

        // Email gönderme işlemi
        await _emailService.SendEmailAsync(
            to: email,
            subject: _options.EmailSubject,
            body: string.Format(_options.EmailTemplate, code)
        );

        return code;
    }

    public async Task<string> GenerateSmsCodeAsync(string userId, string phoneNumber)
    {
        var code = GenerateRandomNumericCode();
        await _store.SaveCodeAsync(new TwoFactorCode
        {
            UserId = userId,
            Code = code,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_options.SmsCodeExpirationMinutes),
            Type = TwoFactorType.Sms,
            Purpose = TwoFactorCodePurpose.Sms,
            IsUsed = false
        });

        // TODO: SMS gönderme işlemi
        // _smsService.SendAsync(phoneNumber, 
        //     string.Format(_options.SmsTemplate, _options.IssuerName, code));

        return code;
    }

    public async Task<bool> VerifyEmailCodeAsync(string userId, string code)
    {
        var storedCode = await _store.GetCodeAsync(userId, code, TwoFactorCodePurpose.Email);
        if (storedCode == null || storedCode.IsUsed)
            return false;

        if (DateTime.UtcNow > storedCode.ExpiresAt)
            return false;

        await _store.MarkCodeAsUsedAsync(userId, code);
        return true;
    }

    public async Task<bool> VerifySmsCodeAsync(string userId, string code)
    {
        var storedCode = await _store.GetCodeAsync(userId, code, TwoFactorCodePurpose.Sms);
        if (storedCode == null || storedCode.IsUsed)
            return false;

        if (DateTime.UtcNow > storedCode.ExpiresAt)
            return false;

        await _store.MarkCodeAsUsedAsync(userId, code);
        return true;
    }

    public async Task<bool> EnableEmailTwoFactorAsync(string userId)
    {
        var data = await _store.GetTwoFactorDataAsync(userId);
        if (data == null)
        {
            data = new TwoFactorData
            {
                UserId = userId,
                Type = TwoFactorType.Email,
                IsEnabled = true
            };
        }
        else
        {
            data.Type = TwoFactorType.Email;
            data.IsEnabled = true;
        }

        await _store.SaveTwoFactorDataAsync(data);
        return true;
    }

    public async Task<bool> EnableSmsTwoFactorAsync(string userId)
    {
        var data = await _store.GetTwoFactorDataAsync(userId);
        if (data == null)
        {
            data = new TwoFactorData
            {
                UserId = userId,
                Type = TwoFactorType.Sms,
                IsEnabled = true
            };
        }
        else
        {
            data.Type = TwoFactorType.Sms;
            data.IsEnabled = true;
        }

        await _store.SaveTwoFactorDataAsync(data);
        return true;
    }

    public async Task<bool> DisableEmailTwoFactorAsync(string userId)
    {
        await _store.DeleteAllDataAsync(userId);
        return true;
    }

    public async Task<bool> DisableSmsTwoFactorAsync(string userId)
    {
        await _store.DeleteAllDataAsync(userId);
        return true;
    }

    public async Task<TwoFactorType> GetEnabledTwoFactorTypeAsync(string userId)
    {
        var data = await _store.GetTwoFactorDataAsync(userId);
        return data?.IsEnabled == true ? data.Type : TwoFactorType.None;
    }

    private async Task IncrementFailedAttemptsAsync(string userId)
    {
        var data = await _store.GetTwoFactorDataAsync(userId);
        if (data == null) return;

        var failedAttempts = (data.FailedAttempts + 1);
        var lockoutEnd = failedAttempts >= _options.MaxFailedAttempts
            ? DateTime.UtcNow.AddSeconds(_options.LockoutDuration)
            : data.LockoutEnd;

        await _store.UpdateLockoutAsync(userId, lockoutEnd, failedAttempts);
    }

    private byte[] GenerateSecretKey()
    {
        var key = new byte[20]; // 160 bits
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(key);
        return key;
    }

    private string GenerateQrCodeUri(string email, string secret)
    {
        var issuer = Uri.EscapeDataString(_options.IssuerName);
        var account = Uri.EscapeDataString(email);
        return $"otpauth://totp/{issuer}:{account}?secret={secret}&issuer={issuer}&algorithm=SHA1&digits={_options.CodeLength}&period={_options.CodeValidityPeriod}";
    }

    private string GenerateRandomCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var code = new char[8];
        using var rng = RandomNumberGenerator.Create();

        for (int i = 0; i < code.Length; i++)
        {
            var randomByte = new byte[1];
            rng.GetBytes(randomByte);
            code[i] = chars[randomByte[0] % chars.Length];
        }

        return new string(code);
    }

    private string GenerateRandomNumericCode()
    {
        var code = new char[_options.CodeLength];
        using var rng = RandomNumberGenerator.Create();

        for (int i = 0; i < code.Length; i++)
        {
            var randomByte = new byte[1];
            rng.GetBytes(randomByte);
            code[i] = (char)('0' + (randomByte[0] % 10));
        }

        return new string(code);
    }
} 