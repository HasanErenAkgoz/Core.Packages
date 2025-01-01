using Core.Packages.Domain.Identity;

namespace Core.Packages.Infrastructure.Extensions;

public static class RefreshTokenExtensions
{
    public static RefreshToken ToEntity(this RefreshToken refreshToken)
    {
        return new RefreshToken
        {
            Token = refreshToken.Token,
            Expires = refreshToken.Expires,
            UserId = refreshToken.UserId,
            Created = refreshToken.Created,
            CreatedByIp = refreshToken.CreatedByIp,
            Revoked = refreshToken.Revoked,
            
        };
    }
    
    public static RefreshToken ToDto(this RefreshToken refreshToken)
    {
        return new RefreshToken
        {
            Token = refreshToken.Token,
            Expires = refreshToken.Expires,
            CreatedByIp = refreshToken.CreatedByIp
        };
    }
}