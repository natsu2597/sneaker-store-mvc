using System.ComponentModel.DataAnnotations;

namespace SneakerStore.Services.Dtos
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;
    }
}
