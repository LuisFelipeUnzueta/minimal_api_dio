using Microsoft.EntityFrameworkCore;
using Minimal.Api.Domain.Entity;
using Minimal.Api.Domain.Enuns;

namespace Minimal.Api.Infra.Db
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        public DbSet<Admin> Admins { get; set; } = default!;

        public DbSet<Vehicle> Vehicles { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .Property(a => a.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Admin>().HasData(
                new Admin { Id = 1, Email = "admin@example.com", Password = "admin123", Role = RoleType.Admin }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection")?.ToString();
            if (!string.IsNullOrEmpty(connectionString))
            {
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
           
        }
    }

}
