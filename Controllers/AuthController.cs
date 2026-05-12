using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography; // Şifreleme (Hash) işlemleri için eklendi
using sipariş_uygulaması.Models;
using System.Linq;

namespace sipariş_uygulaması.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AuthController(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        // --- ŞİFRELEME (HASH) METOTLARI ---
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        // --- GERÇEK KAYIT OLMA ---
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserLoginDto dto)
        {
            if (_context.Users.Any(u => u.Username.ToLower() == dto.Username.ToLower()))
            {
                return BadRequest("Bu kullanıcı adı zaten kullanılıyor.");
            }

            CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "customer"
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok(new { message = "Kayıt başarıyla oluşturuldu!" });
        }

        // --- GERÇEK GİRİŞ YAPMA ---
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto loginDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username.ToLower() == loginDto.Username.ToLower());

            // Süper Admin Kontrolü (Veritabanında olmasa bile poyrazadmin girebilir)
            if (user == null && loginDto.Username == "poyrazadmin" && loginDto.Password == "Sifre12345")
            {
                return GenerateToken("poyrazadmin", "admin");
            }

            // Normal Kullanıcı Kontrolü
            if (user == null || !VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Unauthorized("Kullanıcı adı veya şifre hatalı.");
            }

            return GenerateToken(user.Username, user.Role);
        }

        // --- TOKEN ÜRETİCİ ---
        private IActionResult GenerateToken(string username, string role)
        {
            var tokenKey = _configuration.GetSection("AppSettings:Token").Value ?? "bu_benim_cok_gizli_sifrem_1234567890";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new { token = tokenHandler.WriteToken(token) });
        }
    }

    public class UserLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}