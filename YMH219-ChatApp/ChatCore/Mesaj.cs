using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChatCore;

/// <summary>İstemci ile sunucu arasında gidip gelen mesajın türü.</summary>
public enum MesajTuru
{
    /// <summary>İstemci → Sunucu: kullanıcı adı ile bağlanma isteği.</summary>
    Baglan,
    /// <summary>Sunucu → İstemci: bağlantı kabul edildi.</summary>
    BaglantiKabul,
    /// <summary>Sunucu → İstemci: bağlantı reddedildi (ör. kullanıcı adı kullanımda).</summary>
    BaglantiRed,
    /// <summary>Herkese (odaya) gönderilen sohbet mesajı.</summary>
    Sohbet,
    /// <summary>Tek bir kullanıcıya gönderilen özel mesaj.</summary>
    Ozel,
    /// <summary>Sunucu → İstemci: çevrimiçi kullanıcı listesi.</summary>
    KullaniciListesi,
    /// <summary>Sunucu bilgilendirme mesajı (katıldı/ayrıldı/duyuru).</summary>
    Sistem,
    /// <summary>İstemci → Sunucu: ayrılma bildirimi.</summary>
    Ayril
}

/// <summary>
/// Ağ üzerinden gönderilen mesaj. Her mesaj tek satırlık JSON olarak yazılır (satır sonu = mesaj sonu).
/// </summary>
public class Mesaj
{
    private static readonly JsonSerializerOptions JsonAyarlari = new()
    {
        Converters = { new JsonStringEnumConverter() },
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public MesajTuru Tur { get; set; }
    public string Gonderen { get; set; } = string.Empty;
    public string? Alici { get; set; }
    public string Icerik { get; set; } = string.Empty;
    public DateTime Zaman { get; set; } = DateTime.Now;
    public List<string>? Kullanicilar { get; set; }

    public Mesaj() { }

    public Mesaj(MesajTuru tur, string gonderen, string icerik, string? alici = null)
    {
        Tur = tur;
        Gonderen = gonderen;
        Icerik = icerik;
        Alici = alici;
        Zaman = DateTime.Now;
    }

    /// <summary>Mesajı tek satırlık JSON metnine çevirir.</summary>
    public string SatiraCevir() => JsonSerializer.Serialize(this, JsonAyarlari);

    /// <summary>Gelen JSON satırını mesaja çevirir. Geçersiz satırda null döner.</summary>
    public static Mesaj? SatirdanOku(string? satir)
    {
        if (string.IsNullOrWhiteSpace(satir)) return null;
        try
        {
            return JsonSerializer.Deserialize<Mesaj>(satir, JsonAyarlari);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    /// <summary>Arayüzde ve geçmiş dosyasında kullanılan okunabilir biçim.</summary>
    public override string ToString() => Tur switch
    {
        MesajTuru.Ozel => $"[{Zaman:HH:mm:ss}] (Özel) {Gonderen} → {Alici}: {Icerik}",
        MesajTuru.Sistem => $"[{Zaman:HH:mm:ss}] * {Icerik}",
        _ => $"[{Zaman:HH:mm:ss}] {Gonderen}: {Icerik}"
    };
}
