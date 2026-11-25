using System.Net.Sockets;

namespace ChatCore;

/// <summary>
/// TcpClient ile sunucuya bağlanan, mesaj gönderen ve gelen mesajları olay (event) ile bildiren istemci.
/// </summary>
public class SohbetIstemcisi
{
    private MesajKanali? _kanal;
    private CancellationTokenSource? _iptal;

    public string KullaniciAdi { get; private set; } = string.Empty;
    public bool Bagli => _kanal?.Bagli == true;

    /// <summary>Sunucudan bir mesaj geldiğinde tetiklenir.</summary>
    public event EventHandler<Mesaj>? MesajAlindi;

    /// <summary>Bağlantı koptuğunda veya kesildiğinde tetiklenir. Parametre: açıklama.</summary>
    public event EventHandler<string>? BaglantiKesildi;

    /// <summary>
    /// Sunucuya bağlanır ve kullanıcı adını bildirir. Sunucu kabul ederse true döner.
    /// Bağlantı hatalarında açıklayıcı mesajla <see cref="SohbetBaglantiHatasi"/> fırlatır.
    /// </summary>
    public async Task BaglanAsync(string sunucu, int port, string kullaniciAdi, int zamanAsimiMs = 5000)
    {
        if (Bagli) throw new InvalidOperationException("Zaten bağlı.");
        if (string.IsNullOrWhiteSpace(kullaniciAdi))
            throw new SohbetBaglantiHatasi("Kullanıcı adı boş olamaz.");

        var tcp = new TcpClient();
        using var zamanAsimi = new CancellationTokenSource(zamanAsimiMs);
        try
        {
            await tcp.ConnectAsync(sunucu, port, zamanAsimi.Token);
        }
        catch (OperationCanceledException)
        {
            tcp.Dispose();
            throw new SohbetBaglantiHatasi($"{sunucu}:{port} adresine bağlantı zaman aşımına uğradı.");
        }
        catch (SocketException ex)
        {
            tcp.Dispose();
            throw new SohbetBaglantiHatasi(ex.SocketErrorCode switch
            {
                SocketError.ConnectionRefused => $"{sunucu}:{port} adresinde çalışan bir sunucu bulunamadı. Sunucunun başlatıldığından emin olun.",
                SocketError.HostNotFound or SocketError.NoData => $"\"{sunucu}\" adresi bulunamadı.",
                _ => "Sunucuya bağlanılamadı: " + ex.Message
            }, ex);
        }

        var kanal = new MesajKanali(tcp);
        try
        {
            await kanal.GonderAsync(new Mesaj(MesajTuru.Baglan, kullaniciAdi.Trim(), string.Empty));
            using var cevapZamanAsimi = new CancellationTokenSource(zamanAsimiMs);
            var cevap = await kanal.OkuAsync(cevapZamanAsimi.Token);

            if (cevap == null)
                throw new SohbetBaglantiHatasi("Sunucu bağlantıyı kapattı.");
            if (cevap.Tur == MesajTuru.BaglantiRed)
                throw new SohbetBaglantiHatasi("Sunucu bağlantıyı reddetti: " + cevap.Icerik);
            if (cevap.Tur != MesajTuru.BaglantiKabul)
                throw new SohbetBaglantiHatasi("Sunucudan beklenmeyen cevap alındı.");

            _kanal = kanal;
            KullaniciAdi = kullaniciAdi.Trim();
            _iptal = new CancellationTokenSource();
            MesajAlindi?.Invoke(this, cevap);
            _ = DinlemeDongusuAsync(_kanal, _iptal.Token);
        }
        catch (OperationCanceledException)
        {
            kanal.Dispose();
            throw new SohbetBaglantiHatasi("Sunucu zamanında cevap vermedi.");
        }
        catch (IOException ex)
        {
            kanal.Dispose();
            throw new SohbetBaglantiHatasi("Bağlantı sırasında iletişim hatası: " + ex.Message, ex);
        }
        catch
        {
            kanal.Dispose();
            throw;
        }
    }

    private async Task DinlemeDongusuAsync(MesajKanali kanal, CancellationToken iptal)
    {
        string sebep = "Sunucu bağlantıyı kapattı.";
        try
        {
            while (!iptal.IsCancellationRequested)
            {
                var mesaj = await kanal.OkuAsync(iptal);
                if (mesaj == null) break;
                MesajAlindi?.Invoke(this, mesaj);
            }
        }
        catch (OperationCanceledException) { sebep = "Bağlantı kesildi."; }
        catch (ObjectDisposedException) { sebep = "Bağlantı kesildi."; }
        catch (IOException) { sebep = "Sunucu ile bağlantı beklenmedik şekilde koptu."; }

        if (_kanal == kanal)
        {
            _kanal = null;
            kanal.Dispose();
            BaglantiKesildi?.Invoke(this, sebep);
        }
    }

    /// <summary>Odaya (herkese) mesaj gönderir.</summary>
    public Task MesajGonderAsync(string icerik) =>
        GonderAsync(new Mesaj(MesajTuru.Sohbet, KullaniciAdi, icerik));

    /// <summary>Belirli bir kullanıcıya özel mesaj gönderir.</summary>
    public Task OzelMesajGonderAsync(string alici, string icerik) =>
        GonderAsync(new Mesaj(MesajTuru.Ozel, KullaniciAdi, icerik, alici));

    private async Task GonderAsync(Mesaj mesaj)
    {
        var kanal = _kanal ?? throw new SohbetBaglantiHatasi("Sunucuya bağlı değilsiniz.");
        try
        {
            await kanal.GonderAsync(mesaj);
        }
        catch (Exception ex) when (ex is IOException or ObjectDisposedException or InvalidOperationException)
        {
            throw new SohbetBaglantiHatasi("Mesaj gönderilemedi, bağlantı kopmuş olabilir.", ex);
        }
    }

    /// <summary>Sunucuya ayrılma bildirimi gönderir ve bağlantıyı kapatır.</summary>
    public async Task BaglantiyiKesAsync()
    {
        var kanal = _kanal;
        if (kanal == null) return;
        try
        {
            await kanal.GonderAsync(new Mesaj(MesajTuru.Ayril, KullaniciAdi, string.Empty));
        }
        catch (Exception ex) when (ex is IOException or ObjectDisposedException or InvalidOperationException) { }

        _kanal = null;
        _iptal?.Cancel();
        kanal.Dispose();
        BaglantiKesildi?.Invoke(this, "Bağlantı kullanıcı tarafından kesildi.");
    }
}

/// <summary>Kullanıcıya gösterilebilecek açıklamaya sahip bağlantı hatası.</summary>
public class SohbetBaglantiHatasi : Exception
{
    public SohbetBaglantiHatasi(string mesaj, Exception? ic = null) : base(mesaj, ic) { }
}
