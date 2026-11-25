using NAudio.Wave;
using WinSes = Windows.Media.SpeechSynthesis;

namespace SpeechApp.Services;

public enum SeslendirmeDurumu
{
    Hazir,
    Hazirlaniyor,
    Okuyor,
    Duraklatildi
}

/// <summary>Arayüzde gösterilecek ses bilgisi.</summary>
public sealed class SesSecenegi
{
    internal WinSes.VoiceInformation Bilgi { get; }
    public string Ad => Bilgi.DisplayName;
    public string Dil => Bilgi.Language;

    internal SesSecenegi(WinSes.VoiceInformation bilgi) => Bilgi = bilgi;

    public override string ToString() => $"{Ad} ({Dil})";
}

/// <summary>
/// Text-to-Speech: Windows'un konuşma sentezleyicisi (Windows.Media.SpeechSynthesis) ile metni
/// WAV sesine çevirir ve NAudio ile çalar. Başlat, Duraklat/Devam ve Durdur işlemlerini destekler.
/// </summary>
public sealed class MetinSeslendirici : IDisposable
{
    private WaveOutEvent? _cikis;
    private WaveFileReader? _okuyucu;
    private MemoryStream? _sesVerisi;
    private bool _durduruldu;

    public SeslendirmeDurumu Durum { get; private set; } = SeslendirmeDurumu.Hazir;

    /// <summary>Durum değiştiğinde tetiklenir (arayüz göstergesi için).</summary>
    public event EventHandler? DurumDegisti;

    /// <summary>Okuma sırasında oluşan hata (ör. ses çıkış aygıtı yok).</summary>
    public event EventHandler<string>? HataOlustu;

    /// <summary>Okunan sesin toplam süresi ve şu anki konumu.</summary>
    public TimeSpan ToplamSure => _okuyucu?.TotalTime ?? TimeSpan.Zero;
    public TimeSpan Konum => _okuyucu?.CurrentTime ?? TimeSpan.Zero;

    /// <summary>Bilgisayarda yüklü konuşma seslerini döndürür (ör. Microsoft Tolga - tr-TR).</summary>
    public static List<SesSecenegi> YukluSesler()
    {
        try
        {
            return WinSes.SpeechSynthesizer.AllVoices
                .Select(v => new SesSecenegi(v))
                .OrderByDescending(s => s.Dil.StartsWith("tr", StringComparison.OrdinalIgnoreCase))
                .ThenBy(s => s.Ad)
                .ToList();
        }
        catch (Exception)
        {
            return new List<SesSecenegi>();
        }
    }

    /// <summary>Metni seslendirmeye başlar.</summary>
    /// <param name="hiz">0.5 (yavaş) – 2.0 (hızlı), 1.0 normal.</param>
    /// <param name="sesSeviyesi">0.0 – 1.0</param>
    public async Task OkuAsync(string metin, SesSecenegi? ses, double hiz, double sesSeviyesi)
    {
        if (string.IsNullOrWhiteSpace(metin))
            throw new ArgumentException("Okunacak metin boş.");

        Durdur();
        DurumAyarla(SeslendirmeDurumu.Hazirlaniyor);

        try
        {
            _sesVerisi = await SentezleAsync(metin, ses, hiz, sesSeviyesi);
            _okuyucu = new WaveFileReader(_sesVerisi);
            _cikis = new WaveOutEvent();
            _cikis.PlaybackStopped += Cikis_PlaybackStopped;
            _cikis.Init(_okuyucu);
            _durduruldu = false;
            _cikis.Play();
            DurumAyarla(SeslendirmeDurumu.Okuyor);
        }
        catch (NAudio.MmException ex)
        {
            Temizle();
            DurumAyarla(SeslendirmeDurumu.Hazir);
            throw new InvalidOperationException("Ses çıkış aygıtına erişilemedi (hoparlör/kulaklık bağlı mı?). " + ex.Message, ex);
        }
        catch
        {
            Temizle();
            DurumAyarla(SeslendirmeDurumu.Hazir);
            throw;
        }
    }

    /// <summary>Metni çalmadan .wav dosyasına kaydeder.</summary>
    public async Task WavOlarakKaydetAsync(string metin, SesSecenegi? ses, double hiz, string dosyaYolu)
    {
        using var veri = await SentezleAsync(metin, ses, hiz, 1.0);
        await using var dosya = File.Create(dosyaYolu);
        await veri.CopyToAsync(dosya);
    }

    private static async Task<MemoryStream> SentezleAsync(string metin, SesSecenegi? ses, double hiz, double sesSeviyesi)
    {
        using var sentezleyici = new WinSes.SpeechSynthesizer();
        if (ses != null) sentezleyici.Voice = ses.Bilgi;
        sentezleyici.Options.SpeakingRate = Math.Clamp(hiz, 0.5, 6.0);
        sentezleyici.Options.AudioVolume = Math.Clamp(sesSeviyesi, 0.0, 1.0);

        using var akis = await sentezleyici.SynthesizeTextToStreamAsync(metin);
        var bellek = new MemoryStream();
        await using (var netAkis = akis.AsStreamForRead())
        {
            await netAkis.CopyToAsync(bellek);
        }
        bellek.Position = 0;
        return bellek;
    }

    public void Duraklat()
    {
        if (Durum != SeslendirmeDurumu.Okuyor || _cikis == null) return;
        _cikis.Pause();
        DurumAyarla(SeslendirmeDurumu.Duraklatildi);
    }

    public void DevamEt()
    {
        if (Durum != SeslendirmeDurumu.Duraklatildi || _cikis == null) return;
        _cikis.Play();
        DurumAyarla(SeslendirmeDurumu.Okuyor);
    }

    public void Durdur()
    {
        if (_cikis == null) return;
        _durduruldu = true;
        _cikis.Stop(); // PlaybackStopped olayı kaynakları temizler
        Temizle();
        DurumAyarla(SeslendirmeDurumu.Hazir);
    }

    private void Cikis_PlaybackStopped(object? sender, StoppedEventArgs e)
    {
        if (e.Exception != null)
            HataOlustu?.Invoke(this, "Ses çalınırken hata oluştu: " + e.Exception.Message);

        if (_durduruldu) return; // Durdur() zaten temizledi
        Temizle();
        DurumAyarla(SeslendirmeDurumu.Hazir);
    }

    private void Temizle()
    {
        if (_cikis != null)
        {
            _cikis.PlaybackStopped -= Cikis_PlaybackStopped;
            _cikis.Dispose();
            _cikis = null;
        }
        _okuyucu?.Dispose();
        _okuyucu = null;
        _sesVerisi?.Dispose();
        _sesVerisi = null;
    }

    private void DurumAyarla(SeslendirmeDurumu durum)
    {
        if (Durum == durum) return;
        Durum = durum;
        DurumDegisti?.Invoke(this, EventArgs.Empty);
    }

    public void Dispose()
    {
        _durduruldu = true;
        Temizle();
    }
}
