using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopSystem.Api.Data;
using ShopSystem.Api.Models;

namespace ShopSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return user;
        }

        // POST: api/users/register
        [HttpPost("register")]
public async Task<IActionResult> Register([FromBody] User user)
{
    // Restrict public registration to Customer, Seller, and Courier only
    var allowedRoles = new[] { "Customer", "Seller", "Courier" };
    
    if (string.IsNullOrEmpty(user.Role) || !allowedRoles.Contains(user.Role))
    {
        return BadRequest(new { message = "Invalid role. Allowed roles are: Customer, Seller, Courier." });
    }

    // Hash password and save user to database...
    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
    
    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return Ok(user);
}

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return Unauthorized("Invalid email or password.");

            // NOTE: this just confirms credentials and returns the user.
            // Add JWT issuing here once you need real session/auth handling.
            return user;
        }

        // PUT: api/users/5
        // PUT: api/users/5
[HttpPut("{id}")]
public async Task<IActionResult> UpdateUser(int id, User updated)
{
    // Automatically assign the ID from the URL path to the model
    updated.Id = id;

    _context.Entry(updated).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!_context.Users.Any(e => e.Id == id))
        {
            return NotFound();
        }
        throw;
    }

    return NoContent();
}

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class RegisterRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Customer;
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
