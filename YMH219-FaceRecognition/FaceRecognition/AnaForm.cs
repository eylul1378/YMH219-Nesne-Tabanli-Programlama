using System.Diagnostics;
using FaceRecognition.Services;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using CvRect = OpenCvSharp.Rect;

namespace FaceRecognition;

public partial class AnaForm : Form
{
    private YuzAlgilayici? _algilayici;
    private readonly YuzTanimaModeli _model = new();
    private readonly VeriKumesi _veri = new();
    private readonly Kamera _kamera = new();

    /// <summary>Son işlenen görüntü (kamera karesi veya seçilen resim); örnek eklemek için kullanılır.</summary>
    private Mat? _sonGoruntu;
    private CvRect[] _sonYuzler = Array.Empty<CvRect>();
    private readonly object _kilit = new();

    /// <summary>Arayüz önceki kareyi henüz çizmediyse yeni kare atlanır (kuyruk birikmesin).</summary>
    private volatile bool _cizimBekliyor;
    private DateTime _sonAlgilananKayit = DateTime.MinValue;

    public AnaForm()
    {
        InitializeComponent();
        _kamera.KareAlindi += Kamera_KareAlindi;
        _kamera.HataOlustu += (_, mesaj) => BeginInvoke(() =>
        {
            KamerayiDurdur();
            Uyari(mesaj);
        });
    }

    private void AnaForm_Load(object? sender, EventArgs e)
    {
        try
        {
            _algilayici = new YuzAlgilayici(YuzAlgilayici.VarsayilanCascadeYolu());
        }
        catch (Exception ex) when (ex is FileNotFoundException or InvalidDataException)
        {
            DurumYaz("Yüz algılama modeli yüklenemedi!", Color.Firebrick);
            Uyari(ex.Message + "\n\nUygulama klasöründe Cascades\\" + YuzAlgilayici.CascadeDosyaAdi +
                  " dosyası bulunmalıdır.");
            btnKameraBaslat.Enabled = btnGorselSec.Enabled = btnOrnekEkle.Enabled = false;
        }

        try
        {
            if (_model.Yukle(_veri.ModelDosyasi))
                lblModelDurum.Text = $"Kayıtlı model yüklendi. Etiketler: {string.Join(", ", _model.Etiketler)}";
        }
        catch (Exception ex) when (ex is IOException or OpenCVException)
        {
            lblModelDurum.Text = "Kayıtlı model okunamadı; modeli yeniden eğitin.";
        }

        EtiketleriYenile();
    }

    private void AnaForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        _kamera.KareAlindi -= Kamera_KareAlindi;
        _kamera.Dispose();
        _algilayici?.Dispose();
        _model.Dispose();
        lock (_kilit) _sonGoruntu?.Dispose();
    }

    // =================================================================
    // Görüntü kaynağı: kamera / dosya
    // =================================================================

    private async void btnKameraBaslat_Click(object? sender, EventArgs e)
    {
        btnKameraBaslat.Enabled = false;
        DurumYaz("Kamera açılıyor...", Color.DarkOrange);
        try
        {
            await _kamera.BaslatAsync((int)nudKamera.Value);
            btnKameraDurdur.Enabled = true;
            btnGorselSec.Enabled = false;
            DurumYaz("● KAMERA AKTİF", Color.FromArgb(198, 40, 40));
        }
        catch (InvalidOperationException ex)
        {
            btnKameraBaslat.Enabled = true;
            DurumYaz("Kamera açılamadı", Color.Gray);
            Uyari(ex.Message);
        }
    }

    private void btnKameraDurdur_Click(object? sender, EventArgs e) => KamerayiDurdur();

    private void KamerayiDurdur()
    {
        _kamera.Durdur();
        btnKameraBaslat.Enabled = _algilayici != null;
        btnKameraDurdur.Enabled = false;
        btnGorselSec.Enabled = _algilayici != null;
        DurumYaz("Kamera durduruldu", Color.Gray);
    }

    private void btnGorselSec_Click(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Görüntü Seç",
            Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp|Tüm Dosyalar|*.*"
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        Mat? resim;
        try
        {
            resim = GoruntuDosyasi.Oku(dlg.FileName);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            Uyari("Dosya okunamadı: " + ex.Message);
            return;
        }
        if (resim == null)
        {
            Uyari("Seçilen dosya geçerli bir resim değil.");
            return;
        }

        using (resim)
        {
            var (bitmap, sonuclar) = Isle(resim);
            Goster(bitmap, sonuclar);
        }
        DurumYaz("Görüntü: " + Path.GetFileName(dlg.FileName), Color.FromArgb(25, 118, 210));
    }

    /// <summary>Kamera iş parçacığında her kare için çalışır.</summary>
    private void Kamera_KareAlindi(object? sender, Mat kare)
    {
        if (_cizimBekliyor || IsDisposed) return;
        _cizimBekliyor = true;
        try
        {
            var (bitmap, sonuclar) = Isle(kare);
            BeginInvoke(() =>
            {
                Goster(bitmap, sonuclar);
                _cizimBekliyor = false;
            });
        }
        catch (Exception)
        {
            _cizimBekliyor = false;
        }
    }

    // =================================================================
    // Algılama + tanıma + çizim
    // =================================================================

    /// <summary>
    /// Görüntüdeki yüzleri bulur, model eğitilmişse her yüzü tanır ve sonuçları
    /// görüntü üzerine dikdörtgen ve etiket olarak çizer.
    /// </summary>
    private (Bitmap Bitmap, List<string> Sonuclar) Isle(Mat goruntu)
    {
        var yuzler = _algilayici!.Algila(goruntu);
        var sonuclar = new List<string>();
        var etiketler = new List<(CvRect Yuz, string Metin, Color Renk)>();

        foreach (var yuz in yuzler)
        {
            string metin;
            Color renk;
            if (_model.Egitildi)
            {
                using var hazir = YuzAlgilayici.YuzuHazirla(goruntu, yuz);
                var sonuc = _model.Tahmin(hazir);
                metin = sonuc.Taninmis ? $"{sonuc.Etiket} %{sonuc.GuvenYuzdesi}" : "Bilinmiyor";
                renk = sonuc.Taninmis ? Color.LimeGreen : Color.Orange;
                sonuclar.Add(sonuc.ToString());
            }
            else
            {
                metin = "Yüz";
                renk = Color.DeepSkyBlue;
                sonuclar.Add($"Yüz: x={yuz.X}, y={yuz.Y}, {yuz.Width}x{yuz.Height} (model eğitilmedi)");
            }
            etiketler.Add((yuz, metin, renk));
        }

        // İsteğe bağlı: algılanan yüzleri saniyede en fazla bir kez kaydet.
        if (chkAlgilananKaydet.Checked && yuzler.Length > 0 && DateTime.Now - _sonAlgilananKayit > TimeSpan.FromSeconds(1))
        {
            _sonAlgilananKayit = DateTime.Now;
            foreach (var yuz in yuzler)
            {
                using var kesit = new Mat(goruntu, yuz.Intersect(new CvRect(0, 0, goruntu.Width, goruntu.Height)));
                try { _veri.AlgilananYuzuKaydet(kesit); } catch (IOException) { }
            }
        }

        lock (_kilit)
        {
            _sonGoruntu?.Dispose();
            _sonGoruntu = goruntu.Clone();
            _sonYuzler = yuzler;
        }

        // Türkçe karakterli etiketler için çizim GDI+ ile yapılır (Cv2.PutText yalnızca ASCII destekler).
        var bitmap = goruntu.ToBitmap();
        using (var g = Graphics.FromImage(bitmap))
        using (var font = new Font("Segoe UI", Math.Max(10, goruntu.Width / 55f), FontStyle.Bold))
        {
            foreach (var (yuz, metin, renk) in etiketler)
            {
                using var kalem = new Pen(renk, Math.Max(2, goruntu.Width / 250f));
                g.DrawRectangle(kalem, yuz.X, yuz.Y, yuz.Width, yuz.Height);
                var olcu = g.MeasureString(metin, font);
                float y = Math.Max(0, yuz.Y - olcu.Height);
                using var arka = new SolidBrush(renk);
                g.FillRectangle(arka, yuz.X, y, olcu.Width, olcu.Height);
                g.DrawString(metin, font, Brushes.Black, yuz.X, y);
            }
        }
        return (bitmap, sonuclar);
    }

    private void Goster(Bitmap bitmap, List<string> sonuclar)
    {
        var eski = pbGoruntu.Image;
        pbGoruntu.Image = bitmap;
        eski?.Dispose();

        lstYuzler.BeginUpdate();
        lstYuzler.Items.Clear();
        foreach (var s in sonuclar) lstYuzler.Items.Add(s);
        lstYuzler.EndUpdate();

        lblSonuc.Text = sonuclar.Count switch
        {
            0 => "Yüz algılanmadı",
            1 => "1 yüz algılandı",
            _ => $"{sonuclar.Count} yüz algılandı"
        };
        lblSonuc.ForeColor = sonuclar.Count > 0 ? Color.DarkGreen : Color.DimGray;
    }

    // =================================================================
    // Eğitim
    // =================================================================

    private void btnOrnekEkle_Click(object? sender, EventArgs e)
    {
        string etiket = txtEtiket.Text.Trim();
        if (etiket.Length == 0)
        {
            Uyari("Örneğin kime ait olduğunu belirten bir etiket (kişi adı) yazın.");
            txtEtiket.Focus();
            return;
        }

        Mat? yuzMat = null;
        lock (_kilit)
        {
            if (_sonGoruntu != null && _sonYuzler.Length > 0)
            {
                // Birden fazla yüz varsa en büyüğü (kameraya en yakın) alınır.
                var enBuyuk = _sonYuzler.OrderByDescending(r => r.Width * r.Height).First();
                yuzMat = YuzAlgilayici.YuzuHazirla(_sonGoruntu, enBuyuk);
            }
        }

        if (yuzMat == null)
        {
            Uyari("Şu anki görüntüde yüz algılanmadı. Kameraya bakın veya yüz içeren bir resim seçin.");
            return;
        }

        try
        {
            using (yuzMat) _veri.OrnekEkle(etiket, yuzMat);
            EtiketleriYenile();
            lblModelDurum.Text = "Yeni örnek eklendi. Değişikliklerin geçerli olması için modeli yeniden eğitin.";
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or ArgumentException)
        {
            Uyari("Örnek kaydedilemedi: " + ex.Message);
        }
    }

    private async void btnEgit_Click(object? sender, EventArgs e)
    {
        btnEgit.Enabled = false;
        lblModelDurum.Text = "Model eğitiliyor...";
        try
        {
            await Task.Run(() =>
            {
                var ornekler = _veri.TumOrnekler();
                try
                {
                    _model.Egit(ornekler);
                    _model.Kaydet(_veri.ModelDosyasi);
                }
                finally
                {
                    foreach (var (mat, _) in ornekler) mat.Dispose();
                }
            });
            var sayilar = _veri.EtiketSayilari();
            lblModelDurum.Text = $"Model eğitildi ✓  {sayilar.Count} etiket, {sayilar.Values.Sum()} örnek.";
        }
        catch (InvalidOperationException ex)
        {
            lblModelDurum.Text = "Model eğitilemedi.";
            Uyari(ex.Message);
        }
        catch (Exception ex) when (ex is IOException or OpenCVException)
        {
            lblModelDurum.Text = "Model eğitilemedi.";
            Uyari("Eğitim sırasında hata: " + ex.Message);
        }
        finally
        {
            btnEgit.Enabled = true;
        }
    }

    private void btnEtiketSil_Click(object? sender, EventArgs e)
    {
        if (lstEtiketler.SelectedItem is not string satir) return;
        string etiket = satir[..satir.LastIndexOf(" (", StringComparison.Ordinal)];
        if (MessageBox.Show($"\"{etiket}\" etiketine ait tüm örnekler silinsin mi?", "Etiketi Sil",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

        try
        {
            _veri.EtiketiSil(etiket);
            EtiketleriYenile();
            lblModelDurum.Text = "Etiket silindi. Modeli yeniden eğitin.";
        }
        catch (IOException ex)
        {
            Uyari("Silinemedi: " + ex.Message);
        }
    }

    private void btnKlasorAc_Click(object? sender, EventArgs e)
    {
        Directory.CreateDirectory(_veri.AnaKlasor);
        Process.Start(new ProcessStartInfo("explorer.exe", $"\"{_veri.AnaKlasor}\"") { UseShellExecute = true });
    }

    private void nudEsik_ValueChanged(object? sender, EventArgs e) => _model.EsikMesafe = (double)nudEsik.Value;

    private void EtiketleriYenile()
    {
        lstEtiketler.Items.Clear();
        foreach (var (etiket, adet) in _veri.EtiketSayilari())
            lstEtiketler.Items.Add($"{etiket} ({adet} örnek)");
        if (lstEtiketler.Items.Count == 0)
            lstEtiketler.Items.Add("(henüz örnek yok)");
    }

    // =================================================================

    private void DurumYaz(string metin, Color renk)
    {
        lblDurum.Text = metin;
        lblDurum.BackColor = renk;
    }

    private void Uyari(string mesaj) =>
        MessageBox.Show(this, mesaj, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
}
