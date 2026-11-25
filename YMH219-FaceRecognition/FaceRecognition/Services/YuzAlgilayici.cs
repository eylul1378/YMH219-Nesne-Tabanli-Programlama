using OpenCvSharp;
using Size = OpenCvSharp.Size;

namespace FaceRecognition.Services;

/// <summary>
/// OpenCV Haar Cascade sınıflandırıcısı ile görüntüdeki yüz bölgelerini bulur.
/// </summary>
public sealed class YuzAlgilayici : IDisposable
{
    public const string CascadeDosyaAdi = "haarcascade_frontalface_default.xml";

    private readonly CascadeClassifier _siniflandirici;

    public YuzAlgilayici(string cascadeYolu)
    {
        if (!File.Exists(cascadeYolu))
            throw new FileNotFoundException("Yüz algılama model dosyası (Haar cascade) bulunamadı.", cascadeYolu);

        // Not: OpenCV'nin yerel kodu Türkçe karakter (ör. "EYLÜL") içeren yolları açamaz.
        // Bu yüzden XML dosyası .NET ile okunup OpenCV'ye bellekten verilir.
        _siniflandirici = new CascadeClassifier();
        try
        {
            string xml = File.ReadAllText(cascadeYolu);
            using var depo = new FileStorage(xml, FileStorage.Modes.Read | FileStorage.Modes.Memory);
            using var dugum = depo.GetFirstTopLevelNode();
            if (dugum != null) _siniflandirici.Read(dugum);
        }
        catch (OpenCVException)
        {
            // aşağıdaki Empty() kontrolü hatayı bildirir
        }
        if (_siniflandirici.Empty())
        {
            _siniflandirici.Dispose();
            throw new InvalidDataException("Haar cascade dosyası okunamadı veya bozuk: " + cascadeYolu);
        }
    }

    /// <summary>Varsayılan konum: uygulama klasöründeki Cascades\haarcascade_frontalface_default.xml</summary>
    public static string VarsayilanCascadeYolu() =>
        Path.Combine(AppContext.BaseDirectory, "Cascades", CascadeDosyaAdi);

    /// <summary>
    /// Renkli görüntüdeki yüzleri bulur. Aynı karede birden fazla yüz döndürebilir.
    /// </summary>
    public Rect[] Algila(Mat goruntu)
    {
        using var gri = GriTonla(goruntu);
        Cv2.EqualizeHist(gri, gri);
        int enKucuk = Math.Max(40, Math.Min(gri.Width, gri.Height) / 10);
        return _siniflandirici.DetectMultiScale(gri, scaleFactor: 1.1, minNeighbors: 7,
            flags: HaarDetectionTypes.ScaleImage, minSize: new Size(enKucuk, enKucuk));
    }

    /// <summary>Tanıma için yüz bölgesini kesip 100x100 gri, histogramı eşitlenmiş görüntüye çevirir.</summary>
    public static Mat YuzuHazirla(Mat goruntu, Rect yuz)
    {
        var sinir = yuz.Intersect(new Rect(0, 0, goruntu.Width, goruntu.Height));
        using var kesit = new Mat(goruntu, sinir);
        using var gri = GriTonla(kesit);
        var sonuc = new Mat();
        Cv2.Resize(gri, sonuc, new Size(100, 100));
        Cv2.EqualizeHist(sonuc, sonuc);
        return sonuc;
    }

    private static Mat GriTonla(Mat goruntu)
    {
        if (goruntu.Channels() == 1) return goruntu.Clone();
        var gri = new Mat();
        Cv2.CvtColor(goruntu, gri, goruntu.Channels() == 4 ? ColorConversionCodes.BGRA2GRAY : ColorConversionCodes.BGR2GRAY);
        return gri;
    }

    public void Dispose() => _siniflandirici.Dispose();
}
