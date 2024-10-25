using System.ComponentModel.DataAnnotations;

namespace JwtAuthentication_ASP.net8.Core.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage ="Username is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Password is Required")]
        public string Password {  get; set; }

    }
}
