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
