using System.Text;
using SpeechApp.Services;

namespace SpeechApp;

public partial class AnaForm : Form
{
    private readonly MetinSeslendirici _seslendirici = new();
    private IKonusmaTanima? _aktifTanima;
    private bool _modelYukleniyor;
    private CancellationTokenSource? _indirmeIptal;

    public AnaForm()
    {
        InitializeComponent();
        _seslendirici.DurumDegisti += (_, _) => ArayuzdeCalistir(DurumuGuncelle);
        _seslendirici.HataOlustu += (_, mesaj) => ArayuzdeCalistir(() => HataGoster(mesaj));
    }

    /// <summary>NAudio / ses olayları farklı iş parçacığından gelebilir; arayüz UI iş parçacığında güncellenir.</summary>
    private void ArayuzdeCalistir(Action islem)
    {
        if (IsDisposed || Disposing) return;
        if (InvokeRequired) BeginInvoke(islem);
        else islem();
    }

    private void AnaForm_Load(object? sender, EventArgs e)
    {
        // --- Text-to-Speech sesleri ---
        var sesler = MetinSeslendirici.YukluSesler();
        cmbSes.Items.AddRange(sesler.ToArray());
        if (cmbSes.Items.Count > 0) cmbSes.SelectedIndex = 0;
        else HataGoster("Bilgisayarda yüklü bir konuşma sesi bulunamadı. Windows Ayarlar > Zaman ve dil > Konuşma bölümünden ses ekleyin.");

        rtbMetin.Text = "Merhaba! Bu uygulama Nesne Tabanlı Programlama dersi için hazırlandı. " +
                        "Yazdığınız metni sesli okuyabilir, mikrofondan konuşmanızı da metne dönüştürebilir.";

        // --- Speech-to-Text motorları ve mikrofonlar ---
        MotorlariYukle();
        MikrofonlariYukle();
        DurumuGuncelle();
    }

    private void AnaForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        _indirmeIptal?.Cancel();
        _seslendirici.Dispose();
        foreach (MotorSecenegi s in cmbMotor.Items)
            s.Motor?.Dispose();
    }

    // =================================================================
    // TEXT-TO-SPEECH
    // =================================================================

    private async void btnOku_Click(object? sender, EventArgs e)
    {
        string metin = rtbMetin.SelectionLength > 0 ? rtbMetin.SelectedText : rtbMetin.Text;
        if (string.IsNullOrWhiteSpace(metin))
        {
            MessageBox.Show("Okunacak bir metin yazın.", "Metin Yok", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            await _seslendirici.OkuAsync(metin, cmbSes.SelectedItem as SesSecenegi, trkHiz.Value / 10.0,
                trkSesSeviyesi.Value / 100.0);
        }
        catch (Exception ex) when (ex is InvalidOperationException or ArgumentException or System.Runtime.InteropServices.COMException)
        {
            HataGoster("Metin okunamadı: " + ex.Message);
        }
    }

    private void btnDuraklat_Click(object? sender, EventArgs e)
    {
        if (_seslendirici.Durum == SeslendirmeDurumu.Okuyor) _seslendirici.Duraklat();
        else if (_seslendirici.Durum == SeslendirmeDurumu.Duraklatildi) _seslendirici.DevamEt();
    }

    private void btnDurdur_Click(object? sender, EventArgs e) => _seslendirici.Durdur();

    private void btnTemizleTts_Click(object? sender, EventArgs e)
    {
        _seslendirici.Durdur();
        rtbMetin.Clear();
        rtbMetin.Focus();
    }

    private async void btnWavKaydet_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(rtbMetin.Text)) return;
        using var dlg = new SaveFileDialog { Filter = "WAV Ses Dosyası (*.wav)|*.wav", FileName = "konusma.wav" };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            await _seslendirici.WavOlarakKaydetAsync(rtbMetin.Text, cmbSes.SelectedItem as SesSecenegi,
                trkHiz.Value / 10.0, dlg.FileName);
            MessageBox.Show("Ses dosyası kaydedildi.", "Kaydedildi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or System.Runtime.InteropServices.COMException)
        {
            HataGoster("Ses dosyası kaydedilemedi: " + ex.Message);
        }
    }

    private void trkHiz_ValueChanged(object? sender, EventArgs e) => lblHizDeger.Text = $"{trkHiz.Value / 10.0:0.0}x";

    private void trkSesSeviyesi_ValueChanged(object? sender, EventArgs e) =>
        lblSesSeviyesiDeger.Text = $"%{trkSesSeviyesi.Value}";

    private void tmrIlerleme_Tick(object? sender, EventArgs e)
    {
        var toplam = _seslendirici.ToplamSure;
        prgOkuma.Value = toplam.TotalMilliseconds > 0
            ? (int)Math.Min(100, _seslendirici.Konum.TotalMilliseconds * 100 / toplam.TotalMilliseconds)
            : 0;
    }

    // =================================================================
    // SPEECH-TO-TEXT
    // =================================================================

    private void MotorlariYukle(IKonusmaTanima? secilecek = null)
    {
        foreach (MotorSecenegi eski in cmbMotor.Items)
            if (eski.Motor != secilecek) eski.Motor?.Dispose();

        cmbMotor.Items.Clear();
        foreach (var secenek in MotorSecenegi.Listele())
            cmbMotor.Items.Add(secenek);
        if (cmbMotor.Items.Count > 0) cmbMotor.SelectedIndex = 0;
    }

    private void MikrofonlariYukle()
    {
        cmbMikrofon.Items.Clear();
        var aygitlar = Mikrofon.Aygitlar();
        if (aygitlar.Count == 0)
        {
            cmbMikrofon.Items.Add("(mikrofon bulunamadı)");
        }
        else
        {
            cmbMikrofon.Items.AddRange(aygitlar.ToArray());
        }
        cmbMikrofon.SelectedIndex = 0;
    }

    private void cmbMotor_SelectedIndexChanged(object? sender, EventArgs e)
    {
        var secenek = cmbMotor.SelectedItem as MotorSecenegi;
        btnModelIndir.Enabled = secenek is { Hazir: false } && _indirmeIptal == null;
        DurumuGuncelle();
    }

    private async void btnDinle_Click(object? sender, EventArgs e)
    {
        if (cmbMotor.SelectedItem is not MotorSecenegi secenek)
        {
            HataGoster("Konuşma tanıma motoru bulunamadı.");
            return;
        }
        if (!secenek.Hazir)
        {
            HataGoster($"\"{secenek.IndirilecekModel!.Ad}\" konuşma modeli bu bilgisayarda yüklü değil.\n" +
                       "\"Modeli İndir\" butonu ile indirebilirsiniz (internet bağlantısı gerekir).");
            return;
        }
        if (Mikrofon.Aygitlar().Count == 0)
        {
            HataGoster("Mikrofon bulunamadı. Lütfen bir mikrofon bağlayıp tekrar deneyin.");
            MikrofonlariYukle();
            return;
        }

        var motor = secenek.Motor!;
        motor.MetinTanindi += Motor_MetinTanindi;
        motor.AraSonuc += Motor_AraSonuc;
        motor.SesSeviyesi += Motor_SesSeviyesi;
        motor.HataOlustu += Motor_HataOlustu;

        _modelYukleniyor = true;
        DurumuGuncelle();
        try
        {
            await motor.BaslatAsync(cmbMikrofon.SelectedIndex);
            _aktifTanima = motor;
        }
        catch (KonusmaTanimaHatasi ex)
        {
            OlaylariAyir(motor);
            HataGoster(ex.Message);
        }
        finally
        {
            _modelYukleniyor = false;
            DurumuGuncelle();
        }
    }

    private void btnDinlemeDurdur_Click(object? sender, EventArgs e) => DinlemeyiDurdur();

    private void DinlemeyiDurdur()
    {
        var motor = _aktifTanima;
        if (motor == null) return;
        motor.Durdur(); // son sonucu MetinTanindi ile bildirir
        OlaylariAyir(motor);
        _aktifTanima = null;
        lblAraSonuc.Text = "";
        prgSeviye.Value = 0;
        DurumuGuncelle();
    }

    private void OlaylariAyir(IKonusmaTanima motor)
    {
        motor.MetinTanindi -= Motor_MetinTanindi;
        motor.AraSonuc -= Motor_AraSonuc;
        motor.SesSeviyesi -= Motor_SesSeviyesi;
        motor.HataOlustu -= Motor_HataOlustu;
    }

    private void Motor_MetinTanindi(object? sender, string metin) => ArayuzdeCalistir(() =>
    {
        // Tanınan metin, metin alanının sonuna eklenir.
        if (rtbTaninan.TextLength > 0 && !rtbTaninan.Text.EndsWith(' ') && !rtbTaninan.Text.EndsWith('\n'))
            rtbTaninan.AppendText(" ");
        rtbTaninan.AppendText(metin);
        rtbTaninan.ScrollToCaret();

        lstSonuclar.Items.Add($"[{DateTime.Now:HH:mm:ss}] {metin}");
        lstSonuclar.TopIndex = lstSonuclar.Items.Count - 1;
        lblAraSonuc.Text = "";
    });

    private void Motor_AraSonuc(object? sender, string metin) =>
        ArayuzdeCalistir(() => lblAraSonuc.Text = string.IsNullOrEmpty(metin) ? "" : "… " + metin);

    private void Motor_SesSeviyesi(object? sender, int seviye) =>
        ArayuzdeCalistir(() => prgSeviye.Value = Math.Clamp(seviye, 0, 100));

    private void Motor_HataOlustu(object? sender, string mesaj) => ArayuzdeCalistir(() =>
    {
        DinlemeyiDurdur();
        HataGoster(mesaj);
    });

    private async void btnModelIndir_Click(object? sender, EventArgs e)
    {
        if (cmbMotor.SelectedItem is not MotorSecenegi { IndirilecekModel: { } model }) return;

        var cevap = MessageBox.Show(
            $"\"{model.Ad}\" Vosk konuşma modeli indirilecek (~{model.YaklasikBoyutMb} MB).\n\n" +
            $"Kaynak: {model.Adres}\nHedef: {ModelYoneticisi.ModelKlasoru}\n\nDevam edilsin mi?",
            "Model İndir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (cevap != DialogResult.Yes) return;

        _indirmeIptal = new CancellationTokenSource();
        btnModelIndir.Enabled = false;
        btnDinle.Enabled = false;
        var ilerleme = new Progress<int>(p => DurumYaz($"⬇  MODEL İNDİRİLİYOR... %{p}", Color.DarkCyan));
        try
        {
            await ModelYoneticisi.IndirAsync(model, ilerleme, _indirmeIptal.Token);
            MotorlariYukle();
            MessageBox.Show("Model indirildi ve kullanıma hazır.", "Tamamlandı",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (HttpRequestException ex)
        {
            HataGoster("Model indirilemedi. İnternet bağlantınızı kontrol edin.\n" + ex.Message);
        }
        catch (Exception ex) when (ex is IOException or InvalidDataException or UnauthorizedAccessException or TaskCanceledException)
        {
            HataGoster("Model indirilemedi: " + ex.Message);
        }
        finally
        {
            _indirmeIptal?.Dispose();
            _indirmeIptal = null;
            cmbMotor_SelectedIndexChanged(null, EventArgs.Empty);
        }
    }

    private void btnTemizleStt_Click(object? sender, EventArgs e)
    {
        rtbTaninan.Clear();
        lstSonuclar.Items.Clear();
        lblAraSonuc.Text = "";
    }

    private void btnTxtKaydet_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(rtbTaninan.Text))
        {
            MessageBox.Show("Kaydedilecek metin yok.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dlg = new SaveFileDialog
        {
            Filter = "Metin Dosyası (*.txt)|*.txt",
            FileName = $"konusma_metni_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            using var yazici = new StreamWriter(dlg.FileName, false, Encoding.UTF8);
            yazici.WriteLine(rtbTaninan.Text);
            if (lstSonuclar.Items.Count > 0)
            {
                yazici.WriteLine();
                yazici.WriteLine("--- Zaman damgalı sonuçlar ---");
                foreach (var satir in lstSonuclar.Items) yazici.WriteLine(satir);
            }
            MessageBox.Show("Metin kaydedildi.", "Kaydedildi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            HataGoster("Dosya kaydedilemedi: " + ex.Message);
        }
    }

    /// <summary>Tanınan metni Text-to-Speech alanına aktarır ve sesli okur.</summary>
    private void btnOkumayaAktar_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(rtbTaninan.Text)) return;
        rtbMetin.Text = rtbTaninan.Text;
        btnOku.PerformClick();
    }

    // =================================================================
    // DURUM GÖSTERGESİ
    // =================================================================

    /// <summary>Hangi işlemin aktif olduğunu üstteki göstergede ve butonlarda gösterir.</summary>
    private void DurumuGuncelle()
    {
        var tts = _seslendirici.Durum;
        bool dinliyor = _aktifTanima?.Dinliyor == true;
        bool indiriliyor = _indirmeIptal != null;

        // TTS butonları
        btnOku.Enabled = tts is SeslendirmeDurumu.Hazir;
        btnDuraklat.Enabled = tts is SeslendirmeDurumu.Okuyor or SeslendirmeDurumu.Duraklatildi;
        btnDuraklat.Text = tts == SeslendirmeDurumu.Duraklatildi ? "▶ Devam" : "⏸ Duraklat";
        btnDurdur.Enabled = tts is SeslendirmeDurumu.Okuyor or SeslendirmeDurumu.Duraklatildi;
        if (tts == SeslendirmeDurumu.Okuyor) tmrIlerleme.Start();
        else if (tts == SeslendirmeDurumu.Hazir) { tmrIlerleme.Stop(); prgOkuma.Value = 0; }

        // STT butonları
        btnDinle.Enabled = !dinliyor && !_modelYukleniyor && !indiriliyor;
        btnDinlemeDurdur.Enabled = dinliyor;
        cmbMotor.Enabled = cmbMikrofon.Enabled = !dinliyor && !_modelYukleniyor && !indiriliyor;

        grpTts.BackColor = tts != SeslendirmeDurumu.Hazir ? Color.FromArgb(232, 240, 254) : SystemColors.Control;
        grpStt.BackColor = dinliyor ? Color.FromArgb(253, 236, 234) : SystemColors.Control;

        if (indiriliyor) return; // indirme ilerlemesi kendi metnini yazar

        if (dinliyor && tts == SeslendirmeDurumu.Okuyor)
            DurumYaz("🎤 DİNLENİYOR  +  🔊 METİN OKUNUYOR", Color.Purple);
        else if (dinliyor)
            DurumYaz($"🎤  MİKROFON DİNLENİYOR — konuşabilirsiniz  ({_aktifTanima!.Ad})", Color.FromArgb(198, 40, 40));
        else if (_modelYukleniyor)
            DurumYaz("⏳  KONUŞMA MODELİ YÜKLENİYOR...", Color.DarkOrange);
        else if (tts == SeslendirmeDurumu.Hazirlaniyor)
            DurumYaz("⏳  SES HAZIRLANIYOR...", Color.DarkOrange);
        else if (tts == SeslendirmeDurumu.Okuyor)
            DurumYaz("🔊  METİN SESLİ OKUNUYOR", Color.FromArgb(25, 118, 210));
        else if (tts == SeslendirmeDurumu.Duraklatildi)
            DurumYaz("⏸  OKUMA DURAKLATILDI", Color.FromArgb(245, 124, 0));
        else
            DurumYaz("●  HAZIR — aktif işlem yok", Color.Gray);
    }

    private void DurumYaz(string metin, Color renk)
    {
        lblDurum.Text = metin;
        lblDurum.BackColor = renk;
    }

    /// <summary>
    /// Klavye kısayolları: F5 = Oku, F6 = Duraklat/Devam, F7 = Okumayı durdur,
    /// F9 = Dinlemeyi başlat, F10 = Dinlemeyi durdur.
    /// </summary>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        Button? hedef = keyData switch
        {
            Keys.F5 => btnOku,
            Keys.F6 => btnDuraklat,
            Keys.F7 => btnDurdur,
            Keys.F9 => btnDinle,
            Keys.F10 => btnDinlemeDurdur,
            _ => null
        };
        if (hedef == null) return base.ProcessCmdKey(ref msg, keyData);
        if (hedef.Enabled) hedef.PerformClick();
        return true;
    }

    private void HataGoster(string mesaj) =>
        MessageBox.Show(this, mesaj, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
}
