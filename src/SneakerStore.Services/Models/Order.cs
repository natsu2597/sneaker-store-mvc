using SneakerStore.Services.Enums;

namespace SneakerStore.Services.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }
}
