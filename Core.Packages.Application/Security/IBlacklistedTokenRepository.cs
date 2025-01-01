namespace Core.Packages.Application.Security;

public interface IBlacklistedTokenRepository 
{
    Task<bool> IsTokenBlacklistedAsync(string token);
    Task BlacklistTokenAsync(string token, DateTime expiryDate);
}