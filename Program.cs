using Microsoft.EntityFrameworkCore;
using sipariş_uygulaması.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace sipariş_uygulaması
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. CORS: Tarayıcı engelini tamamen kaldırıyoruz
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // 2. Veritabanı
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();

            // 3. Güvenlik (Token): Hata olmaması için garantili anahtar kontrolü
            var tokenKey = builder.Configuration.GetSection("AppSettings:Token").Value ?? "bu_benim_cok_gizli_sifrem_1234567890";

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            // 4. SIRALAMA ÇOK KRİTİK: Önce Cors, Sonra Güvenlik!
            app.UseCors("AllowAll");

            // Yerel testlerde Https hatası almamak için bunu kapattık:
            // app.UseHttpsRedirection(); 

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}