using System;
using System.Text.Json.Serialization; // Bunu ekledik

namespace sipariş_uygulaması.Models
{
    public class Order
    {
        [JsonPropertyName("id")] // JSON'daki "id" ismini buna zorla
        public int? Id { get; set; }

        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Hazırlanıyor";
        public DateTime OrderDate { get; set; } = DateTime.Now;
    }
}