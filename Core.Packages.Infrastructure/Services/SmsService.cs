using Core.Packages.Application.CrossCuttingConcerns.Sms;
using Core.Packages.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Core.Packages.Infrastructure.Services.Sms;

public class SmsService : ISmsService
{
    private readonly SmsSettings _smsSettings;
    private readonly HttpClient _httpClient;

    public SmsService(IOptions<SmsSettings> smsSettings, HttpClient httpClient)
    {
        _smsSettings = smsSettings.Value;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_smsSettings.BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("ApiKey", _smsSettings.ApiKey);
        _httpClient.DefaultRequestHeaders.Add("ApiSecret", _smsSettings.ApiSecret);
    }

    public async Task SendSmsAsync(string phoneNumber, string message)
    {
        var request = new
        {
            PhoneNumber = phoneNumber,
            Message = message,
            Sender = _smsSettings.SenderName
        };

        var response = await _httpClient.PostAsJsonAsync("api/sms/send", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task SendBulkSmsAsync(List<string> phoneNumbers, string message)
    {
        var request = new
        {
            PhoneNumbers = phoneNumbers,
            Message = message,
            Sender = _smsSettings.SenderName
        };

        var response = await _httpClient.PostAsJsonAsync("api/sms/send-bulk", request);
        response.EnsureSuccessStatusCode();
    }
} 