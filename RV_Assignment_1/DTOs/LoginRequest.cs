using System.ComponentModel.DataAnnotations;

namespace RV_Assignment_1.DTOs
{
    public class LoginRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
