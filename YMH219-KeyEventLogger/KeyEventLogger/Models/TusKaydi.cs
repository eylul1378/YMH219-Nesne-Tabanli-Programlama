namespace KeyEventLogger.Models;

/// <summary>Kaydedilen klavye olayının türü.</summary>
public enum OlayTuru
{
    KeyDown,
    KeyUp,
    KeyPress
}

/// <summary>
/// Tek bir klavye olayını (tuş adı, olay türü, zaman, değiştirici tuşlar) temsil eder.
/// </summary>
public class TusKaydi
{
    public DateTime Zaman { get; }
    public OlayTuru Tur { get; }

    /// <summary>Tuşun adı (ör. "A", "Enter", "Space", "F5").</summary>
    public string TusAdi { get; }

    /// <summary>Basılı değiştirici tuşlar (Ctrl, Shift, Alt).</summary>
    public Keys Degistiriciler { get; }

    /// <summary>KeyPress olayında üretilen karakter (diğer olaylarda null).</summary>
    public char? Karakter { get; }

    public TusKaydi(DateTime zaman, OlayTuru tur, string tusAdi, Keys degistiriciler = Keys.None, char? karakter = null)
    {
        Zaman = zaman;
        Tur = tur;
        TusAdi = tusAdi;
        Degistiriciler = degistiriciler;
        Karakter = karakter;
    }

    /// <summary>KeyDown / KeyUp olayından kayıt oluşturur.</summary>
    public static TusKaydi TusOlayindan(OlayTuru tur, KeyEventArgs e) =>
        new(DateTime.Now, tur, TusAdiniBul(e.KeyCode), e.Modifiers);

    /// <summary>KeyPress olayından kayıt oluşturur.</summary>
    public static TusKaydi KarakterOlayindan(KeyPressEventArgs e) =>
        new(DateTime.Now, OlayTuru.KeyPress, KarakterAdi(e.KeyChar), Keys.None, e.KeyChar);

    private static string TusAdiniBul(Keys tus) => tus switch
    {
        >= Keys.D0 and <= Keys.D9 => ((int)(tus - Keys.D0)).ToString(),
        Keys.Return => "Enter",
        Keys.Back => "Backspace",
        Keys.Capital => "CapsLock",
        Keys.Next => "PageDown",
        Keys.Prior => "PageUp",
        Keys.ShiftKey or Keys.LShiftKey or Keys.RShiftKey => "Shift",
        Keys.ControlKey or Keys.LControlKey or Keys.RControlKey => "Ctrl",
        Keys.Menu or Keys.LMenu or Keys.RMenu => "Alt",
        _ => tus.ToString()
    };

    private static string KarakterAdi(char c) => c switch
    {
        ' ' => "Space",
        '\r' => "Enter",
        '\b' => "Backspace",
        '\t' => "Tab",
        (char)27 => "Esc",
        _ when char.IsControl(c) => $"Kontrol(0x{(int)c:X2})",
        _ => c.ToString()
    };

    /// <summary>Değiştirici tuşları okunabilir metne çevirir: "Ctrl+Shift".</summary>
    public string DegistiriciMetni()
    {
        var parcalar = new List<string>();
        if (Degistiriciler.HasFlag(Keys.Control)) parcalar.Add("Ctrl");
        if (Degistiriciler.HasFlag(Keys.Shift)) parcalar.Add("Shift");
        if (Degistiriciler.HasFlag(Keys.Alt)) parcalar.Add("Alt");
        return string.Join("+", parcalar);
    }

    /// <summary>Ekranda ve dosyada kullanılan kayıt satırı biçimi.</summary>
    public override string ToString()
    {
        string degistirici = DegistiriciMetni();
        string ek = string.IsNullOrEmpty(degistirici) ? "" : $"  [{degistirici}]";
        return $"{Zaman:dd.MM.yyyy HH:mm:ss.fff}  {Tur,-8}  {TusAdi}{ek}";
    }
}
