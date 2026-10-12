using Microsoft.AspNetCore.Mvc;
using ShopSystem.Api.Data;
using ShopSystem.Api.Models;

namespace ShopSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaymentsController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/payments/pay   (Payment step of the sequence)
        [HttpPost("pay")]
        public async Task<ActionResult<Payment>> Pay(PayRequest request)
        {
            var order = await _context.Orders.FindAsync(request.OrderId);
            if (order == null) return NotFound("Order not found.");

            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = order.TotalAmount,
                Method = request.Method,
                Status = PaymentStatus.Completed, // simulated instant success — swap in a real gateway later
                PaidAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            order.Status = OrderStatus.Paid;

            // Auto-create the shipment record so fulfillment can begin
            _context.Shipments.Add(new Shipment
            {
                OrderId = order.Id,
                TrackingNumber = $"TRK-{Guid.NewGuid().ToString()[..8].ToUpper()}",
                Status = ShipmentStatus.Preparing
            });

            await _context.SaveChangesAsync();

            return Ok(payment);
        }
    }

    public class PayRequest
    {
        public int OrderId { get; set; }
        public PaymentMethod Method { get; set; }
    }
}
