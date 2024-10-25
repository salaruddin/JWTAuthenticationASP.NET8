using System.ComponentModel.DataAnnotations;

namespace JwtAuthentication_ASP.net8.Core.Dtos
{
    public class UpdatePermissionDto
    {
        [Required(ErrorMessage ="UserName is required")]
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}
