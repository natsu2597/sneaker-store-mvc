using System.ComponentModel.DataAnnotations;

namespace SneakerStore.Services.Models
{
    public class Brand
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }

    }
}
