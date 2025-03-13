# Film API

Film API, kullanıcıların film bilgilerini yönetebilecekleri, listeleyebilecekleri ve arayabilecekleri modern bir RESTful API'dir. Bu proje, ASP.NET Core kullanılarak Clean Architecture prensiplerine uygun olarak geliştirilmiştir.

## 🚀 Özellikler

- **Film Yönetimi**: Filmleri listeleme, detay görüntüleme, ekleme, güncelleme ve silme
- **Önbellekleme**: MemoryCache ile performans optimizasyonu
- **Hata Yönetimi**: Global exception handling ve Polly ile yeniden deneme mekanizması
- **Validasyon**: Giriş verilerinin doğrulanması
- **Dokümantasyon**: Swagger/OpenAPI entegrasyonu
- **Testler**: Kapsamlı unit testler

## 🛠️ Teknolojiler

- **ASP.NET Core 9.0**: Modern web API geliştirme
- **Dapper**: Hafif ve hızlı ORM
- **SQL Server**: Veritabanı
- **MemoryCache**: Önbellekleme
- **Swagger/OpenAPI**: API dokümantasyonu
- **xUnit, Moq**: Unit testler

## 📋 Gereksinimler

- .NET 9.0 SDK
- SQL Server veya LocalDB
- Visual Studio 2022 / VS Code / JetBrains Rider

## 🔧 Kurulum

1. Projeyi klonlayın:
   ```bash
   git clone https://github.com/KULLANICI_ADI/FilmAPI.git
   cd FilmAPI
   ```

2. Veritabanı bağlantı ayarlarını kontrol edin:
   `FilmAPI/src/FilmAPI.API/appsettings.json` dosyasındaki bağlantı dizesini kendi ortamınıza göre düzenleyin.

3. Projeyi derleyin:
   ```bash
   dotnet build
   ```

4. API'yi çalıştırın:
   ```bash
   dotnet run --project src/FilmAPI.API/FilmAPI.API.csproj
   ```

5. Tarayıcınızda Swagger arayüzüne erişin:
   ```
   http://localhost:5092
   ```

## 🧪 Testleri Çalıştırma

Unit testleri çalıştırmak için:

```bash
dotnet test tests/FilmAPI.UnitTests/FilmAPI.UnitTests.csproj
```

## 📚 API Kullanımı

API aşağıdaki endpoint'leri sağlar:

### Film İşlemleri

- `GET /api/movies` - Tüm filmleri listeler
- `GET /api/movies/{id}` - Belirli bir filmin detaylarını getirir
- `POST /api/movies` - Yeni bir film ekler
- `PUT /api/movies/{id}` - Var olan bir filmi günceller
- `DELETE /api/movies/{id}` - Bir filmi siler

### Örnek İstek ve Yanıtlar

#### Tüm Filmleri Getir

```http
GET /api/movies
```

Yanıt:
```json
{
  "success": true,
  "message": "Filmler başarıyla getirildi",
  "data": [
    {
      "id": 1,
      "title": "Esaretin Bedeli",
      "director": "Frank Darabont",
      "releaseYear": 1994,
      "genre": "Dram",
      "rating": 9.3
    },
    // Diğer filmler...
  ]
}
```

#### Film Ekle

```http
POST /api/movies
Content-Type: application/json

{
  "title": "Yeni Film",
  "director": "Yönetmen Adı",
  "releaseYear": 2023,
  "genre": "Aksiyon",
  "rating": 8.5
}
```

Yanıt:
```json
{
  "success": true,
  "message": "Film başarıyla eklendi",
  "data": 11
}
```

## 📁 Proje Yapısı

```
FilmAPI/
├── src/
│   ├── FilmAPI.API/           # API katmanı
│   ├── FilmAPI.Application/   # Uygulama katmanı
│   ├── FilmAPI.Core/          # Çekirdek katman
│   └── FilmAPI.Infrastructure/# Altyapı katmanı
├── tests/
│   ├── FilmAPI.UnitTests/     # Unit testler
│   └── FilmAPI.IntegrationTests/ # Entegrasyon testleri
└── README.md
```

## 🤝 Katkıda Bulunma

1. Bu repo'yu fork edin
2. Feature branch'i oluşturun (`git checkout -b feature/amazing-feature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add some amazing feature'`)
4. Branch'inize push edin (`git push origin feature/amazing-feature`)
5. Pull Request oluşturun

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Daha fazla bilgi için `LICENSE` dosyasına bakın.

## 📞 İletişim

Proje Sahibi - [@GITHUB_KULLANICI_ADI](https://github.com/GITHUB_KULLANICI_ADI)

Proje Linki: [https://github.com/GITHUB_KULLANICI_ADI/FilmAPI](https://github.com/GITHUB_KULLANICI_ADI/FilmAPI) 