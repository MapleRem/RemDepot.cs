using System.ComponentModel.DataAnnotations.Schema;

namespace ShopSystem.Api.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }

        // Snapshot of the price at purchase time, so later price changes
        // don't rewrite history.
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }
    }
}
