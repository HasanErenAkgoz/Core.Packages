using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
}