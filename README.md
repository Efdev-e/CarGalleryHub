# Agarte Car Gallery

Siyah-altın temalı, lüks bir araba galerisi / alışveriş sitesi. Kullanıcılar araba ilanlarına bakıp sepete ekleyebiliyor, gerçek bir ödeme altyapısı üzerinden satın alabiliyor. Adminler tarafında da istatistikler, kullanıcı yönetimi gibi bir panel var.

Basit bir CRUD projesi değil; sepet → sipariş → ödeme → durum takibi gibi tam bir e-ticaret akışı üzerine kurulu.

## Neyle yapıldı?

- **.NET 10.0**
- **Clean Architecture** — Domain, Application, Persistence, Infrastructure, Api ve MVC olmak üzere 6 ayrı proje var. Klasör sayısı fazla ama amaç düzeni korumak
- **Entity Framework Core + SQLite** — veritabanı tarafı
- **JWT authentication**
- **Iyzico** — gerçek ödeme entegrasyonu
- **Razor / MVC** — arayüz tarafı

## Proje yapısı (özet geçelim)

```
CarGalleryHub.Domain          → Entity'ler, enum'lar, temel yapılar
CarGalleryHub.Application     → DTO'lar, arayüzler, validasyonlar
CarGalleryHub.Persistence     → DbContext, repository'ler, Iyzico servisi
CarGalleryHub.Infrastructure  → JWT, auth yardımcıları
CarGalleryHub.Api             → REST API uçları
CarGalleryHub.MVC             → Kullanıcının gördüğü kısım, arayüz
```

## Neler var içeride?

- Kullanıcı & admin rolleri (Admin / Customer)
- Marka → Model → Araç şeklinde hiyerarşik araç yapısı (Audi → A4 → o spesifik araba gibi)
- İlan sistemi, sepet, sipariş, ödeme takibi (Pending / Paid / Failed / Refunded / Success)
- Adres ve resim yönetimi
- Admin paneli: gerçek zamanlı istatistikler (uydurma sayı yok, direkt veritabanından çekiliyor), kullanıcı düzenleme

## Görsel tema

Site tamamen **obsidian siyahı + altın** renk paletinde. Başlıklarda Cinzel, gövde metinlerinde Montserrat fontu kullanılıyor. Kartların üzerine gelince hafif yukarı kayıp altın rengi bir gölge oluşturuyor, bildirimler de temaya uygun şekilde tasarlanmış.

## Nasıl çalıştırılır?

1. Repo'yu klonla
2. `.NET 10 SDK` kurulu olsun
3. `CarGalleryHub.slnx` dosyasını aç
4. Api ve MVC projelerini ayağa kaldır

(Migration, connection string, Iyzico key gibi detayları kendi ortamına göre ayarlaman gerekecek — proje şu an "elden geçirilme" aşamasında, o yüzden production-ready falan demiyorum.)

## Notlar

- Proje aktif geliştiriliyor, yapı zamanla değişebilir.
- Issue veya PR açmak istersen sorun yok.

---

Sorun veya soru olursa issue üzerinden yazabilirsin.
