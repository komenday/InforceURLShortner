using Microsoft.EntityFrameworkCore;

namespace InforceURLShortner.Models
{
    public class InforceShortnerContext : DbContext
    {
        public DbSet<User>? Users { get; set; }
        public DbSet<Role>? Roles { get; set; }
        public DbSet<Url>? Urls { get; set; }
        public InforceShortnerContext(DbContextOptions<InforceShortnerContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminLogin = "admin";
            string adminPassword = "admin";

            Role adminRole = new Role { Id = 1, Name = "admin" };
            Role userRole = new Role { Id = 2, Name = "user" };
            User adminUser = new User { 
                Id = 1,
                Login = adminLogin, 
                Password = adminPassword,
                RoleId = adminRole.Id 
            };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            base.OnModelCreating(modelBuilder);
        }
    }
}
