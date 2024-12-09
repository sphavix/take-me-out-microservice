using Dinner.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Dinner.Api.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<ApplicationUser> Users { get; set; }
    }
}
