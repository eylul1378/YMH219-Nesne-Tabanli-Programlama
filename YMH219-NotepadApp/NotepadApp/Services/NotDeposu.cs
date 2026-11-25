using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using NotepadApp.Models;

namespace NotepadApp.Services;

/// <summary>
/// Notları bellekte bir List&lt;Not&gt; koleksiyonunda tutar ve
/// JSON serileştirme ile dosyaya kaydeder / dosyadan yükler.
/// </summary>
public class NotDeposu
{
    private readonly List<Not> _notlar = new();

    private static readonly JsonSerializerOptions JsonAyarlari = new()
    {
        WriteIndented = true,
        // Türkçe karakterlerin dosyada okunabilir kalması için
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    /// <summary>Notların kaydedildiği varsayılan dosya yolu.</summary>
    public string DosyaYolu { get; private set; }

    public IReadOnlyList<Not> Notlar => _notlar;

    public NotDeposu(string dosyaYolu)
    {
        DosyaYolu = dosyaYolu;
    }

    /// <summary>Uygulama verileri için varsayılan konum: %AppData%\YMH219-NotepadApp\notlar.json</summary>
    public static string VarsayilanDosyaYolu()
    {
        string klasor = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "YMH219-NotepadApp");
        return Path.Combine(klasor, "notlar.json");
    }

    public void Ekle(Not not)
    {
        ArgumentNullException.ThrowIfNull(not);
        if (string.IsNullOrWhiteSpace(not.Baslik))
            throw new ArgumentException("Not başlığı boş olamaz.");
        _notlar.Add(not);
    }

    public bool Sil(Guid id) => _notlar.RemoveAll(n => n.Id == id) > 0;

    public Not? Bul(Guid id) => _notlar.FirstOrDefault(n => n.Id == id);

    /// <summary>Başlık, içerik veya kategoriye göre filtrelenmiş notları, en son düzenlenen başta olacak şekilde döndürür.</summary>
    public List<Not> Ara(string aranan) =>
        _notlar.Where(n => n.Eslesir(aranan))
               .OrderByDescending(n => n.DuzenlemeTarihi)
               .ToList();

    public IEnumerable<string> Kategoriler() =>
        _notlar.Select(n => n.Kategori).Distinct().OrderBy(k => k);

    /// <summary>
    /// Notları JSON biçiminde StreamWriter ile dosyaya yazar.
    /// Önce geçici dosyaya yazılır, sonra asıl dosyanın yerine konur; böylece yazma sırasında
    /// oluşabilecek bir hata mevcut dosyayı bozmaz.
    /// </summary>
    public void Kaydet(string? hedefYol = null)
    {
        string yol = hedefYol ?? DosyaYolu;
        string? klasor = Path.GetDirectoryName(yol);
        if (!string.IsNullOrEmpty(klasor))
            Directory.CreateDirectory(klasor);

        string geciciYol = yol + ".tmp";
        string json = JsonSerializer.Serialize(_notlar, JsonAyarlari);

        using (var yazici = new StreamWriter(geciciYol, false, Encoding.UTF8))
        {
            yazici.Write(json);
        }

        File.Move(geciciYol, yol, overwrite: true);

        if (hedefYol != null)
            DosyaYolu = hedefYol;
    }

    /// <summary>
    /// Notları JSON dosyasından StreamReader ile okur.
    /// Dosya bulunamazsa FileNotFoundException, içerik bozuksa InvalidDataException fırlatır.
    /// </summary>
    public void Yukle(string? kaynakYol = null)
    {
        string yol = kaynakYol ?? DosyaYolu;

        if (!File.Exists(yol))
            throw new FileNotFoundException("Not dosyası bulunamadı.", yol);

        string json;
        using (var okuyucu = new StreamReader(yol, Encoding.UTF8))
        {
            json = okuyucu.ReadToEnd();
        }

        List<Not>? yuklenen;
        try
        {
            yuklenen = string.IsNullOrWhiteSpace(json)
                ? new List<Not>()
                : JsonSerializer.Deserialize<List<Not>>(json, JsonAyarlari);
        }
        catch (JsonException ex)
        {
            throw new InvalidDataException("Not dosyası bozuk veya geçerli bir JSON değil.", ex);
        }

        _notlar.Clear();
        if (yuklenen != null)
            _notlar.AddRange(yuklenen.Where(n => n != null));

        if (kaynakYol != null)
            DosyaYolu = kaynakYol;
    }

    /// <summary>
    /// Bozuk bir dosyayı silmek yerine .bak uzantısıyla yedekler; kullanıcı verisi kaybolmaz.
    /// </summary>
    public string BozukDosyayiYedekle()
    {
        string yedek = DosyaYolu + $".{DateTime.Now:yyyyMMdd_HHmmss}.bak";
        if (File.Exists(DosyaYolu))
            File.Copy(DosyaYolu, yedek, overwrite: true);
        _notlar.Clear();
        return yedek;
    }
}
