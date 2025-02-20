namespace Core.Packages.Application.Features.Auth.Login.Commands
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}