using System.Media;

namespace FlappyBird.Oyun;

/// <summary>
/// Harici ses dosyası kullanmadan, bellekte üretilen kısa WAV sesleri ile efekt çalar.
/// </summary>
public sealed class SesEfektleri : IDisposable
{
    private readonly SoundPlayer _zipla;
    private readonly SoundPlayer _puan;
    private readonly SoundPlayer _carpma;

    public bool Acik { get; set; } = true;

    public SesEfektleri()
    {
        _zipla = new SoundPlayer(TonUret(new[] { (660.0, 0.05), (880.0, 0.04) }));
        _puan = new SoundPlayer(TonUret(new[] { (988.0, 0.06), (1319.0, 0.10) }));
        _carpma = new SoundPlayer(TonUret(new[] { (220.0, 0.12), (150.0, 0.20) }));
        _zipla.Load();
        _puan.Load();
        _carpma.Load();
    }

    public void Zipla() => Cal(_zipla);
    public void Puan() => Cal(_puan);
    public void Carpma() => Cal(_carpma);

    private void Cal(SoundPlayer oynatici)
    {
        if (!Acik) return;
        try { oynatici.Play(); }
        catch (Exception) { /* Ses aygıtı yoksa oyun sessiz devam eder. */ }
    }

    /// <summary>Verilen (frekans, süre) dizisinden 16 bit mono PCM WAV akışı üretir.</summary>
    private static MemoryStream TonUret((double Frekans, double Sure)[] tonlar)
    {
        const int ornekleme = 22050;
        var ornekler = new List<short>();
        foreach (var (frekans, sure) in tonlar)
        {
            int adet = (int)(ornekleme * sure);
            for (int i = 0; i < adet; i++)
            {
                double zarf = 1.0 - (double)i / adet; // sesin sönümlenmesi
                double deger = Math.Sin(2 * Math.PI * frekans * i / ornekleme) * zarf * 0.35;
                ornekler.Add((short)(deger * short.MaxValue));
            }
        }

        var akis = new MemoryStream();
        using (var w = new BinaryWriter(akis, System.Text.Encoding.ASCII, leaveOpen: true))
        {
            int veriBoyutu = ornekler.Count * 2;
            w.Write("RIFF"u8.ToArray());
            w.Write(36 + veriBoyutu);
            w.Write("WAVE"u8.ToArray());
            w.Write("fmt "u8.ToArray());
            w.Write(16);              // fmt bölüm boyutu
            w.Write((short)1);        // PCM
            w.Write((short)1);        // mono
            w.Write(ornekleme);
            w.Write(ornekleme * 2);   // byte/sn
            w.Write((short)2);        // blok hizalama
            w.Write((short)16);       // bit/örnek
            w.Write("data"u8.ToArray());
            w.Write(veriBoyutu);
            foreach (short s in ornekler) w.Write(s);
        }
        akis.Position = 0;
        return akis;
    }

    public void Dispose()
    {
        _zipla.Dispose();
        _puan.Dispose();
        _carpma.Dispose();
    }
}
