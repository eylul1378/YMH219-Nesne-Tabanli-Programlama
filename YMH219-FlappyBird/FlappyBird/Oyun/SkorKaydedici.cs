namespace FlappyBird.Oyun;

/// <summary>
/// En yüksek skoru %AppData%\YMH219-FlappyBird\enyuksekskor.txt dosyasında saklar.
/// </summary>
public class SkorKaydedici
{
    private readonly string _dosyaYolu;

    public SkorKaydedici()
    {
        string klasor = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YMH219-FlappyBird");
        _dosyaYolu = Path.Combine(klasor, "enyuksekskor.txt");
    }

    /// <summary>Kayıtlı en yüksek skoru okur; dosya yoksa veya bozuksa 0 döner.</summary>
    public int Oku()
    {
        try
        {
            if (!File.Exists(_dosyaYolu)) return 0;
            string metin = File.ReadAllText(_dosyaYolu).Trim();
            return int.TryParse(metin, out int skor) && skor >= 0 ? skor : 0;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return 0;
        }
    }

    /// <summary>En yüksek skoru dosyaya yazar. Başarısız olursa false döner (oyun etkilenmez).</summary>
    public bool Yaz(int skor)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_dosyaYolu)!);
            File.WriteAllText(_dosyaYolu, skor.ToString());
            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return false;
        }
    }
}
