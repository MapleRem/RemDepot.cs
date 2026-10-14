using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopSystem.Api.Data;
using ShopSystem.Api.Models;

namespace ShopSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/cart/user/3
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<Cart>> GetCart(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return NotFound("No cart found for this user.");
            return cart;
        }

        // POST: api/cart/add   (Add to cart)
        [HttpPost("add")]
        public async Task<ActionResult<Cart>> AddToCart(AddToCartRequest request)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == request.UserId);

            if (cart == null) return NotFound("No cart found for this user.");

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                });
            }

            await _context.SaveChangesAsync();
            return Ok(cart);
        }

        // PUT: api/cart/item/5
        [HttpPut("item/{itemId}")]
        public async Task<IActionResult> UpdateQuantity(int itemId, [FromBody] int quantity)
        {
            var item = await _context.CartItems.FindAsync(itemId);
            if (item == null) return NotFound();

            item.Quantity = quantity;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/cart/item/5
        [HttpDelete("item/{itemId}")]
        public async Task<IActionResult> RemoveItem(int itemId)
        {
            var item = await _context.CartItems.FindAsync(itemId);
            if (item == null) return NotFound();

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class AddToCartRequest
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
