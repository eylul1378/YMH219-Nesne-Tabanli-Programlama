using OpenCvSharp;
using OpenCvSharp.Face;

namespace FaceRecognition.Services;

/// <summary>Bir yüz için tanıma sonucu.</summary>
public record TanimaSonucu(string Etiket, double Mesafe, int GuvenYuzdesi, bool Taninmis)
{
    public override string ToString() =>
        Taninmis ? $"{Etiket}  (güven %{GuvenYuzdesi}, mesafe {Mesafe:0.0})"
                 : $"Bilinmiyor  (en yakın: {Etiket}, mesafe {Mesafe:0.0})";
}

/// <summary>
/// LBPH (Local Binary Patterns Histograms) algoritması ile yüz tanıma.
/// Her etikete (kişiye) bir sayı atanır, model örneklerle eğitilir ve dosyaya kaydedilebilir.
/// </summary>
public sealed class YuzTanimaModeli : IDisposable
{
    /// <summary>Bu mesafenin üzerindeki eşleşmeler "Bilinmiyor" kabul edilir (küçük = daha benzer).</summary>
    public double EsikMesafe { get; set; } = 85;

    private LBPHFaceRecognizer? _tanima;
    private readonly Dictionary<int, string> _etiketler = new();

    public bool Egitildi => _tanima != null;
    public IReadOnlyCollection<string> Etiketler => _etiketler.Values;

    /// <summary>
    /// Modeli verilen örneklerle eğitir. En az iki farklı etiket (sınıf) gerekir.
    /// </summary>
    public void Egit(IReadOnlyList<(Mat Goruntu, string Etiket)> ornekler)
    {
        var farkliEtiketler = ornekler.Select(o => o.Etiket).Distinct().OrderBy(e => e).ToList();
        if (farkliEtiketler.Count < 2)
            throw new InvalidOperationException(
                "Tanıma için en az iki farklı etiket (kişi veya sınıf) ve her birine ait örnekler gerekir.");

        _etiketler.Clear();
        for (int i = 0; i < farkliEtiketler.Count; i++)
            _etiketler[i] = farkliEtiketler[i];
        var numara = _etiketler.ToDictionary(x => x.Value, x => x.Key);

        _tanima?.Dispose();
        _tanima = LBPHFaceRecognizer.Create(radius: 1, neighbors: 8, gridX: 8, gridY: 8);
        _tanima.Train(ornekler.Select(o => o.Goruntu), ornekler.Select(o => numara[o.Etiket]));
    }

    /// <summary>Hazırlanmış (100x100 gri) yüz görüntüsü için tahmin yapar.</summary>
    public TanimaSonucu Tahmin(Mat yuz)
    {
        if (_tanima == null) throw new InvalidOperationException("Model henüz eğitilmedi.");

        _tanima.Predict(yuz, out int numara, out double mesafe);
        string etiket = _etiketler.TryGetValue(numara, out var e) ? e : "?";
        int guven = (int)Math.Clamp(100 - mesafe * 100 / (EsikMesafe * 1.6), 0, 100);
        return new TanimaSonucu(etiket, mesafe, guven, mesafe <= EsikMesafe);
    }

    /// <summary>Eğitilmiş modeli ve etiket listesini dosyaya kaydeder.</summary>
    public void Kaydet(string dosya)
    {
        if (_tanima == null) return;
        // Model, Türkçe karakterli yollarda da çalışması için bellekte yazılıp .NET ile kaydedilir.
        using (var depo = new FileStorage("model.yml", FileStorage.Modes.Write | FileStorage.Modes.Memory))
        {
            _tanima.Write(depo);
            File.WriteAllText(dosya, depo.ReleaseAndGetString());
        }
        File.WriteAllLines(dosya + ".etiketler.txt", _etiketler.OrderBy(x => x.Key).Select(x => $"{x.Key};{x.Value}"));
    }

    /// <summary>Kayıtlı modeli yükler. Dosya yoksa false döner.</summary>
    public bool Yukle(string dosya)
    {
        string etiketDosyasi = dosya + ".etiketler.txt";
        if (!File.Exists(dosya) || !File.Exists(etiketDosyasi)) return false;

        var tanima = LBPHFaceRecognizer.Create();
        using (var depo = new FileStorage(File.ReadAllText(dosya), FileStorage.Modes.Read | FileStorage.Modes.Memory))
        using (var dugum = depo.Root())
        {
            if (dugum == null) { tanima.Dispose(); return false; }
            tanima.Read(dugum);
        }

        _etiketler.Clear();
        foreach (string satir in File.ReadAllLines(etiketDosyasi))
        {
            var parca = satir.Split(';', 2);
            if (parca.Length == 2 && int.TryParse(parca[0], out int no)) _etiketler[no] = parca[1];
        }

        _tanima?.Dispose();
        _tanima = tanima;
        return true;
    }

    public void Dispose()
    {
        _tanima?.Dispose();
        _tanima = null;
    }
}
