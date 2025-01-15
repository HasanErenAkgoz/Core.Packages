namespace Core.Packages.Security.TwoFactorAuth.Models;

public class TwoFactorAuthOptions
{
    public int CodeLength { get; set; } = 6;
    public int CodeValidityPeriod { get; set; } = 30; // seconds
    public string IssuerName { get; set; } = "YourApp";
    public int BackupCodesCount { get; set; } = 10;
    public int RecoveryCodesCount { get; set; } = 8;
    public int MaxFailedAttempts { get; set; } = 3;
    public int LockoutDuration { get; set; } = 300; // seconds

    public string EmailSubject { get; set; } = "Güvenlik Kodu";
    public string EmailTemplate { get; set; } = "Güvenlik kodunuz: {0}";
    public int EmailCodeExpirationMinutes { get; set; } = 5;

    public string SmsTemplate { get; set; } = "{0} kodunu kimseyle paylaşmayın. İki faktörlü kimlik doğrulama kodunuz: {1}";
    public int SmsCodeExpirationMinutes { get; set; } = 5;
} 