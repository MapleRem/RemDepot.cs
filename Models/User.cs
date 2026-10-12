using System.ComponentModel.DataAnnotations;

namespace ShopSystem.Api.Models
{
    // Represents Customer, Seller, Admin, or Courier — distinguished by Role.
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required, Phone, MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.Customer;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<Product>? Products { get; set; }   // if Role == Seller
        public Cart? Cart { get; set; }                        // if Role == Customer
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
