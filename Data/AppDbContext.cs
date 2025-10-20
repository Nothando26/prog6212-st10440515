using Microsoft.EntityFrameworkCore;
using prog6212_st10440515_poe.Models;

namespace prog6212_st10440515_poe.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Claim> Claims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User -> Lecturer (one-to-one)
            modelBuilder.Entity<Lecturer>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(l => l.UserID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Lecturer -> Claim (one-to-many)
            modelBuilder.Entity<Claim>()
                .HasOne(c => c.Lecturer)
                .WithMany(l => l.Claims)
                .HasForeignKey(c => c.LecturerID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
