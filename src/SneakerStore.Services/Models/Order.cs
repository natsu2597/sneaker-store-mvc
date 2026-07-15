namespace SneakerStore.Services.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public string PaymentMethod { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = "Pending";
        public DateTime OrderDate { get; set; } = DateTime.Now;
    }
}
