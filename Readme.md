# Core.Packages

Core.Packages, .NET uygulamaları için temel altyapı bileşenlerini içeren kapsamlı bir kütüphane paketidir. Bu paket, modern uygulamaların ihtiyaç duyduğu yaygın özellikleri ve en iyi uygulamaları içerir.

## 🚀 Özellikler

- **Kimlik Doğrulama & Yetkilendirme**
  - JWT tabanlı kimlik doğrulama
  - Rol bazlı yetkilendirme
  - Refresh token desteği
  - Token blacklist yönetimi

- **Önbellekleme**
  - Memory Cache desteği
  - Redis Cache entegrasyonu
  - Esnek önbellekleme stratejileri

- **Kesişen İlgiler (Cross-Cutting Concerns)**
  - Loglama
  - Exception handling
  - Validation
  - Caching aspects

- **Güvenlik**
  - Şifreleme servisleri
  - Güvenli token yönetimi
  - Permission bazlı yetkilendirme

## 🛠️ Kurulum

Package Manager Console üzerinden:
```
bash
Install-Package Core.Packages

bash
dotnet add package Core.Packages
```

## 🔧 Kullanım
```
Program.cs veya Startup.cs
services.AddCoreInfrastructure(Configuration);
```


## 📋 Gereksinimler

- .NET 9.0 veya üzeri
- Microsoft.EntityFrameworkCore 9.0.0 veya üzeri

## 🤝 Katkıda Bulunma

1. Bu repository'yi fork edin
2. Feature branch'inizi oluşturun (`git checkout -b feature/AmazingFeature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add some AmazingFeature'`)
4. Branch'inizi push edin (`git push origin feature/AmazingFeature`)
5. Pull Request oluşturun

## 🙏 Teşekkürler

Bu projeye katkıda bulunan herkese teşekkür ederiz. Özel teşekkürler:

- [@HasanErenAkgoz](https://github.com/HasanErenAkgoz) - Proje sahibi ve ana geliştirici

## ⭐ Projeyi Destekleyin
Eğer bu proje size yardımcı olduysa, ⭐️ vermeyi unutmayın! Bu, projenin daha fazla kişiye ulaşmasına yardımcı olur.


