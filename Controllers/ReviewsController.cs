using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopSystem.Api.Data;
using ShopSystem.Api.Models;

namespace ShopSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/reviews/product/5
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetByProduct(int productId)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId)
                .Include(r => r.User)
                .ToListAsync();
        }

        // POST: api/reviews   (Review product)
        [HttpPost]
        public async Task<ActionResult<Review>> CreateReview(Review review)
        {
            if (review.Rating < 1 || review.Rating > 5)
                return BadRequest("Rating must be between 1 and 5.");

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByProduct), new { productId = review.ProductId }, review);
        }
    }
}
