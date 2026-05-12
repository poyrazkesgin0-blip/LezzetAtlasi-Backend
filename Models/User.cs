namespace sipariş_uygulaması.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Şifre güvenliği için Hash ve Salt
        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt { get; set; } = new byte[0];

        // Roller: admin, restaurant_owner, customer
        public string Role { get; set; } = "customer";
    }
}