using Microsoft.VisualBasic;
using SneakerStore.Services.Enums;
using System.ComponentModel.DataAnnotations;

namespace SneakerStore.Services.Models
{
    public class Sneaker
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int Stock { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public Gender Gender { get; set; } 
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
