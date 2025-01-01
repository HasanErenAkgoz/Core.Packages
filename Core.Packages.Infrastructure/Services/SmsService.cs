using System.Text;
using System.Text.Json;
using Core.Packages.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.Packages.Infrastructure.Services;

public class SmsService : ISmsService
{
    private readonly SmsSettings _smsSettings;
    private readonly ILogger<SmsService> _logger;
    private readonly HttpClient _httpClient;

    public SmsService(
        IOptions<SmsSettings> smsSettings,
        ILogger<SmsService> logger,
        HttpClient httpClient)
    {
        _smsSettings = smsSettings.Value;
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task SendSmsAsync(string phoneNumber, string message)
    {
        try
        {
            // SMS provider'a göre implementasyon yapılacak
            var request = new HttpRequestMessage(HttpMethod.Post, _smsSettings.ApiUrl);
            var content = new
            {
                To = phoneNumber,
                Message = message,
                ApiKey = _smsSettings.ApiKey
            };

            request.Content = new StringContent(
                JsonSerializer.Serialize(content),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation($"SMS sent successfully to {phoneNumber}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending SMS to {phoneNumber}");
            throw;
        }
    }

    public async Task SendOtpAsync(string phoneNumber, string otp)
    {
        var message = $"Doğrulama kodunuz: {otp}. Bu kod 5 dakika geçerlidir.";
        await SendSmsAsync(phoneNumber, message);
    }

    public async Task SendVerificationCodeAsync(string phoneNumber, string code)
    {
        var message = $"Telefon numaranızı doğrulamak için kodunuz: {code}";
        await SendSmsAsync(phoneNumber, message);
    }
} 