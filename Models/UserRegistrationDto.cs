using System.ComponentModel.DataAnnotations;

namespace ShopSystem.Api.Models
{
    public class UserRegistrationDto
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required, Phone, MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public int Role { get; set; } = 0; // 0 for Customer, etc., depending on your UserRole enum
    }
}