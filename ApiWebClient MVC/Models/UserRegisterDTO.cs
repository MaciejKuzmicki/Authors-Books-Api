using System.ComponentModel.DataAnnotations;

namespace ApiWebClient.Models
{
    public class UserRegisterDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "The passwords do net match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required, MinLength(3)]
        public string Username { get; set; } = string.Empty;
    }
}
