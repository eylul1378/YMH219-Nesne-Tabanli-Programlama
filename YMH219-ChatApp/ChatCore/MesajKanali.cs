using System.Net.Sockets;
using System.Text;

namespace ChatCore;

/// <summary>
/// Bir TcpClient üzerinde satır tabanlı mesaj okuma/yazma işlemlerini kapsüller.
/// NetworkStream üzerinde StreamReader / StreamWriter kullanır.
/// </summary>
public sealed class MesajKanali : IDisposable
{
    private readonly TcpClient _tcp;
    private readonly StreamReader _okuyucu;
    private readonly StreamWriter _yazici;
    private readonly SemaphoreSlim _yazmaKilidi = new(1, 1);
    private bool _kapatildi;

    public MesajKanali(TcpClient tcp)
    {
        _tcp = tcp;
        NetworkStream akis = tcp.GetStream();
        var utf8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
        _okuyucu = new StreamReader(akis, utf8);
        _yazici = new StreamWriter(akis, utf8) { AutoFlush = true, NewLine = "\n" };
    }

    /// <summary>Karşı tarafın IP:Port bilgisi.</summary>
    public string UzakAdres => _tcp.Client?.RemoteEndPoint?.ToString() ?? "?";

    public bool Bagli => !_kapatildi && _tcp.Connected;

    /// <summary>
    /// Bir sonraki mesajı bekler. Bağlantı kapanırsa null döner.
    /// Geçersiz (JSON olmayan) satırlar atlanır.
    /// </summary>
    public async Task<Mesaj?> OkuAsync(CancellationToken iptal = default)
    {
        while (true)
        {
            string? satir = await _okuyucu.ReadLineAsync(iptal);
            if (satir == null) return null; // karşı taraf bağlantıyı kapattı
            var mesaj = Mesaj.SatirdanOku(satir);
            if (mesaj != null) return mesaj;
        }
    }

    /// <summary>Mesajı gönderir. Aynı anda birden fazla gönderim olursa sıraya sokar.</summary>
    public async Task GonderAsync(Mesaj mesaj)
    {
        if (_kapatildi) throw new InvalidOperationException("Bağlantı kapalı.");
        await _yazmaKilidi.WaitAsync();
        try
        {
            await _yazici.WriteLineAsync(mesaj.SatiraCevir());
        }
        finally
        {
            _yazmaKilidi.Release();
        }
    }

    public void Dispose()
    {
        if (_kapatildi) return;
        _kapatildi = true;
        try { _tcp.Close(); } catch { /* zaten kapalı olabilir */ }
        _okuyucu.Dispose();
        try { _yazici.Dispose(); } catch (IOException) { }
        _yazmaKilidi.Dispose();
    }
}
