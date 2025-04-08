namespace Dinner.Api.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
