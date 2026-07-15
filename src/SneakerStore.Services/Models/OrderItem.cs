namespace SneakerStore.Services.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public int SneakerId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
