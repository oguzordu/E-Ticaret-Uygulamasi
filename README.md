# E-Commerce Platform

[![CI](https://github.com/oguzordu/ecommerce-platform/actions/workflows/ci.yml/badge.svg)](https://github.com/oguzordu/ecommerce-platform/actions/workflows/ci.yml)

## Türkçe

Temel e-ticaret işlevlerini içeren bir sistem: kullanıcılar ürünleri
görüntüleyebiliyor, sepete ekleyip sipariş verebiliyor. Yöneticiler için bir
admin paneli de var.

### Kullanılan Teknolojiler

Ders kapsamında API'yi sıfırdan geliştirmek gerektiği için backend tarafında
**ASP.NET Core Web API** kullanıldı. Veritabanı işlemleri **Entity Framework
Core** ile yapıldı, kullanıcı girişinde **JWT token** yapısı kuruldu. API
testleri için **Swagger**'dan yararlanıldı.

Arayüz kısmı için **ASP.NET Core MVC** ve **Bootstrap** kullanıldı.

### Mimari

**Backend**

- Modeller: Users, Products, Categories, CartItems, Orders, OrderItems
- `DbContext` içinde ilişkiler ve tablolar tanımlandı
- Controller'lar: Auth, Products, Cart, Orders, Categories
- Tüm iş mantığı, interface yapısı kullanılarak service katmanına konuldu
- JWT token ile kimlik doğrulama
- Frontend'in API'ye erişebilmesi için CORS ayarlandı

**Frontend**

- MVC ile ürün listeleme, arama ve kategori filtreleme
- Ürün detay, sepet ve sipariş sayfaları
- Admin paneli ile ürün ve sipariş yönetimi
- API'den veri çekmek için ayrı bir service sınıfı

**Veritabanı**

Altı tablo: Users, Categories, Products, CartItems, Orders, OrderItems.
Kategori–ürün, kullanıcı–sepet ve sipariş–sipariş öğeleri ilişkileri foreign
key olarak tanımlandı.

### Kurulum ve Çalıştırma

**Gerekenler**

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server LocalDB — Visual Studio ile birlikte gelir. Ayrı kurmak isteyen:
  [SQL Server Express LocalDB](https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb)
- EF Core komut satırı aracı:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

**1. Repoyu klonla**

```bash
git clone https://github.com/oguzordu/ecommerce-platform.git
cd ecommerce-platform
```

**2. HTTPS sertifikasına güven** (frontend API'ye HTTPS üzerinden bağlanıyor)

```bash
dotnet dev-certs https --trust
```

**3. Veritabanını oluştur**

Migration'lar repoda hazır, `script.sql` dosyasına ihtiyaç yok:

```bash
cd backend/ECommerce.API
dotnet ef database update
```

Bu, `appsettings.json` içindeki bağlantı dizesini kullanarak
`(localdb)\mssqllocaldb` üzerinde `ECommerceDB` veritabanını kurar.
Farklı bir SQL Server kullanacaksan `appsettings.json` içindeki
`ConnectionStrings:DefaultConnection` değerini değiştir.

**4. API'yi başlat**

```bash
cd backend/ECommerce.API
dotnet run --launch-profile https
```

API `https://localhost:7125` adresinde açılır. Swagger arayüzü:
`https://localhost:7125/swagger`

⚠️ **`https` profili şart.** Frontend, API adresini
`frontend/ECommerce.Web/Services/ApiService.cs` içinde sabit olarak
`https://localhost:7125/api` şeklinde tutuyor. Farklı bir portta çalıştırırsan
o dosyayı da güncellemen gerekir.

**5. Frontend'i başlat** (API çalışır durumdayken, ikinci bir terminalde)

```bash
cd frontend/ECommerce.Web
dotnet run
```

Siteyi `https://localhost:7196` adresinde aç.

**Sıra önemli:** önce API, sonra frontend. API kapalıyken site açılır ama
ürünler yüklenmez.

### Bilinen Sınırlamalar

- `appsettings.json` içindeki JWT anahtarı repoda açık duruyor. Ders projesi
  olduğu için böyle bırakıldı; gerçek bir dağıtımda ortam değişkeni veya
  kullanıcı gizli anahtarları (user secrets) kullanılmalı.
- `script.sql` dosyası içinde `C:\Users\Oguz\...` şeklinde sabit dosya yolları
  var. Migration'ları kullan, o dosyayı çalıştırma.


### Yapım Süreci

Önce backend geliştirildi — modeller, migration'lar ve API — ardından MVC
tarafı buna bağlandı.

En çok zorlanılan iki yer: CORS ayarları ve token'ın frontend'de nerede
saklanacağı (sonunda session'da saklanarak çözüldü).

### Sistemin Çalışma Şekli

Kullanıcı giriş yaptığında bir token alıyor ve bu token sonraki her istekte
kullanılıyor. Ürünleri listeleyip detaylara bakabiliyor, sepete ekleyip
sipariş verebiliyor. Adminler ayrı bir panelden ürün ve sipariş yönetimi
yapabiliyor.

### Gelecek Geliştirmeler

- Tek, uzun ömürlü JWT yerine refresh token
- Service katmanı için birim testleri
- Ürün listeleme uç noktasında sayfalama (pagination)

---

## English

A full-stack e-commerce system where users can browse products, add them to a
cart, and place orders. Includes an admin panel for managing products and
orders.

### Tech Stack

Built as a coursework project that required building an API from scratch:
**ASP.NET Core Web API** on the backend, **Entity Framework Core** for data
access, and **JWT** for authentication. Endpoints were tested with Swagger
throughout development.

The frontend uses **ASP.NET Core MVC** with **Bootstrap**.

### Architecture

**Backend**

- Domain models: Users, Products, Categories, CartItems, Orders, OrderItems
- Relationships and tables defined via `DbContext`
- Controllers: Auth, Products, Cart, Orders, Categories
- Business logic isolated in a service layer behind interfaces
- JWT-based authentication
- CORS configured so the frontend can reach the API

**Frontend**

- Product listing, search, and category filtering (MVC)
- Product detail, cart, and order pages
- Admin panel for product and order management
- A dedicated service class handles all API calls from the frontend

**Database**

Six tables: Users, Categories, Products, CartItems, Orders, OrderItems, with
foreign-key relationships (category–product, user–cart, order–order items).

### Setup and Running

**Prerequisites**

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server LocalDB — ships with Visual Studio, or install
  [SQL Server Express LocalDB](https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb)
  separately
- EF Core CLI tool:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

**1. Clone**

```bash
git clone https://github.com/oguzordu/ecommerce-platform.git
cd ecommerce-platform
```

**2. Trust the HTTPS development certificate** (the frontend calls the API over HTTPS)

```bash
dotnet dev-certs https --trust
```

**3. Create the database**

Migrations are committed, so `script.sql` is not needed:

```bash
cd backend/ECommerce.API
dotnet ef database update
```

This creates `ECommerceDB` on `(localdb)\mssqllocaldb` using the connection
string in `appsettings.json`. To use a different SQL Server instance, edit
`ConnectionStrings:DefaultConnection` there.

**4. Start the API**

```bash
cd backend/ECommerce.API
dotnet run --launch-profile https
```

The API listens on `https://localhost:7125`. Swagger UI:
`https://localhost:7125/swagger`

⚠️ **The `https` profile is required.** The frontend hard-codes the API address
as `https://localhost:7125/api` in
`frontend/ECommerce.Web/Services/ApiService.cs`. If you run the API on a
different port, update that file too.

**5. Start the frontend** (in a second terminal, with the API running)

```bash
cd frontend/ECommerce.Web
dotnet run
```

Open `https://localhost:7196`.

**Order matters:** API first, then the frontend. The site loads without the API
but no products will appear.

### Known Limitations

- The JWT secret in `appsettings.json` is committed to the repository. It was
  left this way because this is a coursework project; a real deployment should
  use environment variables or user secrets.
- `script.sql` contains hard-coded `C:\Users\Oguz\...` file paths. Use the
  migrations instead of running that file.


### How it was built

Backend first — models, migrations, and the API — then the MVC frontend wired
up to it.

The two things that took the most iteration: CORS configuration, and where to
store the JWT on the frontend (ended up using session storage).

### How it works

The user logs in and receives a token, which is attached to every subsequent
request. From there they can browse products, view details, add items to
the cart, and place orders. Admins get a separate panel for managing products
and orders.

### Future Improvements

- Refresh tokens instead of a single long-lived JWT
- Unit tests for the service layer
- Pagination on the product listing endpoint
