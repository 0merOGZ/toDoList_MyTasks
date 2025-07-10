# MyTasks - Takım ve Kişisel Görev Yönetimi Uygulaması

Bu proje, bireysel ve takım bazlı görev yönetimi için geliştirilmiş modern bir ASP.NET Core MVC uygulamasıdır. Kullanıcılar, görev ekleyebilir, kategorilere ayırabilir, durumlarını takip edebilir ve takım arkadaşlarına görev atayabilirler.

## Özellikler

- **Kullanıcı Yönetimi:** Kayıt olma, giriş yapma, çıkış yapma.
- **Rol Sistemi:** Takım Lideri ve Takım Üyesi rolleri.
- **Takım Yönetimi:** Takım oluşturma (lider), davet kodu ile takıma katılma (üye).
- **Görev Yönetimi:**
  - Görev ekleme, düzenleme, silme.
  - Görevleri kategoriye, aciliyetine, durumuna göre filtreleme ve sıralama.
  - Takım lideri, takım üyelerine görev atayabilir.
  - Tamamlanan görevleri arşivleme.
- **Kategoriler:** Kişisel, iş, okul, ilişki, sağlık, finans, diğer.
- **Durumlar:** Beklemede, tamamlandı.
- **Modern ve responsive arayüz:** Bootstrap ve özel CSS ile şık tasarım.

## Kurulum ve Çalıştırma

### Gereksinimler

- [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- SQL Server (veya LocalDB)
- Visual Studio 2022+ veya VS Code

### Adımlar

1. **Projeyi klonlayın:**
   ```sh
   git clone <repo-url>
   cd tod
   ```

2. **Bağımlılıkları yükleyin:**
   - NuGet paketleri otomatik olarak yüklenecektir. Gerekirse:
     ```sh
     dotnet restore
     ```

3. **Veritabanı bağlantısını ayarlayın:**
   - `appsettings.json` dosyasındaki `ConnectionStrings:Context` kısmını kendi SQL Server ayarınıza göre güncelleyin.
   - Varsayılan:  
     "Context": "Server=(localdb)\\mssqllocaldb;Database=tod;Trusted_Connection=True;MultipleActiveResultSets=true"

4. **Veritabanı migrasyonlarını uygulayın:**
   ```sh
   dotnet ef database update
   ```

5. **Projeyi başlatın:**
   ```sh
   dotnet run
   ```
   veya Visual Studio ile F5

6. **Uygulamaya erişin:**
   - [http://localhost:5244](http://localhost:5244) veya [https://localhost:7140](https://localhost:7140)

## Kullanım

- **Kayıt Ol:** Rol seçerek (Takım Lideri veya Üye) kayıt olabilirsiniz.
- **Takım Lideri:** Kayıt sonrası otomatik takım oluşturulur, davet kodu ile üyeler eklenebilir.
- **Takım Üyesi:** Kayıt sonrası davet kodu ile bir takıma katılabilirsiniz.
- **Görev Ekle:** Kendi görevlerinizi veya lider olarak takım üyelerine görev atayabilirsiniz.
- **Filtreleme & Sıralama:** Görevleri kategori, durum, tarih ve aciliyet gibi kriterlere göre filtreleyebilirsiniz.
- **Arşiv:** Tamamlanan görevler arşivlenebilir.

## Proje Yapısı

- `Controllers/` - MVC Controller'lar
- `Models/` - Entity ve veri modelleri
- `Views/` - Razor View dosyaları (sayfa arayüzleri)
- `Migrations/` - EF Core veritabanı migrasyonları
- `wwwroot/` - Statik dosyalar (CSS, JS, görseller)

## Bağımlılıklar

- ASP.NET Core MVC
- Entity Framework Core (SQL Server)
- Bootstrap 5
- jQuery

## Katkı ve Lisans

Bu proje bir staj prototipidir. Katkıda bulunmak için fork'layabilir, pull request gönderebilirsiniz. 