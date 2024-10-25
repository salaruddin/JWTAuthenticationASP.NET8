using System.ComponentModel.DataAnnotations;

namespace JwtAuthentication_ASP.net8.Core.Dtos
{
    public class RegisterDto

    {
        [Required(ErrorMessage = "First Name is Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is Required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Age is Required")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Username is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }


    }
}
