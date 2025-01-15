using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net;

namespace Core.Packages.Infrastructure.Middleware.IPSecurity;

public class IPSecurityMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IPSecurityOptions _options;

    public IPSecurityMiddleware(RequestDelegate next, IOptions<IPSecurityOptions> options)
    {
        _next = next;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!_options.EnableIPBlocking)
        {
            await _next(context);
            return;
        }

        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        if (string.IsNullOrEmpty(ipAddress))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return;
        }

        if (_options.EnableWhitelist)
        {
            if (!IsIPWhitelisted(ipAddress))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
        }
        else if (IsIPBlacklisted(ipAddress))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return;
        }

        await _next(context);
    }

    private bool IsIPWhitelisted(string ipAddress)
    {
        return _options.WhitelistedIPs.Contains(ipAddress) ||
               _options.WhitelistedIPRanges.Any(range => IsInRange(ipAddress, range));
    }

    private bool IsIPBlacklisted(string ipAddress)
    {
        return _options.BlacklistedIPs.Contains(ipAddress) ||
               _options.BlacklistedIPRanges.Any(range => IsInRange(ipAddress, range));
    }

    private bool IsInRange(string ipAddress, string range)
    {
        var parts = range.Split('/');
        if (parts.Length != 2) return false;

        var baseIP = IPAddress.Parse(parts[0]);
        var prefixLength = int.Parse(parts[1]);

        var ipBytes = IPAddress.Parse(ipAddress).GetAddressBytes();
        var rangeBytes = baseIP.GetAddressBytes();

        var numBytes = prefixLength / 8;
        var remainingBits = prefixLength % 8;

        for (var i = 0; i < numBytes && i < ipBytes.Length; i++)
        {
            if (ipBytes[i] != rangeBytes[i]) return false;
        }

        if (remainingBits > 0 && numBytes < ipBytes.Length)
        {
            var mask = (byte)(0xFF << (8 - remainingBits));
            if ((ipBytes[numBytes] & mask) != (rangeBytes[numBytes] & mask))
                return false;
        }

        return true;
    }
} 