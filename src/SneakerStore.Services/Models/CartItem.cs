namespace SneakerStore.Services.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public int SneakerId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
