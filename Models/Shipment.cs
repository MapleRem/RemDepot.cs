namespace ShopSystem.Api.Models
{
    public class Shipment
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int? CourierId { get; set; }
        public User? Courier { get; set; }

        public string TrackingNumber { get; set; } = string.Empty;

        public ShipmentStatus Status { get; set; } = ShipmentStatus.Preparing;

        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }
}
