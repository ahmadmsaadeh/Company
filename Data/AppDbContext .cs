using Company.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne<Permission>()
                .WithMany()
                .HasForeignKey(u => u.PermissionID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
