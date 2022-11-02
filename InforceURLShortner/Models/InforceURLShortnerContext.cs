using Microsoft.EntityFrameworkCore;

namespace InforceURLShortner.Models
{
    public class InforceShortnerContext : DbContext
    {
        public DbSet<User>? Users { get; set; }
        public InforceShortnerContext(DbContextOptions<InforceShortnerContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
