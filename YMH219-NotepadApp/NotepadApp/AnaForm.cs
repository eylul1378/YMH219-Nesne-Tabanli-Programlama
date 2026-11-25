using NotepadApp.Models;
using NotepadApp.Services;

namespace NotepadApp;

public partial class AnaForm : Form
{
    private readonly NotDeposu _depo;

    /// <summary>Editörde açık olan not. null ise yeni (henüz kaydedilmemiş) not yazılıyor demektir.</summary>
    private Not? _seciliNot;

    /// <summary>Editördeki metin değişti mi?</summary>
    private bool _editorDegisti;

    /// <summary>Bellekteki notlar dosyaya yazılmamış değişiklik içeriyor mu?</summary>
    private bool _dosyaDegisti;

    /// <summary>Program kodu kontrolleri doldururken olayların tetiklenmesini engeller.</summary>
    private bool _yukleniyor;

    public AnaForm()
    {
        InitializeComponent();
        _depo = new NotDeposu(NotDeposu.VarsayilanDosyaYolu());
    }

    // ---------------------------------------------------------------
    // Form açılış / kapanış
    // ---------------------------------------------------------------

    private void AnaForm_Load(object? sender, EventArgs e)
    {
        try
        {
            _depo.Yukle();
            DurumYaz($"{_depo.Notlar.Count} not dosyadan yüklendi.");
        }
        catch (FileNotFoundException)
        {
            // İlk çalıştırmada dosya yoktur; bu bir hata değildir.
            DurumYaz("Kayıtlı not dosyası bulunamadı, yeni bir dosya oluşturulacak.");
        }
        catch (InvalidDataException)
        {
            string yedek = _depo.BozukDosyayiYedekle();
            MessageBox.Show(
                "Not dosyası bozuk olduğu için okunamadı.\n\n" +
                $"Eski dosya şu adla yedeklendi:\n{yedek}\n\nBoş bir not listesiyle devam ediliyor.",
                "Bozuk Dosya", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            DurumYaz("Bozuk dosya yedeklendi, boş liste ile başlatıldı.");
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            MessageBox.Show("Not dosyası okunurken hata oluştu:\n" + ex.Message,
                "Dosya Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        ListeyiYenile();
        EditoruTemizle();
    }

    private void AnaForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (!KaydedilmemisDegisiklikleriSor())
        {
            e.Cancel = true;
            return;
        }

        if (_dosyaDegisti)
        {
            var cevap = MessageBox.Show("Notlarda dosyaya kaydedilmemiş değişiklikler var. Kaydedilsin mi?",
                "Çıkış", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (cevap == DialogResult.Cancel) e.Cancel = true;
            else if (cevap == DialogResult.Yes && !DosyayaKaydet()) e.Cancel = true;
        }
    }

    // ---------------------------------------------------------------
    // Liste ve editör yardımcıları
    // ---------------------------------------------------------------

    private void ListeyiYenile(Guid? secilecekId = null)
    {
        _yukleniyor = true;
        try
        {
            var notlar = _depo.Ara(txtAra.Text);
            lstNotlar.BeginUpdate();
            lstNotlar.Items.Clear();
            foreach (var not in notlar)
                lstNotlar.Items.Add(not);
            lstNotlar.EndUpdate();

            Guid? hedef = secilecekId ?? _seciliNot?.Id;
            if (hedef != null)
            {
                for (int i = 0; i < lstNotlar.Items.Count; i++)
                {
                    if (((Not)lstNotlar.Items[i]).Id == hedef)
                    {
                        lstNotlar.SelectedIndex = i;
                        break;
                    }
                }
            }

            lblNotSayisi.Text = string.IsNullOrWhiteSpace(txtAra.Text)
                ? $"{_depo.Notlar.Count} not"
                : $"{notlar.Count} / {_depo.Notlar.Count} not gösteriliyor";

            // Kategori önerileri
            string mevcut = cmbKategori.Text;
            cmbKategori.Items.Clear();
            cmbKategori.Items.Add("Genel");
            foreach (var k in _depo.Kategoriler())
                if (!cmbKategori.Items.Contains(k)) cmbKategori.Items.Add(k);
            cmbKategori.Text = mevcut;

            lblDosya.Text = _depo.DosyaYolu;
        }
        finally
        {
            _yukleniyor = false;
        }
    }

    private void NotuEditoreYukle(Not not)
    {
        _yukleniyor = true;
        try
        {
            _seciliNot = not;
            txtBaslik.Text = not.Baslik;
            cmbKategori.Text = not.Kategori;

            if (!string.IsNullOrEmpty(not.IcerikRtf))
            {
                try { rtbIcerik.Rtf = not.IcerikRtf; }
                catch (ArgumentException) { rtbIcerik.Text = not.Icerik; } // RTF bozuksa düz metne dön
            }
            else
            {
                rtbIcerik.Text = not.Icerik;
            }

            TarihleriGoster(not);
            _editorDegisti = false;
            BaslikCubugunuGuncelle();
        }
        finally
        {
            _yukleniyor = false;
        }
    }

    private void EditoruTemizle()
    {
        _yukleniyor = true;
        try
        {
            _seciliNot = null;
            txtBaslik.Clear();
            cmbKategori.Text = "Genel";
            rtbIcerik.Clear();
            lblTarihler.Text = "Yeni not (henüz kaydedilmedi)";
            lstNotlar.ClearSelected();
            _editorDegisti = false;
            BaslikCubugunuGuncelle();
            txtBaslik.Focus();
        }
        finally
        {
            _yukleniyor = false;
        }
    }

    private void TarihleriGoster(Not not)
    {
        lblTarihler.Text =
            $"Oluşturma: {not.OlusturmaTarihi:dd.MM.yyyy HH:mm:ss}     " +
            $"Son düzenleme: {not.DuzenlemeTarihi:dd.MM.yyyy HH:mm:ss}";
    }

    private void BaslikCubugunuGuncelle()
    {
        string ad = _seciliNot?.Baslik ?? "Yeni Not";
        Text = $"Not Defteri - YMH219  |  {ad}{(_editorDegisti ? " *" : "")}";
    }

    private void DurumYaz(string mesaj)
    {
        lblDurum.Text = $"[{DateTime.Now:HH:mm:ss}] {mesaj}";
    }

    /// <summary>
    /// Editörde kaydedilmemiş değişiklik varsa kullanıcıya sorar.
    /// İşleme devam edilebilecekse true döner.
    /// </summary>
    private bool KaydedilmemisDegisiklikleriSor()
    {
        if (!_editorDegisti) return true;

        var cevap = MessageBox.Show("Açık notta kaydedilmemiş değişiklikler var. Kaydedilsin mi?",
            "Kaydedilmemiş Değişiklik", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

        if (cevap == DialogResult.Cancel) return false;
        if (cevap == DialogResult.Yes) return NotuKaydet();

        _editorDegisti = false;
        return true;
    }

    // ---------------------------------------------------------------
    // Not işlemleri (ekle / düzenle / sil)
    // ---------------------------------------------------------------

    /// <summary>Editördeki notu listeye ekler veya mevcut notu günceller.</summary>
    private bool NotuKaydet()
    {
        string baslik = txtBaslik.Text.Trim();
        if (string.IsNullOrEmpty(baslik))
        {
            MessageBox.Show("Lütfen not için bir başlık girin.", "Eksik Bilgi",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtBaslik.Focus();
            return false;
        }

        string kategori = cmbKategori.Text.Trim();

        if (_seciliNot == null)
        {
            var yeni = new Not(baslik, rtbIcerik.Text, kategori) { IcerikRtf = rtbIcerik.Rtf ?? string.Empty };
            _depo.Ekle(yeni);
            _seciliNot = yeni;
            DurumYaz($"\"{baslik}\" notu eklendi.");
        }
        else
        {
            _seciliNot.Guncelle(baslik, rtbIcerik.Text, rtbIcerik.Rtf ?? string.Empty, kategori);
            DurumYaz($"\"{baslik}\" notu güncellendi.");
        }

        _editorDegisti = false;
        _dosyaDegisti = true;
        TarihleriGoster(_seciliNot);
        BaslikCubugunuGuncelle();

        if (chkOtomatikKaydet.Checked)
            DosyayaKaydet();

        ListeyiYenile(_seciliNot.Id);
        return true;
    }

    /// <summary>Tüm notları JSON dosyasına yazar. Başarılıysa true döner.</summary>
    private bool DosyayaKaydet(string? yol = null)
    {
        try
        {
            _depo.Kaydet(yol);
            _dosyaDegisti = false;
            lblDosya.Text = _depo.DosyaYolu;
            DurumYaz($"{_depo.Notlar.Count} not dosyaya kaydedildi.");
            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or NotSupportedException)
        {
            MessageBox.Show("Notlar dosyaya kaydedilemedi:\n" + ex.Message, "Kaydetme Hatası",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            DurumYaz("Kaydetme başarısız!");
            return false;
        }
    }

    private void btnYeni_Click(object? sender, EventArgs e)
    {
        if (!KaydedilmemisDegisiklikleriSor()) return;
        EditoruTemizle();
        DurumYaz("Yeni not oluşturuluyor.");
    }

    private void btnKaydet_Click(object? sender, EventArgs e) => NotuKaydet();

    private void btnSil_Click(object? sender, EventArgs e)
    {
        if (_seciliNot == null)
        {
            MessageBox.Show("Silmek için listeden bir not seçin.", "Bilgi",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var cevap = MessageBox.Show($"\"{_seciliNot.Baslik}\" notu silinsin mi?", "Silme Onayı",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (cevap != DialogResult.Yes) return;

        string baslik = _seciliNot.Baslik;
        _depo.Sil(_seciliNot.Id);
        _dosyaDegisti = true;
        EditoruTemizle();
        if (chkOtomatikKaydet.Checked) DosyayaKaydet();
        ListeyiYenile();
        DurumYaz($"\"{baslik}\" notu silindi.");
    }

    private void btnDisaAktar_Click(object? sender, EventArgs e)
    {
        if (_seciliNot == null || _editorDegisti)
        {
            MessageBox.Show("Dışa aktarmadan önce notu kaydedin veya listeden bir not seçin.", "Bilgi",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dlg = new SaveFileDialog
        {
            Title = "Notu .txt Olarak Dışa Aktar",
            Filter = "Metin Dosyası (*.txt)|*.txt",
            FileName = TxtDisaAktarici.OneriDosyaAdi(_seciliNot)
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            TxtDisaAktarici.DisaAktar(_seciliNot, dlg.FileName);
            DurumYaz($"Not dışa aktarıldı: {dlg.FileName}");
            MessageBox.Show("Not başarıyla .txt dosyasına aktarıldı.", "Dışa Aktarma",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            MessageBox.Show("Dışa aktarma başarısız:\n" + ex.Message, "Hata",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ---------------------------------------------------------------
    // Liste, arama ve editör olayları
    // ---------------------------------------------------------------

    private void lstNotlar_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_yukleniyor) return;
        if (lstNotlar.SelectedItem is not Not secilen || secilen == _seciliNot) return;

        if (!KaydedilmemisDegisiklikleriSor())
        {
            // Kullanıcı iptal etti: seçimi eski nota geri al.
            ListeyiYenile();
            return;
        }

        NotuEditoreYukle(secilen);
        DurumYaz($"\"{secilen.Baslik}\" notu açıldı.");
    }

    private void txtAra_TextChanged(object? sender, EventArgs e) => ListeyiYenile();

    private void Editor_TextChanged(object? sender, EventArgs e)
    {
        if (_yukleniyor) return;
        _editorDegisti = true;
        BaslikCubugunuGuncelle();
    }

    // ---------------------------------------------------------------
    // Menü: dosya işlemleri
    // ---------------------------------------------------------------

    private void mnuKaydet_Click(object? sender, EventArgs e)
    {
        if (_editorDegisti && !NotuKaydet()) return;
        DosyayaKaydet();
    }

    private void mnuFarkliKaydet_Click(object? sender, EventArgs e)
    {
        using var dlg = new SaveFileDialog
        {
            Title = "Notları Farklı Kaydet",
            Filter = "JSON Dosyası (*.json)|*.json",
            FileName = "notlar.json"
        };
        if (dlg.ShowDialog(this) == DialogResult.OK)
            DosyayaKaydet(dlg.FileName);
    }

    private void mnuYukle_Click(object? sender, EventArgs e)
    {
        if (!KaydedilmemisDegisiklikleriSor()) return;

        using var dlg = new OpenFileDialog
        {
            Title = "Not Dosyası Yükle",
            Filter = "JSON Dosyası (*.json)|*.json|Tüm Dosyalar (*.*)|*.*"
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            _depo.Yukle(dlg.FileName);
            _dosyaDegisti = false;
            EditoruTemizle();
            ListeyiYenile();
            DurumYaz($"{_depo.Notlar.Count} not yüklendi: {dlg.FileName}");
        }
        catch (FileNotFoundException)
        {
            MessageBox.Show("Seçilen dosya bulunamadı.", "Dosya Bulunamadı",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (InvalidDataException)
        {
            MessageBox.Show("Seçilen dosya geçerli bir not dosyası değil (JSON biçimi bozuk).",
                "Geçersiz Dosya", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            MessageBox.Show("Dosya okunamadı:\n" + ex.Message, "Hata",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void mnuCikis_Click(object? sender, EventArgs e) => Close();

    // ---------------------------------------------------------------
    // Menü: biçimlendirme (RichTextBox)
    // ---------------------------------------------------------------

    private void StilDegistir(FontStyle stil)
    {
        Font mevcut = rtbIcerik.SelectionFont ?? rtbIcerik.Font;
        FontStyle yeniStil = mevcut.Style ^ stil; // seçili stili aç/kapat
        rtbIcerik.SelectionFont = new Font(mevcut, yeniStil);
        rtbIcerik.Focus();
    }

    private void mnuKalin_Click(object? sender, EventArgs e) => StilDegistir(FontStyle.Bold);
    private void mnuItalik_Click(object? sender, EventArgs e) => StilDegistir(FontStyle.Italic);
    private void mnuAltiCizili_Click(object? sender, EventArgs e) => StilDegistir(FontStyle.Underline);

    private void mnuYaziTipi_Click(object? sender, EventArgs e)
    {
        fontDialog1.Font = rtbIcerik.SelectionFont ?? rtbIcerik.Font;
        if (fontDialog1.ShowDialog(this) == DialogResult.OK)
            rtbIcerik.SelectionFont = fontDialog1.Font;
    }

    private void mnuHakkinda_Click(object? sender, EventArgs e)
    {
        MessageBox.Show(
            "Not Defteri Uygulaması\nYMH 219 - Nesne Tabanlı Programlama\n\n" +
            "Notlar JSON biçiminde şu dosyada saklanır:\n" + _depo.DosyaYolu +
            "\n\nNihal Eylül İl - 250541001",
            "Hakkında", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
