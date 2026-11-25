using System.Speech.Recognition;

namespace SpeechApp.Services;

/// <summary>
/// Windows'un yerleşik konuşma tanıma motoru (System.Speech) ile dikte.
/// Yalnızca Windows'ta ilgili dilin konuşma tanıma paketi yüklüyse kullanılabilir
/// (örneğin İngilizce Windows'larda en-US genellikle yüklüdür).
/// </summary>
public sealed class SistemKonusmaTanima : IKonusmaTanima
{
    private readonly RecognizerInfo _bilgi;
    private SpeechRecognitionEngine? _motor;

    public string Ad => $"Windows Konuşma Tanıma ({_bilgi.Culture.Name})";
    public bool Dinliyor { get; private set; }

    public event EventHandler<string>? MetinTanindi;
    public event EventHandler<string>? AraSonuc;
    public event EventHandler<int>? SesSeviyesi;
    public event EventHandler<string>? HataOlustu;

    private SistemKonusmaTanima(RecognizerInfo bilgi) => _bilgi = bilgi;

    /// <summary>Bilgisayarda yüklü System.Speech tanıyıcılarını döndürür.</summary>
    public static List<SistemKonusmaTanima> YukluMotorlar()
    {
        try
        {
            return SpeechRecognitionEngine.InstalledRecognizers()
                .Select(r => new SistemKonusmaTanima(r))
                .ToList();
        }
        catch (Exception)
        {
            return new List<SistemKonusmaTanima>();
        }
    }

    public Task BaslatAsync(int mikrofonNo)
    {
        if (Dinliyor) return Task.CompletedTask;

        try
        {
            _motor = new SpeechRecognitionEngine(_bilgi);
            _motor.LoadGrammar(new DictationGrammar());
            // System.Speech yalnızca Windows'un varsayılan kayıt aygıtını kullanabilir.
            _motor.SetInputToDefaultAudioDevice();
        }
        catch (InvalidOperationException ex)
        {
            Temizle();
            throw new KonusmaTanimaHatasi("Varsayılan mikrofona erişilemedi. Bir mikrofon bağlı ve Windows'ta " +
                                          "varsayılan kayıt aygıtı olarak seçili olmalıdır.", ex);
        }

        _motor.SpeechRecognized += (_, e) => MetinTanindi?.Invoke(this, e.Result.Text);
        _motor.SpeechHypothesized += (_, e) => AraSonuc?.Invoke(this, e.Result.Text);
        _motor.AudioLevelUpdated += (_, e) => SesSeviyesi?.Invoke(this, e.AudioLevel);
        _motor.RecognizeCompleted += (_, e) =>
        {
            if (e.Error != null) HataOlustu?.Invoke(this, "Konuşma tanıma hatası: " + e.Error.Message);
        };

        _motor.RecognizeAsync(RecognizeMode.Multiple);
        Dinliyor = true;
        return Task.CompletedTask;
    }

    public void Durdur()
    {
        if (!Dinliyor) return;
        Dinliyor = false;
        _motor?.RecognizeAsyncCancel();
        Temizle();
    }

    private void Temizle()
    {
        _motor?.Dispose();
        _motor = null;
    }

    public void Dispose()
    {
        Dinliyor = false;
        Temizle();
    }

    public override string ToString() => Ad;
}
