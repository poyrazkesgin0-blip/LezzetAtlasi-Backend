namespace sipariş_uygulaması.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Rating { get; set; }

        // Restoran sahibinin User tablosundaki ID'si
        public int OwnerId { get; set; }
    }
}