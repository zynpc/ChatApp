# ChatApp

# ChatApp – Çalıştırma ve Test Kılavuzu

## 1. Gerekli Yazılımlar
- .NET 8 SDK veya üzeri
- Visual Studio 2022 veya Visual Studio Code
- SQL Server (Express veya tam sürüm)

---

## 2. Veritabanı Ayarları
`appsettings.json` içindeki bağlantıyı kendi bilgisayarınıza göre düzenleyin:

```bash
"ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=ChatAppDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
`````
bash

## 3. Veritabanını Oluşturma

Visual Studio’da Package Manager Console kullanarak:

```powershell
Update-Database
```
powershell
veya CLI ile:
```powershell
dotnet ef database update
```
powershell

## 4. Uygulamayı Çalıştırma

Projeyi Visual Studio veya VS Code ile açın.

launchSettings.json dosyasında uygulamanın çalışacağı port ve URL ayarlanmıştır:

"applicationUrl": "https://localhost:7125;http://localhost:5001"

Uygulamayı başlatmak için F5 veya Ctrl+F5 tuşlarını kullanın.


Not: AgentPanel ve AgentDashboard sayfalarına erişmek için URL’in sonuna sayfa isimlerini ekleyin:

https://localhost:7125/AgentPanel

https://localhost:7125/AgentDashboard

## 5. Test HTML ve Widget Kullanımı
Projeyi başlattığınızda (F5 veya Ctrl+F5), launchSettings.json içindeki URL ve port geçerli olacaktır.

test.html dosyasını tarayıcınızda açın.

test.html dosyasındaki <script src="/widget.js" data-endpoint="YOUR_ENDPOINT_URL"></script> kısmı boş bırakılabilir. widget.js bu değeri window.location.origin kullanarak otomatik olarak doldurur.

Tarayıcı açıldığında, kullanıcı ID'si localStorage'a kaydedilir ve SignalR endpoint otomatik olarak seçilir.

Eğer uygulamayı farklı bir portta çalıştırıyorsanız, ekstra bir ayar yapmanıza gerek yoktur, window.location.origin bu durumu otomatik olarak algılar.
