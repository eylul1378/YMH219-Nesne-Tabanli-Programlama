using System.Text;
using NotepadApp.Models;

namespace NotepadApp.Services;

/// <summary>
/// Bir notu düz metin (.txt) dosyası olarak dışa aktarır.
/// </summary>
public static class TxtDisaAktarici
{
    public static void DisaAktar(Not not, string dosyaYolu)
    {
        ArgumentNullException.ThrowIfNull(not);

        using var yazici = new StreamWriter(dosyaYolu, false, Encoding.UTF8);
        yazici.WriteLine($"Başlık    : {not.Baslik}");
        yazici.WriteLine($"Kategori  : {not.Kategori}");
        yazici.WriteLine($"Oluşturma : {not.OlusturmaTarihi:dd.MM.yyyy HH:mm:ss}");
        yazici.WriteLine($"Düzenleme : {not.DuzenlemeTarihi:dd.MM.yyyy HH:mm:ss}");
        yazici.WriteLine(new string('-', 50));
        yazici.WriteLine(not.Icerik);
    }

    /// <summary>Dosya adında kullanılamayacak karakterleri temizleyerek öneri dosya adı üretir.</summary>
    public static string OneriDosyaAdi(Not not)
    {
        string ad = string.IsNullOrWhiteSpace(not.Baslik) ? "not" : not.Baslik;
        foreach (char c in Path.GetInvalidFileNameChars())
            ad = ad.Replace(c, '_');
        return ad + ".txt";
    }
}
