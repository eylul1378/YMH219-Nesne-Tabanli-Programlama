using System.Text.Json;
using NAudio.Wave;
using Vosk;

namespace SpeechApp.Services;

/// <summary>
/// Vosk kütüphanesi ile çevrimdışı (internet gerektirmeyen) konuşma tanıma.
/// Mikrofon sesi NAudio ile 16 kHz / 16 bit / mono olarak alınır ve Vosk'a verilir.
/// </summary>
public sealed class VoskKonusmaTanima : IKonusmaTanima
{
    private const int OrneklemeHizi = 16000;

    private readonly string _modelKlasoru;
    private Model? _model;
    private VoskRecognizer? _tanima;
    private WaveInEvent? _mikrofon;
    private readonly object _kilit = new();

    public string Ad { get; }
    public bool Dinliyor { get; private set; }

    public event EventHandler<string>? MetinTanindi;
    public event EventHandler<string>? AraSonuc;
    public event EventHandler<int>? SesSeviyesi;
    public event EventHandler<string>? HataOlustu;

    public VoskKonusmaTanima(string ad, string modelKlasoru)
    {
        Ad = ad;
        _modelKlasoru = modelKlasoru;
    }

    public async Task BaslatAsync(int mikrofonNo)
    {
        if (Dinliyor) return;

        if (WaveInEvent.DeviceCount == 0)
            throw new KonusmaTanimaHatasi("Mikrofon bulunamadı. Bir mikrofon bağlı olduğundan emin olun.");

        // Model ilk kullanımda yüklenir (birkaç saniye sürebilir, arayüz donmasın diye Task.Run).
        if (_model == null)
        {
            if (!Directory.Exists(_modelKlasoru))
                throw new KonusmaTanimaHatasi($"Konuşma modeli bulunamadı:\n{_modelKlasoru}\n\"Modeli İndir\" butonunu kullanın.");
            try
            {
                Vosk.Vosk.SetLogLevel(-1);
                _model = await Task.Run(() => new Model(_modelKlasoru));
            }
            catch (Exception ex)
            {
                throw new KonusmaTanimaHatasi("Konuşma modeli yüklenemedi (dosyalar eksik veya bozuk olabilir).", ex);
            }
        }

        _tanima = new VoskRecognizer(_model, OrneklemeHizi);

        _mikrofon = new WaveInEvent
        {
            DeviceNumber = mikrofonNo,
            WaveFormat = new WaveFormat(OrneklemeHizi, 16, 1),
            BufferMilliseconds = 100
        };
        _mikrofon.DataAvailable += Mikrofon_DataAvailable;
        _mikrofon.RecordingStopped += Mikrofon_RecordingStopped;

        try
        {
            _mikrofon.StartRecording();
            Dinliyor = true;
        }
        catch (NAudio.MmException ex)
        {
            Temizle();
            throw new KonusmaTanimaHatasi(Mikrofon.HataAciklamasi(ex), ex);
        }
    }

    private void Mikrofon_DataAvailable(object? sender, WaveInEventArgs e)
    {
        // Bu olay NAudio'nun kayıt iş parçacığında çalışır.
        SesSeviyesi?.Invoke(this, SeviyeHesapla(e.Buffer, e.BytesRecorded));

        string? sonuc = null, ara = null;
        lock (_kilit)
        {
            if (_tanima == null) return;
            if (_tanima.AcceptWaveform(e.Buffer, e.BytesRecorded))
                sonuc = JsonAlan(_tanima.Result(), "text");
            else
                ara = JsonAlan(_tanima.PartialResult(), "partial");
        }

        if (!string.IsNullOrWhiteSpace(sonuc)) MetinTanindi?.Invoke(this, sonuc);
        else if (ara != null) AraSonuc?.Invoke(this, ara);
    }

    private void Mikrofon_RecordingStopped(object? sender, StoppedEventArgs e)
    {
        if (e.Exception != null)
            HataOlustu?.Invoke(this, "Mikrofon kaydı beklenmedik şekilde durdu: " + e.Exception.Message);
    }

    public void Durdur()
    {
        if (!Dinliyor) return;
        Dinliyor = false;
        _mikrofon?.StopRecording();

        // Son kalan sesi de metne çevir.
        string? son;
        lock (_kilit)
        {
            son = _tanima != null ? JsonAlan(_tanima.FinalResult(), "text") : null;
        }
        if (!string.IsNullOrWhiteSpace(son)) MetinTanindi?.Invoke(this, son);
        Temizle();
    }

    private void Temizle()
    {
        if (_mikrofon != null)
        {
            _mikrofon.DataAvailable -= Mikrofon_DataAvailable;
            _mikrofon.RecordingStopped -= Mikrofon_RecordingStopped;
            _mikrofon.Dispose();
            _mikrofon = null;
        }
        lock (_kilit)
        {
            _tanima?.Dispose();
            _tanima = null;
        }
    }

    private static string? JsonAlan(string json, string alan)
    {
        try
        {
            using var belge = JsonDocument.Parse(json);
            return belge.RootElement.TryGetProperty(alan, out var deger) ? deger.GetString() : null;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    /// <summary>16 bit PCM örneklerinden 0-100 arası ses seviyesi hesaplar.</summary>
    private static int SeviyeHesapla(byte[] tampon, int uzunluk)
    {
        int enBuyuk = 0;
        for (int i = 0; i + 1 < uzunluk; i += 2)
        {
            int ornek = Math.Abs((int)BitConverter.ToInt16(tampon, i));
            if (ornek > enBuyuk) enBuyuk = ornek;
        }
        return Math.Min(100, enBuyuk * 100 / short.MaxValue);
    }

    public void Dispose()
    {
        Dinliyor = false;
        Temizle();
        _model?.Dispose();
        _model = null;
    }

    public override string ToString() => Ad;
}
