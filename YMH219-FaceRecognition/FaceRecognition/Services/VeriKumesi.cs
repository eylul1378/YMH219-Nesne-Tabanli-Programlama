using OpenCvSharp;
using Size = OpenCvSharp.Size;

namespace FaceRecognition.Services;

/// <summary>
/// Gönüllü yerel yüz örneklerini klasör yapısında saklar:
/// %LocalAppData%\YMH219-FaceRecognition\Ornekler\&lt;Etiket&gt;\*.png
/// </summary>
public class VeriKumesi
{
    public string AnaKlasor { get; }
    public string OrnekKlasoru => Path.Combine(AnaKlasor, "Ornekler");
    public string AlgilananKlasoru => Path.Combine(AnaKlasor, "AlgilananYuzler");
    public string ModelDosyasi => Path.Combine(AnaKlasor, "lbph_model.yml");

    public VeriKumesi(string? anaKlasor = null)
    {
        AnaKlasor = anaKlasor ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "YMH219-FaceRecognition");
        Directory.CreateDirectory(OrnekKlasoru);
    }

    /// <summary>Etiket adını klasör adı olarak kullanılabilir hâle getirir.</summary>
    public static string EtiketiTemizle(string etiket)
    {
        string temiz = etiket.Trim();
        foreach (char c in Path.GetInvalidFileNameChars())
            temiz = temiz.Replace(c, '_');
        return temiz;
    }

    /// <summary>Hazırlanmış (100x100 gri) yüz örneğini etiketin klasörüne kaydeder.</summary>
    public string OrnekEkle(string etiket, Mat yuz)
    {
        string ad = EtiketiTemizle(etiket);
        if (ad.Length == 0) throw new ArgumentException("Etiket (kişi adı) boş olamaz.");

        string klasor = Path.Combine(OrnekKlasoru, ad);
        Directory.CreateDirectory(klasor);
        string dosya = Path.Combine(klasor, $"{DateTime.Now:yyyyMMdd_HHmmss_fff}.png");
        GoruntuDosyasi.Yaz(dosya, yuz);
        return dosya;
    }

    /// <summary>Etiketleri ve her etiketteki örnek sayısını döndürür.</summary>
    public Dictionary<string, int> EtiketSayilari() =>
        Directory.GetDirectories(OrnekKlasoru)
            .ToDictionary(k => Path.GetFileName(k), k => Directory.GetFiles(k, "*.png").Length)
            .Where(x => x.Value > 0)
            .OrderBy(x => x.Key)
            .ToDictionary(x => x.Key, x => x.Value);

    /// <summary>Tüm örnekleri (gri görüntü, etiket adı) olarak yükler. Bozuk dosyalar atlanır.</summary>
    public List<(Mat Goruntu, string Etiket)> TumOrnekler()
    {
        var liste = new List<(Mat, string)>();
        foreach (string klasor in Directory.GetDirectories(OrnekKlasoru))
        {
            string etiket = Path.GetFileName(klasor);
            foreach (string dosya in Directory.GetFiles(klasor, "*.png"))
            {
                var mat = GoruntuDosyasi.Oku(dosya, ImreadModes.Grayscale);
                if (mat == null) continue;
                if (mat.Width != 100 || mat.Height != 100) Cv2.Resize(mat, mat, new Size(100, 100));
                liste.Add((mat, etiket));
            }
        }
        return liste;
    }

    public void EtiketiSil(string etiket)
    {
        string klasor = Path.Combine(OrnekKlasoru, EtiketiTemizle(etiket));
        if (Directory.Exists(klasor)) Directory.Delete(klasor, recursive: true);
    }

    /// <summary>İsteğe bağlı: algılanan yüzü ayrı klasöre kaydeder.</summary>
    public void AlgilananYuzuKaydet(Mat yuzRenkli)
    {
        Directory.CreateDirectory(AlgilananKlasoru);
        GoruntuDosyasi.Yaz(Path.Combine(AlgilananKlasoru, $"yuz_{DateTime.Now:yyyyMMdd_HHmmss_fff}.png"), yuzRenkli);
    }
}
