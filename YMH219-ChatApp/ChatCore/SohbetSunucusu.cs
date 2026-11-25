using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace ChatCore;

/// <summary>
/// TcpListener ile istemci bağlantılarını kabul eden, mesajları odadaki herkese veya
/// özel mesaj olarak tek kullanıcıya ileten sohbet sunucusu.
/// </summary>
public class SohbetSunucusu
{
    public const string SunucuAdi = "Sunucu";

    private readonly ConcurrentDictionary<string, IstemciBaglantisi> _istemciler =
        new(StringComparer.OrdinalIgnoreCase);

    private TcpListener? _dinleyici;
    private CancellationTokenSource? _iptal;

    public bool Calisiyor { get; private set; }
    public int Port { get; private set; }

    public IReadOnlyCollection<IstemciBaglantisi> Istemciler => _istemciler.Values.ToList();

    /// <summary>Sunucu günlüğüne yazılacak bir olay olduğunda tetiklenir.</summary>
    public event EventHandler<string>? Gunluk;

    /// <summary>Bir kullanıcı başarıyla bağlandığında.</summary>
    public event EventHandler<IstemciBaglantisi>? IstemciBaglandi;

    /// <summary>Bir kullanıcı ayrıldığında veya bağlantısı koptuğunda.</summary>
    public event EventHandler<IstemciBaglantisi>? IstemciAyrildi;

    /// <summary>Sunucu üzerinden bir sohbet / özel mesaj geçtiğinde.</summary>
    public event EventHandler<Mesaj>? MesajIletildi;

    /// <summary>Sunucuyu başlatır ve bağlantı kabul döngüsünü arka planda çalıştırır.</summary>
    /// <param name="port">Dinlenecek port.</param>
    /// <param name="sadeceYerel">true ise yalnızca 127.0.0.1, false ise tüm ağ arayüzleri dinlenir.</param>
    public void Baslat(int port, bool sadeceYerel = false)
    {
        if (Calisiyor) throw new InvalidOperationException("Sunucu zaten çalışıyor.");

        var adres = sadeceYerel ? IPAddress.Loopback : IPAddress.Any;
        _dinleyici = new TcpListener(adres, port);
        _dinleyici.Start(); // port kullanımdaysa SocketException fırlatır
        _iptal = new CancellationTokenSource();
        Port = port;
        Calisiyor = true;

        GunlukYaz($"Sunucu başlatıldı: {adres}:{port}");
        _ = BaglantiKabulDongusuAsync(_iptal.Token);
    }

    public async Task DurdurAsync()
    {
        if (!Calisiyor) return;
        Calisiyor = false;

        await HerkeseGonderAsync(new Mesaj(MesajTuru.Sistem, SunucuAdi, "Sunucu kapatıldı."));

        _iptal?.Cancel();
        _dinleyici?.Stop();

        foreach (var istemci in _istemciler.Values)
            istemci.Dispose();
        _istemciler.Clear();

        GunlukYaz("Sunucu durduruldu.");
    }

    private async Task BaglantiKabulDongusuAsync(CancellationToken iptal)
    {
        while (!iptal.IsCancellationRequested)
        {
            TcpClient tcp;
            try
            {
                tcp = await _dinleyici!.AcceptTcpClientAsync(iptal);
            }
            catch (OperationCanceledException) { break; }
            catch (ObjectDisposedException) { break; }
            catch (SocketException ex)
            {
                if (!Calisiyor) break;
                GunlukYaz("Bağlantı kabul hatası: " + ex.Message);
                continue;
            }

            // Her istemci kendi görevinde (Task) dinlenir; sunucu yeni bağlantıları kabul etmeye devam eder.
            _ = IstemciyiDinleAsync(tcp, iptal);
        }
    }

    private async Task IstemciyiDinleAsync(TcpClient tcp, CancellationToken iptal)
    {
        var istemci = new IstemciBaglantisi(new MesajKanali(tcp));
        bool kayitli = false;

        try
        {
            // 1) İlk mesaj kullanıcı adıyla bağlanma isteği olmalı.
            var ilk = await istemci.Kanal.OkuAsync(iptal);
            if (ilk == null || ilk.Tur != MesajTuru.Baglan)
            {
                GunlukYaz($"{istemci.Adres} geçersiz bağlantı isteği gönderdi, kapatıldı.");
                return;
            }

            string ad = ilk.Gonderen.Trim();
            string? hata = KullaniciAdiHatasi(ad);
            istemci.KullaniciAdi = ad;
            if (hata == null && !_istemciler.TryAdd(ad, istemci))
                hata = $"\"{ad}\" kullanıcı adı zaten kullanılıyor.";

            if (hata != null)
            {
                await istemci.GonderAsync(new Mesaj(MesajTuru.BaglantiRed, SunucuAdi, hata));
                GunlukYaz($"{istemci.Adres} reddedildi: {hata}");
                return;
            }

            kayitli = true;

            await istemci.GonderAsync(new Mesaj(MesajTuru.BaglantiKabul, SunucuAdi, $"Hoş geldin {ad}!"));
            GunlukYaz($"{ad} bağlandı ({istemci.Adres}).");
            IstemciBaglandi?.Invoke(this, istemci);

            await HerkeseGonderAsync(new Mesaj(MesajTuru.Sistem, SunucuAdi, $"{ad} sohbete katıldı."));
            await KullaniciListesiniGonderAsync();

            // 2) Mesaj döngüsü
            while (!iptal.IsCancellationRequested)
            {
                var mesaj = await istemci.Kanal.OkuAsync(iptal);
                if (mesaj == null || mesaj.Tur == MesajTuru.Ayril) break;
                await MesajiIsleAsync(istemci, mesaj);
            }
        }
        catch (OperationCanceledException) { }
        catch (IOException) { /* istemci bağlantısı beklenmedik şekilde koptu */ }
        catch (ObjectDisposedException) { }
        catch (Exception ex)
        {
            GunlukYaz($"{istemci} ile iletişim hatası: {ex.Message}");
        }
        finally
        {
            istemci.Dispose();
            if (kayitli && _istemciler.TryRemove(istemci.KullaniciAdi, out _))
            {
                GunlukYaz($"{istemci.KullaniciAdi} ayrıldı.");
                IstemciAyrildi?.Invoke(this, istemci);
                if (Calisiyor)
                {
                    await HerkeseGonderAsync(new Mesaj(MesajTuru.Sistem, SunucuAdi,
                        $"{istemci.KullaniciAdi} sohbetten ayrıldı."));
                    await KullaniciListesiniGonderAsync();
                }
            }
        }
    }

    private static string? KullaniciAdiHatasi(string ad)
    {
        if (string.IsNullOrWhiteSpace(ad)) return "Kullanıcı adı boş olamaz.";
        if (ad.Length > 20) return "Kullanıcı adı en fazla 20 karakter olabilir.";
        if (ad.Equals(SunucuAdi, StringComparison.OrdinalIgnoreCase)) return "Bu kullanıcı adı ayrılmıştır.";
        return null;
    }

    private async Task MesajiIsleAsync(IstemciBaglantisi gonderen, Mesaj mesaj)
    {
        if (string.IsNullOrWhiteSpace(mesaj.Icerik)) return;

        // Gönderen adı ve zaman sunucu tarafından belirlenir (sahte gönderen engellenir).
        mesaj.Gonderen = gonderen.KullaniciAdi;
        mesaj.Zaman = DateTime.Now;
        gonderen.GonderdigiMesajSayisi++;

        switch (mesaj.Tur)
        {
            case MesajTuru.Sohbet:
                await HerkeseGonderAsync(mesaj);
                MesajIletildi?.Invoke(this, mesaj);
                break;

            case MesajTuru.Ozel:
                if (mesaj.Alici != null && _istemciler.TryGetValue(mesaj.Alici, out var alici))
                {
                    await GuvenliGonderAsync(alici, mesaj);
                    if (alici != gonderen) await GuvenliGonderAsync(gonderen, mesaj); // gönderene kopya
                    MesajIletildi?.Invoke(this, mesaj);
                }
                else
                {
                    await GuvenliGonderAsync(gonderen, new Mesaj(MesajTuru.Sistem, SunucuAdi,
                        $"\"{mesaj.Alici}\" adlı kullanıcı çevrimiçi değil."));
                }
                break;
        }
    }

    /// <summary>Sunucudan tüm kullanıcılara duyuru gönderir.</summary>
    public Task DuyuruGonderAsync(string metin)
    {
        var mesaj = new Mesaj(MesajTuru.Sistem, SunucuAdi, "[Duyuru] " + metin);
        GunlukYaz("Duyuru gönderildi: " + metin);
        return HerkeseGonderAsync(mesaj);
    }

    private async Task HerkeseGonderAsync(Mesaj mesaj)
    {
        foreach (var istemci in _istemciler.Values)
            await GuvenliGonderAsync(istemci, mesaj);
    }

    private Task KullaniciListesiniGonderAsync()
    {
        var liste = new Mesaj(MesajTuru.KullaniciListesi, SunucuAdi, string.Empty)
        {
            Kullanicilar = _istemciler.Keys.OrderBy(k => k).ToList()
        };
        return HerkeseGonderAsync(liste);
    }

    /// <summary>Bir istemciye gönderim hatası tüm sunucuyu etkilemesin diye hatayı yakalar.</summary>
    private async Task GuvenliGonderAsync(IstemciBaglantisi istemci, Mesaj mesaj)
    {
        try
        {
            await istemci.GonderAsync(mesaj);
        }
        catch (Exception ex) when (ex is IOException or ObjectDisposedException or InvalidOperationException)
        {
            GunlukYaz($"{istemci.KullaniciAdi} kullanıcısına mesaj gönderilemedi.");
        }
    }

    private void GunlukYaz(string metin) => Gunluk?.Invoke(this, $"[{DateTime.Now:HH:mm:ss}] {metin}");
}
