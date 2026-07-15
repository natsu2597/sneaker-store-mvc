using System.ComponentModel.DataAnnotations;

namespace SneakerStore.Services.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int SneakerId { get; set; }
        public string UserId { get; set; } = string.Empty;
        [Range(1,5)]
        public int Rating { get; set; }
        [StringLength(300)]
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
