# ChatApp

ChatApp, kullanıcılar ile operatörlerin gerçek zamanlı iletişim kurabilmesini sağlayan bir web uygulamasıdır.
Proje kapsamında hem kullanıcı tarafında gömülebilir bir chat widget, hem de operatör tarafında sohbet ve yönetim ekranları geliştirilmiştir.

## Özellikler

-Gerçek Zamanlı Mesajlaşma: Kullanıcı ve operatör arasındaki mesajlar SignalR ile anlık olarak iletilir.

-Birden Fazla Sohbet: Operatör aynı anda birden fazla kullanıcı ile ayrı ayrı sohbet başlatabilir ve yönetebilir. Ayrıca, operatör başka bir kullanıcıyla konuşurken farklı bir kullanıcıdan yeni mesaj gelirse ilgili sohbet penceresinde kırmızı uyarı işareti belirir.

-Yazıyor Göstergesi: Hem kullanıcı hem de operatör için "yazıyor..." bildirimi görünür.

-Dosya ve Görsel Paylaşımı: Kullanıcılar sadece metin değil, aynı zamanda dosya ve görsel de gönderebilir.

-Tam Ekran Görsel Önizleme: Gönderilen görseller büyütülerek görüntülenebilir.

### Operatör Dashboard:

-Toplam kullanıcı sayısı

-Son 5 dakikadaki aktif oturum sayısı

-Günlük mesaj sayıları (line chart)

-Kullanıcı bazlı mesaj istatistikleri (bar chart)

-Mesaj tipi dağılımı (pie chart)

-Otomatik 10 saniyede bir veri güncelleme

Dashboard tarafında görselleştirmeler için Chart.js kütüphanesi kullanılmıştır.

## Kullanılan Teknolojiler

-ASP.NET Core 8.0

-SignalR 

-Entity Framework Core 

-SQL Server

-HTML, CSS, JavaScript 


## ChatApp – Çalıştırma ve Test Kılavuzu

## 1. Gerekli Yazılımlar
- .NET 8 SDK veya üzeri
- Visual Studio 2022 veya Visual Studio Code
- SQL Server (Express veya tam sürüm)

---

## 2. Veritabanı Ayarları
`appsettings.json` içindeki bağlantıyı kendi bilgisayarınıza göre düzenleyin:

```
"ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=ChatAppDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
`````
---

## 3. Veritabanını Oluşturma

Visual Studio’da Package Manager Console kullanarak:

```
Update-Database
```
powershell
veya CLI ile:
```
dotnet ef database update
```
---

## 4. Uygulamayı Çalıştırma

Projeyi Visual Studio veya VS Code ile açın.

launchSettings.json dosyasında uygulamanın çalışacağı port ve URL ayarlanmıştır:
```
"applicationUrl": "https://localhost:7125;http://localhost:5001"
```
Uygulamayı başlatmak için F5 veya Ctrl+F5 tuşlarını kullanın.

---

Not: AgentPanel ve AgentDashboard sayfalarına erişmek için URL’in sonuna sayfa isimlerini ekleyin:

https://localhost:7125/AgentPanel

https://localhost:7125/AgentDashboard

## 5. Test HTML ve Widget Kullanımı
Projeyi başlattığınızda (F5 veya Ctrl+F5), launchSettings.json içindeki URL ve port geçerli olacaktır.

testhtml.html dosyasını tarayıcınızda açın.

testhtml.html dosyasındaki
```
<script src="/widget.js" data-endpoint="YOUR_ENDPOINT_URL"></script>
```
kısmı boş bırakılabilir. widget.js bu değeri window.location.origin kullanarak otomatik olarak doldurur.

Tarayıcı açıldığında, kullanıcı ID'si localStorage'a kaydedilir ve SignalR endpoint otomatik olarak seçilir.

Eğer uygulamayı farklı bir portta çalıştırıyorsanız, ekstra bir ayar yapmanıza gerek yoktur, window.location.origin bu durumu otomatik olarak algılar.


## Ekran Görüntüleri

<img width="1920" height="965" alt="Ekran Görüntüsü (1664)" src="[Screenshots/Ekran_Goruntusu_(1664).png](https://github.com/zynpc/ChatApp/blob/master/Screenshots/Ekran%20G%C3%B6r%C3%BCnt%C3%BCs%C3%BC%20(1664).png
)" />


<img width="1920" height="1014" alt="Ekran Görüntüsü (1663)" src="[Screenshots/Ekran_Goruntusu_(1663).png](https://github.com/zynpc/ChatApp/blob/master/Screenshots/Ekran%20G%C3%B6r%C3%BCnt%C3%BCs%C3%BC%20(1663).png
)" />


<img width="1920" height="919" alt="Ekran Görüntüsü (1662)" src="[Screenshots/Ekran_Goruntusu_(1662).png](https://github.com/zynpc/ChatApp/blob/master/Screenshots/Ekran%20G%C3%B6r%C3%BCnt%C3%BCs%C3%BC%20(1662).png
)" />


<img width="1920" height="1017" alt="Ekran Görüntüsü (1667)" src="[Screenshots/Ekran_Goruntusu_(1667).png](https://github.com/zynpc/ChatApp/blob/master/Screenshots/Ekran%20G%C3%B6r%C3%BCnt%C3%BCs%C3%BC%20(1667).png
)" />


<img width="1920" height="1014" alt="Ekran Görüntüsü (1669)" src="[Screenshots/Ekran_Goruntusu_(1669).png](https://github.com/zynpc/ChatApp/blob/master/Screenshots/Ekran%20G%C3%B6r%C3%BCnt%C3%BCs%C3%BC%20(1669).png
)" />


<img width="1920" height="1021" alt="Ekran Görüntüsü (1670)" src="[Screenshots/Ekran_Goruntusu_(1670).png](https://github.com/zynpc/ChatApp/blob/master/Screenshots/Ekran%20G%C3%B6r%C3%BCnt%C3%BCs%C3%BC%20(1670).png)" />




