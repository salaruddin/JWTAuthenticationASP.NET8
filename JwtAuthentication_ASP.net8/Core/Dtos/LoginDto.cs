using System.ComponentModel.DataAnnotations;

namespace JwtAuthentication_ASP.net8.Core.Dtos
{
    public record LoginDto
    {
        [Required(ErrorMessage ="Username is Required")]
        string UserName;

        [Required(ErrorMessage ="Password is Required")]
        string Password;

    }
}
