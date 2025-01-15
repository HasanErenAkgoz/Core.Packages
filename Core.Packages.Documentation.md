# Core.Packages Dökümantasyonu

## 1. Güvenlik Modülleri (Security Package)

### 1.1. JWT (JSON Web Token) Servisleri
- **JwtHelper**: Token oluşturma ve doğrulama işlemlerini yönetir
- **TokenOptions**: JWT yapılandırma ayarlarını içerir (süre, issuer, audience vb.)

### 1.2. OAuth 2.0 Servisleri
- **OAuthManager**: OAuth 2.0 akışını yönetir
- **Veritabanı Tablosu**: `OAuthClientEntity`
  ```csharp
  - Id (int)
  - ClientId (string)
  - ClientSecret (string, hash'lenmiş)
  - RedirectUris (string, JSON)
  - AllowedGrantTypes (string, JSON)
  - AllowedScopes (string, JSON)
  - AccessTokenLifetime (int)
  - RefreshTokenLifetime (int)
  - Name (string)
  - Description (string)
  - RequirePkce (bool)
  - CreatedById (int)
  - CreatedDate (DateTime)
  - ModifiedById (int?)
  - ModifiedDate (DateTime?)
  - Status (EntityStatus)
  - DeletedDate (DateTime?)
  - DeletedById (int?)
  ```

### 1.3. İki Faktörlü Kimlik Doğrulama (2FA)
- **TwoFactorAuthService**: 2FA işlemlerini yönetir
- **Desteklenen Yöntemler**:
  - Authenticator (Google Authenticator, Microsoft Authenticator vb.)
  - Email
  - SMS
  - Yedek Kodlar

- **Veritabanı Tabloları**:
  1. `TwoFactorDataEntity`:
  ```csharp
  - Id (int)
  - UserId (string)
  - Type (TwoFactorType)
  - SecretKey (byte[])
  - IsEnabled (bool)
  - LockoutEnd (DateTime?)
  - FailedAttempts (int)
  - CreatedById (int)
  - CreatedDate (DateTime)
  - ModifiedById (int?)
  - ModifiedDate (DateTime?)
  - Status (EntityStatus)
  - DeletedDate (DateTime?)
  - DeletedById (int?)
  ```

  2. `TwoFactorCodeEntity`:
  ```csharp
  - Id (int)
  - UserId (string)
  - Code (string)
  - ExpiresAt (DateTime)
  - Type (TwoFactorType)
  - IsUsed (bool)
  - Purpose (string)
  - CreatedById (int)
  - CreatedDate (DateTime)
  - ModifiedById (int?)
  - ModifiedDate (DateTime?)
  - Status (EntityStatus)
  - DeletedDate (DateTime?)
  - DeletedById (int?)
  ```

## 2. Domain Katmanı

### 2.1. Temel Sınıflar
- **Entity<TId>**: Tüm entity'ler için temel sınıf
  ```csharp
  - Id (TId)
  - CreatedById (int)
  - CreatedDate (DateTime)
  - ModifiedById (int?)
  - ModifiedDate (DateTime?)
  - Status (EntityStatus)
  - DeletedDate (DateTime?)
  - DeletedById (int?)
  ```

### 2.2. Enums
- **EntityStatus**:
  ```csharp
  - Active (1)
  - Passive (2)
  - Deleted (3)
  ```

- **TwoFactorType**:
  ```csharp
  - None (0)
  - Email (1)
  - Phone (2)
  - Authenticator (3)
  - Sms (4)
  ```

## 3. Application Katmanı

### 3.1. Validation
- FluentValidation ile validasyon kuralları
- Custom validasyon kuralları için extension'lar

### 3.2. Cross-Cutting Concerns
- **Email Service**: Email gönderimi için servis
- **Caching**: Önbellekleme işlemleri
- **Logging**: Log yönetimi
- **Exception Handling**: Hata yönetimi

## 4. Özellikler ve Yetenekler

### 4.1. OAuth 2.0
- Client kayıt ve yönetimi
- Scope bazlı yetkilendirme
- Access ve Refresh token desteği
- PKCE desteği

### 4.2. İki Faktörlü Kimlik Doğrulama
- TOTP (Time-based One-Time Password) desteği
- Email ve SMS ile doğrulama
- Yedek ve kurtarma kodları
- Brute-force koruması (kilitleme mekanizması)

### 4.3. Genel Özellikler
- Soft delete desteği
- Audit logging (kim, ne zaman oluşturdu/değiştirdi)
- Entity durumu yönetimi (Active, Passive, Deleted)
- Güvenli şifre hash'leme

## 5. Kullanım Örnekleri

### 5.1. OAuth Client Oluşturma
```csharp
var client = new OAuthClient
{
    ClientId = "client_id",
    ClientSecret = "client_secret",
    Name = "Test Client",
    Description = "Test OAuth Client",
    AllowedScopes = new List<string> { "read", "write" },
    RedirectUris = new List<string> { "https://localhost:5001/callback" },
    RequirePkce = true
};

await _oAuthClientStore.SaveClientAsync(client);
```

### 5.2. İki Faktörlü Kimlik Doğrulama Aktifleştirme
```csharp
// TOTP için secret key ve QR kod URI'si oluşturma
var (secretKey, qrCodeUri) = await _twoFactorAuthService.GenerateTotpSecretAsync(userId, userEmail);

// Kullanıcının girdiği kodu doğrulama ve 2FA'yı aktifleştirme
bool isValid = await _twoFactorAuthService.EnableTotpAsync(userId, userCode);
```

### 5.3. JWT Token Oluşturma
```csharp
var token = _jwtHelper.CreateToken(user, roles);
// token.AccessToken ve token.RefreshToken kullanılabilir
```

## 6. Veritabanı Şeması

### 6.1. OAuth Tabloları
- **OAuthClients**: OAuth istemcilerini saklar
- İlişkiler:
  - Soft delete ve audit özellikleri için temel entity'den kalıtım alır

### 6.2. İki Faktörlü Kimlik Doğrulama Tabloları
- **TwoFactorData**: Kullanıcıların 2FA ayarlarını saklar
- **TwoFactorCodes**: Doğrulama kodlarını saklar
- İlişkiler:
  - Her iki tablo da soft delete ve audit özellikleri için temel entity'den kalıtım alır

## 7. Güvenlik Özellikleri

### 7.1. Şifreleme ve Hash'leme
- Client secret'lar için güvenli hash'leme
- TOTP secret key'ler için güvenli depolama
- JWT token'lar için güvenli imzalama

### 7.2. Brute Force Koruması
- Başarısız giriş denemelerinde kilitleme
- Yapılandırılabilir kilitleme süresi ve deneme sayısı
- IP bazlı rate limiting desteği

### 7.3. PKCE Desteği
- OAuth 2.0 PKCE (Proof Key for Code Exchange) desteği
- Mobile ve SPA uygulamaları için güvenli kimlik doğrulama

## 8. Yapılandırma

### 8.1. JWT Yapılandırması
```json
{
  "TokenOptions": {
    "Audience": "www.myapp.com",
    "Issuer": "www.myapp.com",
    "AccessTokenExpiration": 10,
    "SecurityKey": "your_secret_key_here"
  }
}
```

### 8.2. İki Faktörlü Kimlik Doğrulama Yapılandırması
```json
{
  "TwoFactorAuthOptions": {
    "IssuerName": "MyApp",
    "CodeLength": 6,
    "CodeValidityPeriod": 30,
    "MaxFailedAttempts": 5,
    "LockoutDuration": 900,
    "BackupCodesCount": 8,
    "RecoveryCodesCount": 8
  }
}
``` 