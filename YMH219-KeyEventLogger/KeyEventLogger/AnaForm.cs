using KeyEventLogger.Models;
using KeyEventLogger.Services;

namespace KeyEventLogger;

public partial class AnaForm : Form
{
    private readonly TusKaydedici _kaydedici = new();

    /// <summary>Listede tarih aralığı filtresi uygulanıyor mu?</summary>
    private bool _filtreAktif;

    /// <summary>Durum göstergesinin yanıp sönmesi için.</summary>
    private bool _gostergeYanik;

    public AnaForm()
    {
        InitializeComponent();

        _kaydedici.KayitEklendi += Kaydedici_KayitEklendi;
        _kaydedici.DurumDegisti += Kaydedici_DurumDegisti;

        dtpBaslangic.Value = DateTime.Now.Date;
        dtpBitis.Value = DateTime.Now.Date.AddDays(1).AddSeconds(-1);

        DurumGostergesiniGuncelle();
        IstatistikleriGuncelle();
    }

    // ---------------------------------------------------------------
    // Klavye olayları (KeyPreview = true sayesinde form, odaktaki
    // kontrolden önce yalnızca KENDİ penceresine ait olayları alır)
    // ---------------------------------------------------------------

    private void AnaForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (chkKeyDown.Checked)
            _kaydedici.Ekle(TusKaydi.TusOlayindan(OlayTuru.KeyDown, e));
    }

    private void AnaForm_KeyUp(object? sender, KeyEventArgs e)
    {
        if (chkKeyUp.Checked)
            _kaydedici.Ekle(TusKaydi.TusOlayindan(OlayTuru.KeyUp, e));
    }

    private void AnaForm_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (chkKeyPress.Checked)
            _kaydedici.Ekle(TusKaydi.KarakterOlayindan(e));
    }

    // ---------------------------------------------------------------
    // Kaydedici olayları
    // ---------------------------------------------------------------

    private void Kaydedici_KayitEklendi(object? sender, TusKaydi kayit)
    {
        if (!_filtreAktif)
        {
            lstKayitlar.Items.Add(kayit);
            lstKayitlar.TopIndex = Math.Max(0, lstKayitlar.Items.Count - 1); // son kayda kaydır
        }
        IstatistikleriGuncelle();
    }

    private void Kaydedici_DurumDegisti(object? sender, EventArgs e) => DurumGostergesiniGuncelle();

    // ---------------------------------------------------------------
    // Butonlar
    // ---------------------------------------------------------------

    private void btnBaslat_Click(object? sender, EventArgs e)
    {
        _kaydedici.Baslat();
        DurumYaz("Kayıt başlatıldı. Bu pencere odaktayken basılan tuşlar listelenecek.");
        txtDeneme.Focus();
    }

    private void btnDurdur_Click(object? sender, EventArgs e)
    {
        _kaydedici.Durdur();
        DurumYaz($"Kayıt durduruldu. Toplam {_kaydedici.Kayitlar.Count} olay kaydedildi.");
    }

    private void btnTemizle_Click(object? sender, EventArgs e)
    {
        if (_kaydedici.Kayitlar.Count == 0)
        {
            DurumYaz("Temizlenecek kayıt yok.");
            return;
        }

        var cevap = MessageBox.Show("Ekrandaki tüm kayıtlar silinsin mi?", "Temizle",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (cevap != DialogResult.Yes) return;

        _kaydedici.Temizle();
        lstKayitlar.Items.Clear();
        txtDeneme.Clear();
        FiltreyiKaldir();
        IstatistikleriGuncelle();
        DurumYaz("Kayıtlar temizlendi.");
    }

    private void btnKaydet_Click(object? sender, EventArgs e)
    {
        var kaydedilecek = lstKayitlar.Items.Cast<TusKaydi>().ToList();
        if (kaydedilecek.Count == 0)
        {
            MessageBox.Show("Kaydedilecek kayıt bulunmuyor.", "Bilgi",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dlg = new SaveFileDialog
        {
            Title = "Kayıtları .txt Olarak Kaydet",
            Filter = "Metin Dosyası (*.txt)|*.txt",
            FileName = $"tus_kayitlari_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            KayitDosyasi.TxtOlarakKaydet(kaydedilecek, dlg.FileName);
            DurumYaz($"{kaydedilecek.Count} kayıt dosyaya yazıldı: {dlg.FileName}");
            MessageBox.Show($"{kaydedilecek.Count} kayıt başarıyla kaydedildi.", "Kaydedildi",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            MessageBox.Show("Dosya kaydedilemedi:\n" + ex.Message, "Hata",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ---------------------------------------------------------------
    // Filtreleme (tarih / saat aralığı)
    // ---------------------------------------------------------------

    private void btnFiltrele_Click(object? sender, EventArgs e)
    {
        try
        {
            var sonuc = _kaydedici.Filtrele(dtpBaslangic.Value, dtpBitis.Value);
            lstKayitlar.BeginUpdate();
            lstKayitlar.Items.Clear();
            foreach (var kayit in sonuc) lstKayitlar.Items.Add(kayit);
            lstKayitlar.EndUpdate();

            _filtreAktif = true;
            btnFiltreKaldir.Enabled = true;
            lblListeBaslik.Text = $"Filtrelenmiş Kayıtlar: {sonuc.Count} / {_kaydedici.Kayitlar.Count}" +
                                  $"   ({dtpBaslangic.Value:HH:mm:ss} - {dtpBitis.Value:HH:mm:ss})";
            DurumYaz($"Filtre uygulandı: {sonuc.Count} kayıt bulundu.");
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Geçersiz Aralık", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void btnFiltreKaldir_Click(object? sender, EventArgs e)
    {
        FiltreyiKaldir();
        DurumYaz("Filtre kaldırıldı, tüm kayıtlar gösteriliyor.");
    }

    private void FiltreyiKaldir()
    {
        _filtreAktif = false;
        btnFiltreKaldir.Enabled = false;
        lblListeBaslik.Text = "Olay Kayıtları (Tarih / Saat  -  Olay  -  Tuş)";

        lstKayitlar.BeginUpdate();
        lstKayitlar.Items.Clear();
        foreach (var kayit in _kaydedici.Kayitlar) lstKayitlar.Items.Add(kayit);
        lstKayitlar.EndUpdate();
        if (lstKayitlar.Items.Count > 0) lstKayitlar.TopIndex = lstKayitlar.Items.Count - 1;
    }

    // ---------------------------------------------------------------
    // Gösterge, istatistik, odak
    // ---------------------------------------------------------------

    private void DurumGostergesiniGuncelle()
    {
        bool aktif = _kaydedici.KayitAktif;
        btnBaslat.Enabled = !aktif;
        btnDurdur.Enabled = aktif;

        if (aktif)
        {
            lblDurum.Text = "●  KAYIT AKTİF";
            lblDurum.BackColor = Color.FromArgb(211, 47, 47);
            Text = "● KAYIT - Key Event Logger (Eğitsel) - YMH219";
            tmrGosterge.Start();
        }
        else
        {
            tmrGosterge.Stop();
            lblDurum.Text = "■  KAYIT DURDURULDU";
            lblDurum.BackColor = Color.Gray;
            Text = "Key Event Logger (Eğitsel) - YMH219";
        }
    }

    private void tmrGosterge_Tick(object? sender, EventArgs e)
    {
        // Kayıt aktifken gösterge yanıp söner.
        _gostergeYanik = !_gostergeYanik;
        lblDurum.BackColor = _gostergeYanik ? Color.FromArgb(211, 47, 47) : Color.FromArgb(239, 83, 80);
    }

    private void IstatistikleriGuncelle()
    {
        lblToplam.Text = $"Toplam tuş basma: {_kaydedici.ToplamTusBasma}   (toplam olay: {_kaydedici.Kayitlar.Count})";

        lstEnSik.BeginUpdate();
        lstEnSik.Items.Clear();
        int sira = 1;
        foreach (var (tus, adet) in _kaydedici.EnSikTuslar(5))
            lstEnSik.Items.Add($"{sira++}. {tus,-12} {adet,4} kez");
        lstEnSik.EndUpdate();
    }

    private void AnaForm_Activated(object? sender, EventArgs e)
    {
        lblOdak.BackColor = Color.FromArgb(232, 245, 233);
        lblOdak.ForeColor = Color.DarkGreen;
        lblOdak.Text = "  Pencere odakta: tuş olayları yalnızca bu form üzerinden alınabilir.";
    }

    private void AnaForm_Deactivate(object? sender, EventArgs e)
    {
        lblOdak.BackColor = Color.FromArgb(250, 250, 250);
        lblOdak.ForeColor = Color.DimGray;
        lblOdak.Text = "  Pencere odakta değil: başka uygulamalarda basılan tuşlar YAKALANMAZ.";
    }

    private void DurumYaz(string mesaj) => lblDurumCubugu.Text = $"[{DateTime.Now:HH:mm:ss}] {mesaj}";
}
