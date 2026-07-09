# Endless Runner - RatRush

Bu proje, Unity oyun motoru ile geliştirilmiş bir "Endless Runner" (Sonsuz Koşu) oyunudur. Oyunda karakterin ileriye koşması hissi, oyun dünyasındaki zemin parçalarının (Chunk'ların) karaktere doğru hareket ettirilmesiyle sağlanır.

## Proje Mimarisi

Oyun mimarisi, sorumlulukların farklı sınıflara bölündüğü modüler bir yapı üzerine inşa edilmiştir:
- **Managers (Yöneticiler):** Oyunun genel durumunu (`GameManager`), arayüzü (`UIManager`), bölüm/zemin yönetimini (`ChunkManager`) ve veri kayıt işlemlerini (`DataManager`) merkezi olarak yönetir.
- **Controllers (Kontrolcüler):** Oyuncu girdilerini, hareketlerini (`PlayerController`) ve ses efektlerini (`PlayerAudioController`) kontrol eden sınıflardır.
- **Entities & World:** Oyun dünyasındaki etkileşime geçilebilir nesneler (Peynir gibi toplanabilir eşyalar, engeller vb.) ve zemin parçaları (Chunk) bu bölümlerde tanımlanmıştır. Zeminler düzenli olarak yenilenerek sonsuzluk hissi oluşturur.

## Kullanılan Tasarım Kalıpları (Design Patterns)

Projenin performanslı ve yönetilebilir olması için aşağıdaki tasarım kalıplarından faydalanılmıştır:

- **Singleton Pattern:** Oyunda yalnızca bir kopyasının (instance) bulunması gereken yöneticilerde kullanılmıştır (`GameManager.instance`, `UIManager.instance`, `ChunkManager.instance` vb.). Bu sayede diğer sınıflar, yöneticilere kolayca erişebilir.
- **Object Pooling (Nesne Havuzu):** Sonsuz koşu oyunlarında performans çok önemli olduğu için zemin parçaları (`Chunk`) sürekli yaratılıp yok edilmez. Bunun yerine `ChunkPool` yardımıyla havuzdan çekilir ve ekran dışına çıkınca tekrar havuza gönderilir. Bu durum "Garbage Collection" yükünü büyük ölçüde hafifletir.
- **Observer Pattern (Event Bus / Delegate):** Oyun içi olayların iletişimini sağlamak için `GameEvents` sınıfı aracılığıyla bir Event/Delegate yapısı kullanılmıştır (Örn: `OnGameStart`, `OnGameOver`). Bu sayede sınıflar birbirine sıkı sıkıya bağlı kalmadan (loose coupling) olaylardan haberdar olur.
- **State Pattern (Durum Yapısı):** Oyunun anlık durumunu takip etmek (`GameStatus.Running`, `GameStatus.Over`) ve oyun akışını buna göre değiştirmek için kullanılmıştır.

## Kod Nasıl Çalıştırılır?

Projeyi kendi bilgisayarınızda çalıştırmak için aşağıdaki adımları izleyebilirsiniz:

1. Bu depoyu (repository) bilgisayarınıza klonlayın veya zip olarak indirin.
2. **Unity Hub** uygulamasını açın.
3. **Open (Aç)** butonuna tıklayıp projenin ana klasörünü seçerek projeyi Unity Hub'a ekleyin.
4. Projeye tıklayarak uygun Unity versiyonu ile editörü başlatın.
5. Editör açıldığında, **Project** penceresinden sahnelerin bulunduğu klasöre gidin ve ana sahneyi (örneğin "Game" isimli sahne) çift tıklayarak açın.
6. Üst menünün ortasında yer alan **Play (Oynat) ▶** butonuna basarak oyunu başlatıp test edebilirsiniz.

*Dilerseniz oyunu releases kısmından indirebilirsiniz.*

**Geliştirici:** Alaattin Buğra DURMUŞ
