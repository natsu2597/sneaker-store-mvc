using System.ComponentModel.DataAnnotations;

namespace SneakerStore.Services.Dtos
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; } = "";
        [Required]
        public string LastName { get; set; } = "";
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; } = "";
        [Required]
        [MinLength(8,ErrorMessage = "Password should be minimum 8 characters")]
        public string Password { get; set; } = "";
        [Required]
        [Compare(nameof(Password),ErrorMessage = "Passwords don't match!.")]
        public string ConfirmPassword { get; set; } = "";
        [Required]
        [Phone(ErrorMessage = "Enter a valid mobile number")]
        public string Phone { get; set; } = "";
        public IFormFile? ProfileImage { get; set; }
    }
}
