using System.ComponentModel.DataAnnotations;

namespace SneakerStore.Services.Dtos
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; } = "";
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = "";
    }
}
