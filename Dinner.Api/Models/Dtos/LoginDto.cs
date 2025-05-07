using System.ComponentModel.DataAnnotations;

namespace Dinner.Api.Models.Dtos
{
    public class LoginDto
    {
        [Required]
        [MaxLength(100)]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
