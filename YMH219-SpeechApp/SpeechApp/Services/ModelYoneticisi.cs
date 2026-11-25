using System.IO.Compression;

namespace SpeechApp.Services;

/// <summary>İndirilebilir bir Vosk konuşma modeli.</summary>
public sealed record VoskModeli(string Ad, string KlasorAdi, string Adres, int YaklasikBoyutMb)
{
    public override string ToString() => $"{Ad} (~{YaklasikBoyutMb} MB)";
}

/// <summary>
/// Vosk konuşma modellerini bulur ve gerektiğinde resmi Vosk sitesinden indirip açar.
/// Modeller %LocalAppData%\YMH219-SpeechApp\Models klasöründe saklanır (depoya eklenmez).
/// </summary>
public static class ModelYoneticisi
{
    public static readonly VoskModeli[] Modeller =
    {
        new("Türkçe (küçük model)", "vosk-model-small-tr-0.3",
            "https://alphacephei.com/vosk/models/vosk-model-small-tr-0.3.zip", 35),
        new("İngilizce (küçük model)", "vosk-model-small-en-us-0.15",
            "https://alphacephei.com/vosk/models/vosk-model-small-en-us-0.15.zip", 40)
    };

    public static string ModelKlasoru { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "YMH219-SpeechApp", "Models");

    /// <summary>Modelin yüklü olduğu klasör; yoksa null. Uygulama klasöründeki "Models" da aranır.</summary>
    public static string? ModelYolu(VoskModeli model)
    {
        string[] adaylar =
        {
            Path.Combine(ModelKlasoru, model.KlasorAdi),
            Path.Combine(AppContext.BaseDirectory, "Models", model.KlasorAdi)
        };
        // Model yapısı sürüme göre değişebilir: final.mdl ana klasörde veya "am" alt klasöründe olabilir.
        return adaylar.FirstOrDefault(k =>
            File.Exists(Path.Combine(k, "final.mdl")) || File.Exists(Path.Combine(k, "am", "final.mdl")));
    }

    /// <summary>Modeli indirir ve açar. İlerleme 0-100 arası bildirilir.</summary>
    public static async Task IndirAsync(VoskModeli model, IProgress<int> ilerleme, CancellationToken iptal)
    {
        Directory.CreateDirectory(ModelKlasoru);
        string zipYolu = Path.Combine(ModelKlasoru, model.KlasorAdi + ".zip");

        try
        {
            using var http = new HttpClient { Timeout = TimeSpan.FromMinutes(10) };
            using var cevap = await http.GetAsync(model.Adres, HttpCompletionOption.ResponseHeadersRead, iptal);
            cevap.EnsureSuccessStatusCode();

            long? toplam = cevap.Content.Headers.ContentLength;
            await using (var kaynak = await cevap.Content.ReadAsStreamAsync(iptal))
            await using (var hedef = File.Create(zipYolu))
            {
                var tampon = new byte[81920];
                long okunan = 0;
                int n;
                while ((n = await kaynak.ReadAsync(tampon, iptal)) > 0)
                {
                    await hedef.WriteAsync(tampon.AsMemory(0, n), iptal);
                    okunan += n;
                    if (toplam > 0) ilerleme.Report((int)(okunan * 95 / toplam.Value));
                }
            }

            await Task.Run(() => ZipFile.ExtractToDirectory(zipYolu, ModelKlasoru, overwriteFiles: true), iptal);
            ilerleme.Report(100);
        }
        finally
        {
            if (File.Exists(zipYolu)) File.Delete(zipYolu);
        }

        if (ModelYolu(model) == null)
            throw new InvalidDataException("İndirilen arşivde beklenen model dosyaları bulunamadı.");
    }
}
