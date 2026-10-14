using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopSystem.Api.Data;
using ShopSystem.Api.Models;

namespace ShopSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/orders/checkout   (Checkout: turns the cart into an Order)
        [HttpPost("checkout")]
        public async Task<ActionResult<Order>> Checkout(CheckoutRequest request)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == request.UserId);

            if (cart == null || !cart.Items.Any())
                return BadRequest("Cart is empty.");

            var order = new Order
            {
                UserId = request.UserId,
                ShippingAddress = request.ShippingAddress,
                Status = OrderStatus.Pending
            };

            foreach (var item in cart.Items)
            {
                if (item.Product == null) continue;

                order.Items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                });
            }

            order.TotalAmount = order.Items.Sum(i => i.Quantity * i.UnitPrice);

            _context.Orders.Add(order);

            // Empty the cart once its contents become an order
            _context.CartItems.RemoveRange(cart.Items);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        // GET: api/orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .Include(o => o.Payment)
                .Include(o => o.Shipment)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();
            return order;
        }

        // GET: api/orders/user/3   (Track order — a user's order history + status)
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUser(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                .Include(o => o.Shipment)
                .ToListAsync();
        }

        // PUT: api/orders/5/status   (Admin/Seller updates fulfillment status)
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            order.Status = status;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class CheckoutRequest
    {
        public int UserId { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
    }
}
