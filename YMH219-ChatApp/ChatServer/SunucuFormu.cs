using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using ChatCore;

namespace ChatServer;

public partial class SunucuFormu : Form
{
    private readonly SohbetSunucusu _sunucu = new();

    public SunucuFormu()
    {
        InitializeComponent();

        _sunucu.Gunluk += (_, metin) => ArayuzdeCalistir(() => GunlugeEkle(metin));
        _sunucu.IstemciBaglandi += (_, _) => ArayuzdeCalistir(IstemciListesiniYenile);
        _sunucu.IstemciAyrildi += (_, _) => ArayuzdeCalistir(IstemciListesiniYenile);
        _sunucu.MesajIletildi += (_, mesaj) => ArayuzdeCalistir(() =>
        {
            GunlugeEkle(mesaj.ToString());
            IstemciListesiniYenile(); // mesaj sayıları güncellensin
        });
    }

    /// <summary>
    /// Ağ olayları farklı bir iş parçacığından gelebilir; arayüz kontrolleri
    /// yalnızca UI iş parçacığından güncellenebildiği için Invoke kullanılır.
    /// </summary>
    private void ArayuzdeCalistir(Action islem)
    {
        if (IsDisposed || Disposing) return;
        if (InvokeRequired) BeginInvoke(islem);
        else islem();
    }

    private void SunucuFormu_Load(object? sender, EventArgs e)
    {
        lblIpAdresleri.Text = "Bu bilgisayarın IP adresleri (istemciler bu adreslerle bağlanabilir): " +
                              string.Join(", ", YerelIpAdresleri().Prepend("127.0.0.1"));
        ArayuzuGuncelle();
    }

    private static IEnumerable<string> YerelIpAdresleri()
    {
        try
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up &&
                            n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(n => n.GetIPProperties().UnicastAddresses)
                .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork)
                .Select(a => a.Address.ToString())
                .Distinct()
                .ToList();
        }
        catch (NetworkInformationException)
        {
            return Array.Empty<string>();
        }
    }

    private void btnBaslat_Click(object? sender, EventArgs e)
    {
        try
        {
            _sunucu.Baslat((int)nudPort.Value, chkSadeceYerel.Checked);
        }
        catch (SocketException ex)
        {
            string mesaj = ex.SocketErrorCode == SocketError.AddressAlreadyInUse
                ? $"{nudPort.Value} portu başka bir uygulama tarafından kullanılıyor. Farklı bir port deneyin."
                : "Sunucu başlatılamadı: " + ex.Message;
            MessageBox.Show(mesaj, "Sunucu Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        ArayuzuGuncelle();
    }

    private async void btnDurdur_Click(object? sender, EventArgs e)
    {
        btnDurdur.Enabled = false;
        await _sunucu.DurdurAsync();
        IstemciListesiniYenile();
        ArayuzuGuncelle();
    }

    private async void btnDuyuru_Click(object? sender, EventArgs e)
    {
        string metin = txtDuyuru.Text.Trim();
        if (metin.Length == 0) return;
        if (!_sunucu.Calisiyor)
        {
            MessageBox.Show("Duyuru göndermek için önce sunucuyu başlatın.", "Bilgi",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        await _sunucu.DuyuruGonderAsync(metin);
        txtDuyuru.Clear();
    }

    private void txtDuyuru_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        e.SuppressKeyPress = true;
        btnDuyuru.PerformClick();
    }

    private void btnGunlukKaydet_Click(object? sender, EventArgs e)
    {
        using var dlg = new SaveFileDialog
        {
            Filter = "Metin Dosyası (*.txt)|*.txt",
            FileName = $"sunucu_gunlugu_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            File.WriteAllLines(dlg.FileName, lstGunluk.Items.Cast<string>(), Encoding.UTF8);
            GunlugeEkle($"[{DateTime.Now:HH:mm:ss}] Günlük kaydedildi: {dlg.FileName}");
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            MessageBox.Show("Günlük kaydedilemedi:\n" + ex.Message, "Hata",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void SunucuFormu_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (_sunucu.Calisiyor)
            await _sunucu.DurdurAsync();
    }

    private void IstemciListesiniYenile()
    {
        lvIstemciler.BeginUpdate();
        lvIstemciler.Items.Clear();
        foreach (var istemci in _sunucu.Istemciler.OrderBy(i => i.BaglanmaZamani))
        {
            var satir = new ListViewItem(istemci.KullaniciAdi);
            satir.SubItems.Add(istemci.Adres);
            satir.SubItems.Add(istemci.BaglanmaZamani.ToString("HH:mm:ss"));
            satir.SubItems.Add(istemci.GonderdigiMesajSayisi.ToString());
            lvIstemciler.Items.Add(satir);
        }
        lvIstemciler.EndUpdate();
        lblIstemciSayisi.Text = $"Bağlı istemci: {lvIstemciler.Items.Count}";
    }

    private void GunlugeEkle(string metin)
    {
        lstGunluk.Items.Add(metin);
        lstGunluk.TopIndex = lstGunluk.Items.Count - 1;
    }

    private void ArayuzuGuncelle()
    {
        bool calisiyor = _sunucu.Calisiyor;
        btnBaslat.Enabled = !calisiyor;
        btnDurdur.Enabled = calisiyor;
        nudPort.Enabled = !calisiyor;
        chkSadeceYerel.Enabled = !calisiyor;

        lblDurum.BackColor = calisiyor ? Color.FromArgb(46, 125, 50) : Color.Gray;
        lblDurum.Text = calisiyor ? $"● Dinleniyor: port {_sunucu.Port}" : "● Sunucu kapalı";
    }
}
