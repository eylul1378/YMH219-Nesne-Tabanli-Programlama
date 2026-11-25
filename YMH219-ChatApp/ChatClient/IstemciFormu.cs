using System.Text;
using ChatCore;

namespace ChatClient;

public partial class IstemciFormu : Form
{
    private const string HerkesSecenegi = "(Herkes)";

    private readonly SohbetIstemcisi _istemci = new();

    /// <summary>Gönderilen ve alınan tüm mesajlar (geçmişi dosyaya kaydetmek için).</summary>
    private readonly List<Mesaj> _gecmis = new();

    private bool _baglaniyor;

    public IstemciFormu(string? varsayilanKullaniciAdi = null)
    {
        InitializeComponent();
        txtKullaniciAdi.Text = varsayilanKullaniciAdi ?? Environment.UserName;

        _istemci.MesajAlindi += (_, mesaj) => ArayuzdeCalistir(() => MesajGeldi(mesaj));
        _istemci.BaglantiKesildi += (_, sebep) => ArayuzdeCalistir(() => BaglantiKoptu(sebep));

        ArayuzuGuncelle();
    }

    /// <summary>Ağ olaylarında arayüzün UI iş parçacığından güncellenmesini sağlar.</summary>
    private void ArayuzdeCalistir(Action islem)
    {
        if (IsDisposed || Disposing) return;
        if (InvokeRequired) BeginInvoke(islem);
        else islem();
    }

    // ---------------------------------------------------------------
    // Bağlan / Bağlantıyı Kes
    // ---------------------------------------------------------------

    private async void btnBaglan_Click(object? sender, EventArgs e)
    {
        string sunucu = txtSunucu.Text.Trim();
        string ad = txtKullaniciAdi.Text.Trim();

        if (sunucu.Length == 0 || ad.Length == 0)
        {
            MessageBox.Show("Sunucu adresi ve kullanıcı adı boş bırakılamaz.", "Eksik Bilgi",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _baglaniyor = true;
        ArayuzuGuncelle();
        try
        {
            await _istemci.BaglanAsync(sunucu, (int)nudPort.Value, ad);
            txtMesaj.Focus();
        }
        catch (SohbetBaglantiHatasi ex)
        {
            // Hata uygulamayı kapatmaz; kullanıcıya anlaşılır biçimde bildirilir.
            SistemMesajiYaz("Bağlantı hatası: " + ex.Message, Color.Firebrick);
            MessageBox.Show(ex.Message, "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _baglaniyor = false;
            ArayuzuGuncelle();
        }
    }

    private async void btnKes_Click(object? sender, EventArgs e)
    {
        await _istemci.BaglantiyiKesAsync();
    }

    private void BaglantiKoptu(string sebep)
    {
        SistemMesajiYaz(sebep, Color.Firebrick);
        lstKullanicilar.Items.Clear();
        ArayuzuGuncelle();
    }

    private async void IstemciFormu_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (_istemci.Bagli)
            await _istemci.BaglantiyiKesAsync();
    }

    // ---------------------------------------------------------------
    // Mesaj gönderme / alma
    // ---------------------------------------------------------------

    private async void btnGonder_Click(object? sender, EventArgs e)
    {
        string icerik = txtMesaj.Text.Trim();
        if (icerik.Length == 0 || !_istemci.Bagli) return;

        string? alici = SeciliAlici();
        try
        {
            if (alici == null) await _istemci.MesajGonderAsync(icerik);
            else await _istemci.OzelMesajGonderAsync(alici, icerik);
            txtMesaj.Clear();
        }
        catch (SohbetBaglantiHatasi ex)
        {
            SistemMesajiYaz(ex.Message, Color.Firebrick);
        }
        txtMesaj.Focus();
    }

    private void MesajGeldi(Mesaj mesaj)
    {
        switch (mesaj.Tur)
        {
            case MesajTuru.KullaniciListesi:
                KullaniciListesiniGuncelle(mesaj.Kullanicilar ?? new List<string>());
                return;

            case MesajTuru.BaglantiKabul:
                ArayuzuGuncelle();
                SistemMesajiYaz($"{txtSunucu.Text}:{nudPort.Value} sunucusuna bağlanıldı. {mesaj.Icerik}",
                    Color.SeaGreen);
                return;

            case MesajTuru.Sohbet:
            case MesajTuru.Ozel:
            case MesajTuru.Sistem:
                _gecmis.Add(mesaj);
                MesajiYaz(mesaj);
                return;
        }
    }

    private void MesajiYaz(Mesaj mesaj)
    {
        bool benim = mesaj.Gonderen == _istemci.KullaniciAdi;
        Color renk = mesaj.Tur switch
        {
            MesajTuru.Sistem => Color.Gray,
            MesajTuru.Ozel => Color.DarkMagenta,
            _ when benim => Color.FromArgb(21, 101, 192),
            _ => Color.Black
        };

        rtbMesajlar.SelectionStart = rtbMesajlar.TextLength;
        rtbMesajlar.SelectionLength = 0;

        // Zaman bilgisi
        rtbMesajlar.SelectionColor = Color.DimGray;
        rtbMesajlar.SelectionFont = new Font(rtbMesajlar.Font, FontStyle.Regular);
        rtbMesajlar.AppendText($"[{mesaj.Zaman:HH:mm:ss}] ");

        if (mesaj.Tur == MesajTuru.Sistem)
        {
            rtbMesajlar.SelectionColor = renk;
            rtbMesajlar.SelectionFont = new Font(rtbMesajlar.Font, FontStyle.Italic);
            rtbMesajlar.AppendText("* " + mesaj.Icerik + Environment.NewLine);
        }
        else
        {
            string baslik = mesaj.Tur == MesajTuru.Ozel
                ? $"(Özel) {mesaj.Gonderen} → {mesaj.Alici}: "
                : $"{mesaj.Gonderen}{(benim ? " (sen)" : "")}: ";
            rtbMesajlar.SelectionColor = renk;
            rtbMesajlar.SelectionFont = new Font(rtbMesajlar.Font, FontStyle.Bold);
            rtbMesajlar.AppendText(baslik);
            rtbMesajlar.SelectionFont = new Font(rtbMesajlar.Font, FontStyle.Regular);
            rtbMesajlar.AppendText(mesaj.Icerik + Environment.NewLine);
        }

        rtbMesajlar.ScrollToCaret();
    }

    /// <summary>Yalnızca bu istemcide görünen yerel bilgi mesajı.</summary>
    private void SistemMesajiYaz(string metin, Color renk)
    {
        var mesaj = new Mesaj(MesajTuru.Sistem, "İstemci", metin);
        _gecmis.Add(mesaj);
        rtbMesajlar.SelectionStart = rtbMesajlar.TextLength;
        rtbMesajlar.SelectionColor = renk;
        rtbMesajlar.SelectionFont = new Font(rtbMesajlar.Font, FontStyle.Italic);
        rtbMesajlar.AppendText($"[{mesaj.Zaman:HH:mm:ss}] * {metin}{Environment.NewLine}");
        rtbMesajlar.ScrollToCaret();
    }

    // ---------------------------------------------------------------
    // Kullanıcı listesi ve özel mesaj
    // ---------------------------------------------------------------

    private void KullaniciListesiniGuncelle(List<string> kullanicilar)
    {
        string? secili = SeciliAlici();
        lstKullanicilar.BeginUpdate();
        lstKullanicilar.Items.Clear();
        lstKullanicilar.Items.Add(HerkesSecenegi);
        foreach (var k in kullanicilar.Where(k => k != _istemci.KullaniciAdi))
            lstKullanicilar.Items.Add(k);
        lstKullanicilar.EndUpdate();

        int indeks = secili != null ? lstKullanicilar.Items.IndexOf(secili) : 0;
        lstKullanicilar.SelectedIndex = indeks >= 0 ? indeks : 0;
        lblKullanicilar.Text = $"Çevrimiçi Kullanıcılar ({kullanicilar.Count})";
    }

    /// <summary>Özel mesaj alıcısı; "(Herkes)" seçiliyse null.</summary>
    private string? SeciliAlici() =>
        lstKullanicilar.SelectedItem is string s && s != HerkesSecenegi ? s : null;

    private void lstKullanicilar_SelectedIndexChanged(object? sender, EventArgs e)
    {
        string? alici = SeciliAlici();
        lblAlici.Text = alici == null ? "Alıcı: Herkes" : $"Alıcı: {alici} (özel)";
        lblAlici.ForeColor = alici == null ? SystemColors.ControlText : Color.DarkMagenta;
    }

    // ---------------------------------------------------------------
    // Geçmişi kaydetme
    // ---------------------------------------------------------------

    private void btnGecmisKaydet_Click(object? sender, EventArgs e)
    {
        if (_gecmis.Count == 0)
        {
            MessageBox.Show("Kaydedilecek mesaj yok.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dlg = new SaveFileDialog
        {
            Filter = "Metin Dosyası (*.txt)|*.txt",
            FileName = $"sohbet_gecmisi_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            using var yazici = new StreamWriter(dlg.FileName, false, Encoding.UTF8);
            yazici.WriteLine($"Sohbet geçmişi - Kullanıcı: {_istemci.KullaniciAdi} - {DateTime.Now:dd.MM.yyyy HH:mm}");
            yazici.WriteLine(new string('-', 60));
            foreach (var m in _gecmis) yazici.WriteLine(m.ToString());
            SistemMesajiYaz("Geçmiş kaydedildi: " + dlg.FileName, Color.SeaGreen);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            MessageBox.Show("Geçmiş kaydedilemedi:\n" + ex.Message, "Hata",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ---------------------------------------------------------------

    private void ArayuzuGuncelle()
    {
        bool bagli = _istemci.Bagli;
        btnBaglan.Enabled = !bagli && !_baglaniyor;
        btnKes.Enabled = bagli;
        txtSunucu.Enabled = nudPort.Enabled = txtKullaniciAdi.Enabled = !bagli && !_baglaniyor;
        txtMesaj.Enabled = btnGonder.Enabled = bagli;

        if (_baglaniyor)
        {
            lblDurum.Text = "● Bağlanıyor...";
            lblDurum.BackColor = Color.DarkOrange;
        }
        else if (bagli)
        {
            lblDurum.Text = "● Bağlı";
            lblDurum.BackColor = Color.FromArgb(46, 125, 50);
            Text = $"Sohbet İstemcisi - {_istemci.KullaniciAdi} - YMH219";
        }
        else
        {
            lblDurum.Text = "● Bağlı değil";
            lblDurum.BackColor = Color.Gray;
            Text = "Sohbet İstemcisi - YMH219";
        }
    }
}
