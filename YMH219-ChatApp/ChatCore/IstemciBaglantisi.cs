namespace ChatCore;

/// <summary>
/// Sunucu tarafında bağlı bir istemciyi temsil eder (kullanıcı adı, adres, bağlanma zamanı, mesaj sayısı).
/// </summary>
public sealed class IstemciBaglantisi : IDisposable
{
    internal MesajKanali Kanal { get; }

    public string KullaniciAdi { get; internal set; } = string.Empty;
    public string Adres { get; }
    public DateTime BaglanmaZamani { get; } = DateTime.Now;
    public int GonderdigiMesajSayisi { get; internal set; }

    internal IstemciBaglantisi(MesajKanali kanal)
    {
        Kanal = kanal;
        Adres = kanal.UzakAdres;
    }

    public Task GonderAsync(Mesaj mesaj) => Kanal.GonderAsync(mesaj);

    public void Dispose() => Kanal.Dispose();

    public override string ToString() => $"{KullaniciAdi} ({Adres})";
}
