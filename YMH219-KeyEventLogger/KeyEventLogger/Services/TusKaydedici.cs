using KeyEventLogger.Models;

namespace KeyEventLogger.Services;

/// <summary>
/// Klavye olay kayıtlarını tutan, kayıt durumunu (aktif/durdu) yöneten ve istatistik üreten sınıf.
/// Yalnızca uygulamanın kendi formundan gelen olayları alır; sistem genelinde dinleme yapmaz.
/// </summary>
public class TusKaydedici
{
    private readonly List<TusKaydi> _kayitlar = new();

    public IReadOnlyList<TusKaydi> Kayitlar => _kayitlar;

    /// <summary>Kayıt yapılıyor mu?</summary>
    public bool KayitAktif { get; private set; }

    /// <summary>Kaydın en son başlatıldığı zaman.</summary>
    public DateTime? BaslamaZamani { get; private set; }

    /// <summary>Yeni bir kayıt eklendiğinde tetiklenir.</summary>
    public event EventHandler<TusKaydi>? KayitEklendi;

    /// <summary>Kayıt başlatıldığında veya durdurulduğunda tetiklenir.</summary>
    public event EventHandler? DurumDegisti;

    public void Baslat()
    {
        if (KayitAktif) return;
        KayitAktif = true;
        BaslamaZamani = DateTime.Now;
        DurumDegisti?.Invoke(this, EventArgs.Empty);
    }

    public void Durdur()
    {
        if (!KayitAktif) return;
        KayitAktif = false;
        DurumDegisti?.Invoke(this, EventArgs.Empty);
    }

    public void Temizle() => _kayitlar.Clear();

    /// <summary>Kayıt aktifse olayı listeye ekler. Eklendiyse true döner.</summary>
    public bool Ekle(TusKaydi kayit)
    {
        ArgumentNullException.ThrowIfNull(kayit);
        if (!KayitAktif) return false;

        _kayitlar.Add(kayit);
        KayitEklendi?.Invoke(this, kayit);
        return true;
    }

    /// <summary>Toplam tuş basma sayısı (yalnızca KeyDown olayları sayılır).</summary>
    public int ToplamTusBasma => _kayitlar.Count(k => k.Tur == OlayTuru.KeyDown);

    /// <summary>En sık basılan tuşları (KeyDown olaylarına göre) sayılarıyla döndürür.</summary>
    public List<(string Tus, int Adet)> EnSikTuslar(int adet = 5) =>
        _kayitlar.Where(k => k.Tur == OlayTuru.KeyDown)
                 .GroupBy(k => k.TusAdi)
                 .Select(g => (Tus: g.Key, Adet: g.Count()))
                 .OrderByDescending(x => x.Adet)
                 .ThenBy(x => x.Tus)
                 .Take(adet)
                 .ToList();

    /// <summary>Verilen tarih/saat aralığındaki kayıtları döndürür (sınırlar dahil).</summary>
    public List<TusKaydi> Filtrele(DateTime baslangic, DateTime bitis)
    {
        if (bitis < baslangic)
            throw new ArgumentException("Bitiş zamanı başlangıç zamanından önce olamaz.");
        return _kayitlar.Where(k => k.Zaman >= baslangic && k.Zaman <= bitis).ToList();
    }
}
