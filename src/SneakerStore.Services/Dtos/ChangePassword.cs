using System.ComponentModel.DataAnnotations;

namespace SneakerStore.Services.Dtos
{
    public class ChangePassword
    {
        public int Id { get; set; }
        [Required]
        public string CurrentPassword { get; set; } = "";

        [Required]
        public string NewPassword { get; set; } = "";

        [Compare(nameof(NewPassword), ErrorMessage = "Passwords don't match!.")]
        public string ConfirmPassword { get; set; } = "";
    }
}
