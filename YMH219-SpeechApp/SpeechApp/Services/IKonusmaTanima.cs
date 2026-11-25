namespace SpeechApp.Services;

/// <summary>
/// Speech-to-Text motorları için ortak arayüz. Farklı tanıma altyapıları
/// (Vosk, System.Speech) aynı şekilde kullanılabilir.
/// </summary>
public interface IKonusmaTanima : IDisposable
{
    /// <summary>Arayüzde gösterilecek motor adı.</summary>
    string Ad { get; }

    bool Dinliyor { get; }

    /// <summary>Kesinleşmiş tanıma sonucu (bir cümle / ifade).</summary>
    event EventHandler<string>? MetinTanindi;

    /// <summary>Konuşma devam ederken oluşan ara sonuç.</summary>
    event EventHandler<string>? AraSonuc;

    /// <summary>Mikrofon ses seviyesi (0-100).</summary>
    event EventHandler<int>? SesSeviyesi;

    /// <summary>Dinleme sırasında oluşan hata (ör. mikrofon çıkarıldı).</summary>
    event EventHandler<string>? HataOlustu;

    /// <summary>Mikrofondan dinlemeyi başlatır. Erişilemezse <see cref="KonusmaTanimaHatasi"/> fırlatır.</summary>
    Task BaslatAsync(int mikrofonNo);

    void Durdur();
}

/// <summary>Kullanıcıya gösterilebilecek açıklamaya sahip konuşma tanıma hatası.</summary>
public class KonusmaTanimaHatasi : Exception
{
    public KonusmaTanimaHatasi(string mesaj, Exception? ic = null) : base(mesaj, ic) { }
}
