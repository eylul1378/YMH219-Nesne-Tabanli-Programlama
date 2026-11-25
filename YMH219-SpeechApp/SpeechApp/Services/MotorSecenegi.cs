namespace SpeechApp.Services;

/// <summary>
/// Tanıma motoru ComboBox'ında bir seçenek: ya kullanıma hazır bir motor
/// ya da henüz indirilmemiş bir Vosk modeli.
/// </summary>
public sealed class MotorSecenegi
{
    public IKonusmaTanima? Motor { get; }
    public VoskModeli? IndirilecekModel { get; }

    public bool Hazir => Motor != null;

    private MotorSecenegi(IKonusmaTanima? motor, VoskModeli? model)
    {
        Motor = motor;
        IndirilecekModel = model;
    }

    /// <summary>Yüklü Vosk modellerinden ve Windows tanıyıcılarından seçenek listesi oluşturur.</summary>
    public static List<MotorSecenegi> Listele()
    {
        var liste = new List<MotorSecenegi>();

        foreach (var model in ModelYoneticisi.Modeller)
        {
            string? yol = ModelYoneticisi.ModelYolu(model);
            liste.Add(yol != null
                ? new MotorSecenegi(new VoskKonusmaTanima($"Vosk çevrimdışı - {model.Ad}", yol), null)
                : new MotorSecenegi(null, model));
        }

        foreach (var sistem in SistemKonusmaTanima.YukluMotorlar())
            liste.Add(new MotorSecenegi(sistem, null));

        // Hazır motorlar listenin başında olsun.
        return liste.OrderByDescending(s => s.Hazir).ToList();
    }

    public override string ToString() =>
        Motor?.Ad ?? $"Vosk - {IndirilecekModel!.Ad} (indirilmedi)";
}
