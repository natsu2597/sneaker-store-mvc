namespace SneakerStore.Services.Enums
{
    public enum OrderStatus
    {
        None = 0,

        Pending = 1,
        Confirmed,
        Packed,
        Shipped,
        OutForDelivery,
        Delivered,
        Cancelled
    }
}
