
# CorePackages: .NET Geliştiriciler İçin Temel Altyapı

## 📌 Proje Hakkında
CorePackages, .NET geliştiricilerin yeni bir projeye başlarken gereksiz iş yükünden kurtulmasını ve projelerine hızlı bir başlangıç yapmasını sağlamak amacıyla oluşturulmuştur. 

Bu proje, **.NET 9** kullanılarak **Clean Architecture** prensiplerine uygun bir şekilde geliştirilmiştir. İçerisinde bir .NET projesinde bulunması gereken temel yapı ve bileşenler yer almaktadır.

## 🚀 Özellikler
CorePackages, modern bir .NET projesinin ihtiyaç duyduğu aşağıdaki bileşenleri içermektedir:

- **✅ Repository Pattern**: Veri erişim katmanında standart ve esnek bir yapı sunar.
- **✅ Result Pattern**: Metod dönüşlerinde başarılı/başarısız durumları yönetmek için kullanılır.
- **✅ JWT Authentication**: Güvenli kimlik doğrulama ve yetkilendirme mekanizması içerir.
- **✅ Role & Permission Yönetimi**: **CQRS Handler Bazlı Otomatik Yetkilendirme** sistemi.
- **✅ Bildirim Servisi**: **E-posta ve SMS** gönderimi için Notification Service.
- **✅ Logging (Serilog)**: Proje boyunca detaylı loglama desteği sunar.
- **✅ Background Jobs (Hangfire)**: Asenkron işlemler için arka plan görevlerini yönetir.
- **✅ Exception Handling & Validation**: Merkezi hata yönetimi ve FluentValidation entegrasyonu.
- **✅ OpenAPI (Swagger)**: API endpointlerinin belgelenmesi için entegre edilmiş OpenAPI desteği.
- **✅ Genişletilebilir Modüler Yapı**: Proje ihtiyaçlarınıza uygun şekilde genişletilebilir.
- ✅ Redis İmplementasyonu  **:  Verilerinize daha hızlı erişebilmeniz için Redis İmplementasyonu yapılmıştır.
- 
## 🎯 Hedef Kitle
Bu proje, **.NET geliştiricileri** için hazırlanmıştır. Yeni bir projeye başlarken temel bileşenleri tekrar tekrar yazmak yerine, CorePackages kullanılarak zaman kazandıran bir altyapı sunar.

## 🛠 Kullanılan Teknolojiler
- **.NET 9**
- **Clean Architecture**
- **CQRS + MediatR**
- **Entity Framework Core**
- **JWT Authentication**
- **FluentValidation**
- **Serilog** (Loglama için)
- **Hangfire** (Arka plan işlemleri için)
- **Swagger / OpenAPI** (API dokümantasyonu için)
- **AutoMapper** (Veri dönüşümleri için)
- **Redis** (Cache)

## 📌 Kurulum ve Kullanım
Projeyi sisteminize indirmek ve kullanmaya başlamak için aşağıdaki adımları takip edebilirsiniz:

### 🔹 Bağımlılıklar
Projede yer alan bağımlılıkları yüklemek için şu komutu çalıştırabilirsiniz:
```bash
   dotnet restore
```

### 🔹 Veritabanı Yapılandırması
1. **appsettings.json** dosyasında veritabanı bağlantı ayarlarını yapın.
2. Veritabanını oluşturmak ve migrate işlemlerini tamamlamak için:
```bash
   dotnet ef database update
```

### 🔹 Projeyi Çalıştırma
Projeyi çalıştırmak için aşağıdaki komutu kullanabilirsiniz:
```bash
   dotnet run
```

Alternatif olarak, **Visual Studio**, **Rider** veya **VS Code** üzerinden de çalıştırabilirsiniz.

## 🛠 API Kullanımı
API endpointlerini incelemek ve test etmek için projenin çalıştırılmasının ardından **Swagger UI** arayüzüne aşağıdaki URL üzerinden erişebilirsiniz:
```
http://localhost:5000/swagger/index.html
```

## ⚙️ Örnek appsettings.json Yapılandırması
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=CorePackagesTestDb;User Id=myuser;Password=mypassword;TrustServerCertificate=True;"
  },
  "TokenOptions": {
    "Audience": "www.example.com",
    "Issuer": "www.example.com",
    "AccessTokenExpiration": 30,
    "SecurityKey": "MySuperSecretKey!1234567890ABCDEF"
  },
  "EmailSettings": {
    "SmtpServer": "smtp.example.com",
    "SmtpPort": 587,
    "SmtpUser": "your-email@example.com",
    "SmtpPass": "your-secure-password",
    "FromEmail": "your-email@example.com"
  },
  "SmsSettings": {
    "AccountSid": "YourTwilioAccountSid",
    "AuthToken": "YourTwilioAuthToken",
    "FromPhoneNumber": "+1234567890"
  },
  "Redis": {
  "ConnectionString": "localhost:Adresss",
  "InstanceName": "SpecialInstanceName",
  "UseSsl": false,
  "Password": "your-secure-password"
},
}
```


**Kullanım Senaryoları**

 - **Email Service Kullanımı**; Proje içerisinde bulunan IEmailService interface'ini çağırıp içerisinde bulunan SendEmailAsync fonksiyonunu kullanmanız gerekmektedir.
Örnek Kullanım; 
````
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, IResult>
    {
        private readonly IEmailService _emailService;
        private readonly IAuthenticationService _authenticationService;
        public SendEmailCommandHandler(IEmailService emailService, IAuthenticationService authenticationService)
        {
            _emailService = emailService;
            _authenticationService = authenticationService;
        }

        public async Task<IResult> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            _authenticationService.EnsurePermissionForHandler<SendEmailCommandHandler>();
            try
            {
                await _emailService.SendEmailAsync(request.To, request.Subject, request.Body);
                return new SuccessResult(Messages.SendEmail);
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }
    }
````

**Redis Kullanımı**
UserId kısmı sisteme Authentice olan User dan otomatik olarak gelecektir.
````
   [Cache("permissions_{UserId}", 30)]
   public class GetAllPermissionQuery : IRequest<IDataResult<IEnumerable<GetPermissionResponse>>>
   {

   }
````

**AuthoMapper Kullanımı;**
````
   public sealed class GetPermissionResponse : IMapFrom<Core.Packages.Domain.Entities.Permission>
   {
       public string Name { get; set; }
       public string Description { get; set; }

       public void Mapping(Profile profile)
       {
           profile.CreateMap<Core.Packages.Domain.Entities.Permission, GetPermissionResponse>();
       }
   }
````


## 📖 Katkıda Bulunma
Projeye katkıda bulunmak isterseniz **Pull Request** gönderebilir veya **Issue** oluşturarak geri bildirimde bulunabilirsiniz.

## 📩 İletişim
Eğer proje ile ilgili herhangi bir sorunuz veya öneriniz varsa, benimle iletişime geçebilirsiniz:
---
Bu proje, .NET geliştiricilerinin projelerine hızlı bir şekilde başlamalarına yardımcı olmak için geliştirilmiştir. 🎯
