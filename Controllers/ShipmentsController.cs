using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopSystem.Api.Data;
using ShopSystem.Api.Models;

namespace ShopSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShipmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ShipmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/shipments/order/5   (Track order — delivery leg)
        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<Shipment>> GetByOrder(int orderId)
        {
            var shipment = await _context.Shipments
                .Include(s => s.Courier)
                .FirstOrDefaultAsync(s => s.OrderId == orderId);

            if (shipment == null) return NotFound();
            return shipment;
        }

        // PUT: api/shipments/5/assign-courier
        [HttpPut("{id}/assign-courier")]
        public async Task<IActionResult> AssignCourier(int id, [FromBody] int courierId)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null) return NotFound();

            shipment.CourierId = courierId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/shipments/5/status   (Courier updates delivery progress)
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] ShipmentStatus status)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null) return NotFound();

            shipment.Status = status;

            if (status == ShipmentStatus.InTransit)
            {
                shipment.ShippedAt = DateTime.UtcNow;
            }
            else if (status == ShipmentStatus.Delivered)
            {
                shipment.DeliveredAt = DateTime.UtcNow;

                var order = await _context.Orders.FindAsync(shipment.OrderId);
                if (order != null) order.Status = OrderStatus.Delivered;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
