public interface ISmsService
{
    Task SendSmsAsync(string phoneNumber, string message);
    Task SendOtpAsync(string phoneNumber, string otp);
    Task SendVerificationCodeAsync(string phoneNumber, string code);
} 