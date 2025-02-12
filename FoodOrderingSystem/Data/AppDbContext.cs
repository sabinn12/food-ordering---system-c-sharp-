using Microsoft.EntityFrameworkCore;
using FoodOrderingSystem.Models;

namespace FoodOrderingSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Add DbSet properties for your entities
        public DbSet<User> Users { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}