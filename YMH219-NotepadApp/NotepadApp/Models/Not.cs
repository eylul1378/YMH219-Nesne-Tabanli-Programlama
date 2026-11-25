namespace NotepadApp.Models;

/// <summary>
/// Tek bir notu temsil eden sınıf. JSON ile serileştirilerek dosyada saklanır.
/// </summary>
public class Not
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Baslik { get; set; } = string.Empty;

    /// <summary>Notun düz metin içeriği (arama ve .txt dışa aktarma için).</summary>
    public string Icerik { get; set; } = string.Empty;

    /// <summary>Notun biçimlendirilmiş (RTF) içeriği. Boşsa düz metin kullanılır.</summary>
    public string IcerikRtf { get; set; } = string.Empty;

    public string Kategori { get; set; } = "Genel";
    public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
    public DateTime DuzenlemeTarihi { get; set; } = DateTime.Now;

    // JSON serileştirici için parametresiz yapıcı gereklidir.
    public Not() { }

    public Not(string baslik, string icerik, string kategori = "Genel")
    {
        Baslik = baslik;
        Icerik = icerik;
        Kategori = string.IsNullOrWhiteSpace(kategori) ? "Genel" : kategori.Trim();
        OlusturmaTarihi = DateTime.Now;
        DuzenlemeTarihi = OlusturmaTarihi;
    }

    /// <summary>
    /// Notun bilgilerini günceller ve düzenleme tarihini yeniler.
    /// </summary>
    public void Guncelle(string baslik, string icerik, string icerikRtf, string kategori)
    {
        Baslik = baslik;
        Icerik = icerik;
        IcerikRtf = icerikRtf;
        Kategori = string.IsNullOrWhiteSpace(kategori) ? "Genel" : kategori.Trim();
        DuzenlemeTarihi = DateTime.Now;
    }

    /// <summary>
    /// Arama metninin başlıkta, içerikte veya kategoride geçip geçmediğini kontrol eder.
    /// </summary>
    public bool Eslesir(string aranan)
    {
        if (string.IsNullOrWhiteSpace(aranan)) return true;
        return Baslik.Contains(aranan, StringComparison.CurrentCultureIgnoreCase)
            || Icerik.Contains(aranan, StringComparison.CurrentCultureIgnoreCase)
            || Kategori.Contains(aranan, StringComparison.CurrentCultureIgnoreCase);
    }

    /// <summary>ListBox üzerinde görünecek metin.</summary>
    public override string ToString() => $"[{Kategori}] {Baslik}  ({DuzenlemeTarihi:dd.MM.yyyy HH:mm})";
}
