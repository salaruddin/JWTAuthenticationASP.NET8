using Microsoft.AspNetCore.Identity;

namespace JwtAuthentication_ASP.net8.Core.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
