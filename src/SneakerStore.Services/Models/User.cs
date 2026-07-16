using Microsoft.AspNetCore.Identity;
using SneakerStore.Services.Enums;
using System.ComponentModel.DataAnnotations;

namespace SneakerStore.Services.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Please provide a valid Email Address")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Enter a valid phone Number")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = "";
        public UserRole Role { get; set; }
        public string ProfileImage { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
