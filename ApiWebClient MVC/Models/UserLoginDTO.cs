using System.ComponentModel.DataAnnotations;

namespace ApiWebClient.Models
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "E-mail is required")]
        [EmailAddress(ErrorMessage ="Invalid e-mail address")]
        [StringLength(50, MinimumLength = 3)]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }
    }
}
