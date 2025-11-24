# E-Ticaret Projesi

Temel e-ticaret işlevlerini içeren bir sistem geliştirdim. Kullanıcılar ürünleri görüntüleyebiliyor, sepete ekleyip sipariş verebiliyor. Ayrıca yöneticiler için bir admin paneli ekledim.

## Kullandığım Teknolojiler

Ders kapsamında API geliştirmek gerektiği için backend tarafında **ASP.NET Core Web API** kullandım. Veritabanı işlemlerini **Entity Framework Core** ile yaptım ve kullanıcı girişinde **JWT token** yapısını kurdum. API testleri için **Swagger**'dan yararlandım.

Arayüz kısmını oluşturmam gerektiği için de **ASP.NET Core MVC** ve **Bootstrap** kullandım.

## Projenin Yapısı

### Backend

- Modelleri oluşturdum: Users, Products, Categories, CartItems, Orders, OrderItems.
- DbContext içerisinde ilişkileri ve tabloları tanımladım.
- Controller'ları yazdım (Auth, Products, Cart, Orders, Categories).
- Tüm iş mantığını service katmanına koydum ve interface yapısı kullandım.
- JWT token ile kimlik doğrulama ekledim.
- Frontend'in API'ye erişebilmesi için CORS ayarlarını yaptım.

### Frontend

- MVC ile ürün listeleme, arama ve kategori filtreleme özelliklerini yaptım.
- Ürün detay, sepet ve sipariş sayfalarını oluşturdum.
- Admin paneli ekleyerek ürün ve sipariş yönetimi ekledim.
- API'den veri çekmek için frontend tarafında bir service sınıfı yazdım.

## Veritabanı Yapısı

Users, Categories, Products, CartItems, Orders ve OrderItems olmak üzere altı tablo kullandım.

Kategori–ürün, kullanıcı–sepet ve sipariş–sipariş öğeleri gibi ilişkileri foreign key olarak tanımladım.

## Yapım Süreci

Önce backend'i geliştirdim, migration işlemleri ve API yapısını kurdum. Ardından MVC tarafını oluşturdum ve API ile haberleştirdim.

CORS ayarları ve frontend'de token saklama kısmında zorlandım; token'ı session'da saklayarak çözdüm.

## Sistemin Çalışma Şekli

Kullanıcı giriş yaptığında token alıyor ve bütün isteklerde bunu kullanıyor.

Ürünleri listeleyip detaylara bakabiliyor, sepete ekleyip sipariş verebiliyor.

Admin panelinde yöneticiler ürün ve sipariş yönetimi yapabiliyor.

