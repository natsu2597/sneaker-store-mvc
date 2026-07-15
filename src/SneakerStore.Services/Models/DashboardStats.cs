namespace SneakerStore.Services.Models
{
    public class DashboardStats
    {
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public int TotalUsers { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PendingOrders { get; set; }
        public int DeliveredOrders { get; set; }
    }
}
