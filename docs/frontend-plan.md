# PhishGuard AI - Frontend Plan

Bu doküman, PhishGuard AI projesinin React tabanlı kullanıcı arayüzü için planlanan ekranları ve temel kullanıcı akışını özetler.

## Amaç

Frontend tarafında kullanıcının şüpheli e-posta metni veya URL girerek analiz sonucu alabileceği, risk seviyesini görebileceği ve analiz nedenlerini anlaşılır biçimde inceleyebileceği sade bir arayüz hedeflenmektedir.

## Planlanan Ekranlar

### 1. Ana Sayfa

- Projenin kısa tanıtımı
- E-posta analizi ve URL analizi seçenekleri
- Güvenlik farkındalığına yönelik kısa açıklama

### 2. URL Analiz Ekranı

- Kullanıcıdan analiz edilecek URL alınır.
- Backend tarafındaki `/api/analysis/url` endpointine istek gönderilir.
- Risk puanı, risk seviyesi ve tespit edilen risk göstergeleri gösterilir.

### 3. E-posta Analiz Ekranı

- Kullanıcıdan konu, gönderen ve e-posta içeriği alınır.
- Backend tarafındaki `/api/analysis/email` endpointine istek gönderilir.
- Aciliyet dili, kimlik bilgisi talebi, ödeme bilgisi talebi ve şüpheli ek ifadeleri gibi bulgular gösterilir.

### 4. Analiz Sonuç Ekranı

- Risk skoru
- Risk seviyesi
- Özet açıklama
- Tespit edilen kurallar
- Kullanıcıya güvenlik önerisi

### 5. Geçmiş Analizler

- Kullanıcının daha önce yaptığı analizlerin listelenmesi planlanmaktadır.
- Bu ekran, veritabanı bağlantısı tamamlandıktan sonra geliştirilecektir.

### 6. Admin Dashboard

- Toplam analiz sayısı
- Düşük / orta / yüksek risk dağılımı
- En sık tetiklenen risk kuralları
- Son analizler

Bu ekran, JWT ve rol tabanlı yetkilendirme eklendikten sonra geliştirilecektir.

## Öncelikli Frontend Geliştirme Sırası

1. React + Vite proje iskeletinin oluşturulması
2. URL analiz formunun hazırlanması
3. E-posta analiz formunun hazırlanması
4. Analiz sonucu bileşeninin oluşturulması
5. API bağlantılarının yapılması
6. Tasarımın sade ve anlaşılır hâle getirilmesi
7. Geçmiş analizler ve admin dashboard ekranlarının eklenmesi

## Tasarım Yaklaşımı

- Sade ve anlaşılır arayüz
- Düşük risk için yeşil, orta risk için turuncu, yüksek risk için kırmızı görsel vurgu
- Her risk göstergesinin kullanıcıya açıklanması
- Teknik olmayan kullanıcıların da anlayabileceği sonuç metinleri
