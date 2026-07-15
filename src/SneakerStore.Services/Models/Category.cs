using System.ComponentModel.DataAnnotations;

namespace SneakerStore.Services.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }

    }
}
