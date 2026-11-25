using OpenCvSharp;

namespace FaceRecognition.Services;

/// <summary>
/// Görüntü dosyalarını .NET ile okuyup/yazarak OpenCV'ye bellekten verir.
/// Cv2.ImRead/ImWrite, Türkçe karakter içeren yollarda (ör. C:\Users\EYLÜL\...) çalışmadığı için gereklidir.
/// </summary>
public static class GoruntuDosyasi
{
    /// <summary>Görüntüyü okur; dosya geçerli bir resim değilse null döner.</summary>
    public static Mat? Oku(string dosya, ImreadModes mod = ImreadModes.Color)
    {
        byte[] veri = File.ReadAllBytes(dosya);
        var mat = Cv2.ImDecode(veri, mod);
        if (mat.Empty())
        {
            mat.Dispose();
            return null;
        }
        return mat;
    }

    public static void Yaz(string dosya, Mat goruntu)
    {
        string uzanti = Path.GetExtension(dosya);
        if (!Cv2.ImEncode(string.IsNullOrEmpty(uzanti) ? ".png" : uzanti, goruntu, out byte[] veri))
            throw new IOException("Görüntü kodlanamadı: " + dosya);
        File.WriteAllBytes(dosya, veri);
    }
}
