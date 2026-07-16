namespace SneakerStore.Services.Models
{
    public class SneakerImages
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int SneakerId { get; set; }
        public string PublicId { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
    }
}
