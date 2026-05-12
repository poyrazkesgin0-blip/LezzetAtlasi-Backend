using Microsoft.EntityFrameworkCore;

namespace sipariş_uygulaması.Models
{
    // DbContext'ten miras alıyoruz ki Entity Framework özelliklerini kullanabilelim
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Veritabanında oluşacak tablolarımızın isimleri (Çoğul ekliyoruz)
        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
