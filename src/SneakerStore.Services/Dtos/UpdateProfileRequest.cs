using System.ComponentModel.DataAnnotations;

namespace SneakerStore.Services.Dtos
{
    public class UpdateProfileRequest
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [Phone]
        public string Phone { get; set; } = "";

        public IFormFile? ProfileImage { get; set; }
    }
}
