using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;
namespace BookingSystem.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Schedule> Schedules { get; set; }

    // Add other DbSet for related entities
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define relationships and configurations here

        // Example configuration for PurchasedPackage
        modelBuilder.Entity<PurchasedPackage>()
            .HasKey(pp => pp.Id);

        modelBuilder.Entity<PurchasedPackage>()
            .HasOne(pp => pp.User)
            .WithMany(u => u.PurchasedPackages)
            .HasForeignKey(pp => pp.UserId);

        modelBuilder.Entity<PurchasedPackage>()
            .HasOne(pp => pp.Package)
            .WithMany(p => p.PurchasedPackages)
            .HasForeignKey(pp => pp.PackageId);

        modelBuilder.Entity<Waitlist>()
           .HasKey(w => w.UserId);

        // Add other configurations for your model relationships

        base.OnModelCreating(modelBuilder);
    }
}

