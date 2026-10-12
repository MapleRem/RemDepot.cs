using System.ComponentModel.DataAnnotations.Schema;

namespace ShopSystem.Api.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        public string ShippingAddress { get; set; } = string.Empty;

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        public Payment? Payment { get; set; }
        public Shipment? Shipment { get; set; }
    }
}
