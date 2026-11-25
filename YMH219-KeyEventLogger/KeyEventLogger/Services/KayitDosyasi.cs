using System.Text;
using KeyEventLogger.Models;

namespace KeyEventLogger.Services;

/// <summary>
/// Kayıtları kullanıcının seçtiği yerel bir .txt dosyasına yazar.
/// (Ağ üzerinden gönderme veya gizli dosyaya yazma yapılmaz.)
/// </summary>
public static class KayitDosyasi
{
    public static void TxtOlarakKaydet(IEnumerable<TusKaydi> kayitlar, string dosyaYolu)
    {
        ArgumentNullException.ThrowIfNull(kayitlar);
        var liste = kayitlar.ToList();

        using var yazici = new StreamWriter(dosyaYolu, false, Encoding.UTF8);
        yazici.WriteLine("YMH219 Key Event Logger - Kayıt Dosyası");
        yazici.WriteLine($"Oluşturulma : {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
        yazici.WriteLine($"Kayıt sayısı: {liste.Count}");
        yazici.WriteLine(new string('-', 60));
        yazici.WriteLine($"{"Tarih / Saat",-23}  {"Olay",-8}  Tuş");
        yazici.WriteLine(new string('-', 60));
        foreach (var kayit in liste)
            yazici.WriteLine(kayit.ToString());
    }
}
