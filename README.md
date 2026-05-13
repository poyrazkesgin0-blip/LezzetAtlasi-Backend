# ⚙️ Lezzet Atlası  | Backend (ASP.NET Core Web API)

Bu depo, **Lezzet Atlası** online sipariş ve restoran yönetim platformunun sunucu tarafını (Backend) barındırmaktadır. Modern RESTful mimari standartlarına uygun olarak **C#** ve **ASP.NET Core Web API** kullanılarak geliştirilmiştir.

Platformun güvenli oturum yönetimi, dinamik fiyatlandırma algoritmaları, çapraz satış rotaları ve veritabanı işlemleri tamamen bu servis üzerinden yönetilmektedir.

---

## 🏗️ Mimari & Temel Özellikler

### 🔒 Güvenlik & Kimlik Doğrulama (JWT Auth)
- **JSON Web Tokens (JWT):** İstemci tarafında durumsuz (stateless) ve güvenli oturum yönetimi.
- **Rol Bazlı Yetkilendirme (Role-Based Auth):** Müşteri ve Restoran Yöneticisi (`admin`) ayrımı.
  - `[AllowAnonymous]` ile herkese açık dükkan/menü listeleme uç noktaları.
  - `[Authorize]` ile korunan sipariş tamamlama ve profil yönetimi rotaları.

### 🗄️ Veritabanı & ORM (Entity Framework Core)
- **Code-First Yaklaşımı:** C# model sınıfları üzerinden otomatik veritabanı şeması ve tablo oluşturma.
- **İlişkisel Veri Tasarımı:** Restoranlar, Ürünler ve Siparişler arası veri bütünlüğü.
- **Veritabanı Esnekliği:** Geliştirme ortamında **SQLite** veya **SQL Server** ile tam uyumluluk.

### 🚀 API Uç Noktaları (Endpoints) & CORS Mimarisi
- **CORS Politikaları:** Vanilla JS Frontend uygulamasının API'yi sorunsuz tüketebilmesi için yapılandırılmış erişim izinleri.
- **Model Binding Esnekliği:** İstemciden gelen hem `camelCase` hem de `PascalCase` verileri çözümleyen esnek yapı.

---

## 📡 Temel API Rotaları (Endpoints)

| HTTP Metodu | Uç Nokta (Endpoint) | Açıklama | Yetki |
| :--- | :--- | :--- | :--- |
| **POST** | `/api/Auth/register` | Yeni kullanıcı kaydı açar. | Herkese Açık |
| **POST** | `/api/Auth/login` | Giriş yapar ve JWT Token döner. | Herkese Açık |
| **GET** | `/api/Restaurants` | Tüm restoranları listeler. | Herkese Açık |
| **GET** | `/api/Products` | Menüleri ve ürünleri getirir. | Herkese Açık |
| **POST** | `/api/Orders` | Yeni sipariş oluşturur. | Token Gerekli |
| **PUT** | `/api/Orders/{id}` | Sipariş durumunu günceller. | Sadece Admin |

---

## 🛠️ Kullanılan Teknolojiler & Paketler

- **Çekirdek:** .NET 6.0 / 8.0, C# 10+
- **Web API:** ASP.NET Core REST API
- **Veri Erişim (ORM):** Microsoft.EntityFrameworkCore, SQLite/SQL Server
- **Kimlik Doğrulama:** Microsoft.AspNetCore.Authentication.JwtBearer
- **Dökümantasyon:** Swashbuckle.AspNetCore (Swagger UI)

---

## 🚀 Kurulum & Yerel Ortamda Çalıştırma

### 1. Projeyi Klonlama
```bash
git clone https://github.com/poyrazkesgin0-blip/LezzetAtlasi-Backend.git
cd LezzetAtlasi-Backend
