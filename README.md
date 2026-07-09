# PhishGuard AI

PhishGuard AI, şüpheli e-posta metinlerini ve URL bağlantılarını analiz ederek oltalama, sosyal mühendislik, sahte bağlantı ve risk göstergelerini açıklanabilir şekilde değerlendirmeyi amaçlayan bir siber güvenlik farkındalık platformudur.

## Proje Durumu

Bu proje geliştirme aşamasındadır.

Şu ana kadar yapılanlar:

- GitHub repository oluşturuldu.
- Proje yerel ortama klonlandı.
- .NET 10 LTS SDK kurulumu yapıldı.
- ASP.NET Core Web API backend iskeleti oluşturuldu.
- OpenAPI paketinde görülen güvenlik uyarısı giderildi.
- Backend uygulaması yerel ortamda çalıştırıldı ve `/weatherforecast` ile `/openapi/v1.json` endpointleri test edildi.

## Planlanan Teknoloji Yığını

- Backend: ASP.NET Core Web API (.NET 10)
- Frontend: React + Vite
- Veritabanı: PostgreSQL
- API Dokümantasyonu: OpenAPI / Swagger
- Kimlik Doğrulama: JWT
- Yetkilendirme: Role-Based Access Control
- Container: Docker ve Docker Compose
- Orkestrasyon: Kubernetes
- Opsiyonel: Redis ve LLM entegrasyonu

## Planlanan Özellikler

- E-posta metni analizi
- URL/link analizi
- Kural tabanlı risk puanlama
- Düşük, orta ve yüksek risk seviyesi
- Risk nedenlerinin açıklanması
- Kullanıcı analiz geçmişi
- Admin dashboard
- Docker ile çalıştırma
- Kubernetes YAML dosyaları

## Geliştirici

Şemsi Dilara Ulutaş  
Bilgisayar Mühendisliği
