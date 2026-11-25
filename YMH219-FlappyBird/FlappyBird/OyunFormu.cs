using System.Drawing.Drawing2D;
using System.Drawing.Text;
using FlappyBird.Oyun;

namespace FlappyBird;

public partial class OyunFormu : Form
{
    private OyunMotoru _motor = null!;
    private readonly SkorKaydedici _skorKaydedici = new();
    private readonly SesEfektleri _ses = new();
    private int _sonGosterilenSkor;

    // Arka plandaki bulutlar için sabit konumlar
    private readonly PointF[] _bulutlar = { new(80, 60), new(330, 110), new(560, 45), new(720, 140) };

    public OyunFormu()
    {
        InitializeComponent();
        cmbZorluk.Items.AddRange(Enum.GetNames<Zorluk>());
        chkSes.CheckedChanged += (_, _) => _ses.Acik = chkSes.Checked;
    }

    private void OyunFormu_Load(object? sender, EventArgs e)
    {
        _motor = new OyunMotoru(oyunPaneli.ClientSize.Width, oyunPaneli.ClientSize.Height, _skorKaydedici.Oku());
        _motor.SkorDegisti += Motor_SkorDegisti;
        _motor.OyunBitti += Motor_OyunBitti;

        cmbZorluk.SelectedItem = Zorluk.Orta.ToString(); // YeniOyun'u tetikler
        oyunZamanlayici.Start();
    }

    private void OyunFormu_FormClosing(object? sender, FormClosingEventArgs e)
    {
        oyunZamanlayici.Stop();
        _ses.Dispose();
    }

    // ---------------------------------------------------------------
    // Oyun döngüsü
    // ---------------------------------------------------------------

    private void oyunZamanlayici_Tick(object? sender, EventArgs e)
    {
        _motor.Guncelle();
        oyunPaneli.Invalidate(); // Paint olayını tetikler
    }

    private void Motor_SkorDegisti(object? sender, EventArgs e)
    {
        if (_motor.Skor > _sonGosterilenSkor) _ses.Puan();
        _sonGosterilenSkor = _motor.Skor;
        BilgileriGuncelle();
    }

    private void Motor_OyunBitti(object? sender, EventArgs e)
    {
        _ses.Carpma();
        if (_motor.Skor > 0 && _motor.Skor >= _motor.EnYuksekSkor)
            _skorKaydedici.Yaz(_motor.EnYuksekSkor);
        BilgileriGuncelle();
    }

    private void BilgileriGuncelle()
    {
        lblSkor.Text = $"Skor: {_motor.Skor}";
        lblEnYuksek.Text = $"En Yüksek: {_motor.EnYuksekSkor}";
        lblSeviye.Text = $"Seviye: {_motor.Seviye}  Hız: {_motor.OyunHizi:0.0}";
        btnDuraklat.Text = _motor.Durum == OyunDurumu.Duraklatildi ? "Devam (P)" : "Duraklat (P)";
    }

    private void YeniOyunBaslat()
    {
        var zorluk = Enum.TryParse<Zorluk>(cmbZorluk.SelectedItem as string, out var z) ? z : Zorluk.Orta;
        _sonGosterilenSkor = 0;
        _motor.YeniOyun(zorluk);
        BilgileriGuncelle();
        oyunPaneli.Invalidate();
    }

    // ---------------------------------------------------------------
    // Klavye ve fare olayları
    // ---------------------------------------------------------------

    private void OyunFormu_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Space:
            case Keys.Up:
            case Keys.W:
                ZiplaVeyaYenidenBasla();
                break;
            case Keys.P:
            case Keys.Escape:
                Duraklat();
                break;
            case Keys.R:
            case Keys.Enter:
                YeniOyunBaslat();
                break;
            default:
                return;
        }

        // Tuşun butonlara / ComboBox'a gitmesini engelle.
        e.Handled = true;
        e.SuppressKeyPress = true;
    }

    private void OyunFormu_KeyUp(object? sender, KeyEventArgs e)
    {
        // Space tuşu bırakıldığında odaktaki butonun tıklanmasını engeller.
        if (e.KeyCode is Keys.Space or Keys.Up or Keys.Enter)
            e.Handled = true;
    }

    private void oyunPaneli_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left) ZiplaVeyaYenidenBasla();
    }

    private void ZiplaVeyaYenidenBasla()
    {
        if (_motor.Durum == OyunDurumu.Bitti)
        {
            YeniOyunBaslat();
            return;
        }
        if (_motor.Durum is OyunDurumu.Baslamadi or OyunDurumu.Oynaniyor)
        {
            _motor.Zipla();
            _ses.Zipla();
        }
    }

    private void Duraklat()
    {
        _motor.DuraklatDevamEt();
        BilgileriGuncelle();
        oyunPaneli.Invalidate();
    }

    private void btnYeniOyun_Click(object? sender, EventArgs e)
    {
        YeniOyunBaslat();
        oyunPaneli.Focus();
    }

    private void btnDuraklat_Click(object? sender, EventArgs e) => Duraklat();

    private void cmbZorluk_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_motor == null) return;
        YeniOyunBaslat();
        ActiveControl = null; // klavye odağını ComboBox'tan al
    }

    // ---------------------------------------------------------------
    // Çizim
    // ---------------------------------------------------------------

    private void oyunPaneli_Paint(object? sender, PaintEventArgs e)
    {
        if (_motor == null) return;
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = TextRenderingHint.AntiAlias;

        ArkaPlaniCiz(g);
        foreach (var boru in _motor.Borular)
            boru.Ciz(g);
        ZeminiCiz(g);
        _motor.Kus.Ciz(g);

        // Oyun içi büyük skor
        MetniOrtala(g, _motor.Skor.ToString(), 36, 18, Color.White);

        switch (_motor.Durum)
        {
            case OyunDurumu.Baslamadi:
                BilgiKutusu(g, "HAZIR MISIN?", "Başlamak için SPACE / ↑ tuşuna bas veya tıkla",
                    $"Zorluk: {_motor.Zorluk}");
                break;
            case OyunDurumu.Duraklatildi:
                BilgiKutusu(g, "DURAKLATILDI", "Devam etmek için P tuşuna bas", "");
                break;
            case OyunDurumu.Bitti:
                BilgiKutusu(g, "GAME OVER", $"Skor: {_motor.Skor}    En Yüksek: {_motor.EnYuksekSkor}",
                    "Yeniden başlamak için SPACE / R tuşuna bas");
                break;
        }
    }

    private void ArkaPlaniCiz(Graphics g)
    {
        var alan = new RectangleF(0, 0, _motor.AlanGenisligi, _motor.OyunAlaniYuksekligi);
        using (var gokyuzu = new LinearGradientBrush(alan, Color.FromArgb(78, 192, 232), Color.FromArgb(190, 235, 250),
                   LinearGradientMode.Vertical))
        {
            g.FillRectangle(gokyuzu, alan);
        }

        using var bulut = new SolidBrush(Color.FromArgb(230, 255, 255, 255));
        foreach (var b in _bulutlar)
        {
            g.FillEllipse(bulut, b.X, b.Y, 70, 30);
            g.FillEllipse(bulut, b.X + 25, b.Y - 15, 60, 40);
            g.FillEllipse(bulut, b.X + 55, b.Y, 60, 30);
        }
    }

    private void ZeminiCiz(Graphics g)
    {
        float y = _motor.OyunAlaniYuksekligi;
        float w = _motor.AlanGenisligi;
        g.FillRectangle(Brushes.SandyBrown, 0, y, w, _motor.ZeminYuksekligi);
        g.FillRectangle(Brushes.ForestGreen, 0, y, w, 14);

        // Kayan çizgiler, hareket hissi verir.
        using var kalem = new Pen(Color.FromArgb(60, 140, 40), 4);
        for (float x = -_motor.ZeminKaymasi; x < w; x += 40)
            g.DrawLine(kalem, x, y + 14, x + 20, y + 2);
    }

    private void MetniOrtala(Graphics g, string metin, float boyut, float y, Color renk)
    {
        using var font = new Font("Segoe UI", boyut, FontStyle.Bold);
        var olcu = g.MeasureString(metin, font);
        float x = (_motor.AlanGenisligi - olcu.Width) / 2;
        using var golge = new SolidBrush(Color.FromArgb(120, 0, 0, 0));
        using var firca = new SolidBrush(renk);
        g.DrawString(metin, font, golge, x + 3, y + 3);
        g.DrawString(metin, font, firca, x, y);
    }

    private void BilgiKutusu(Graphics g, string baslik, string satir1, string satir2)
    {
        var kutu = new RectangleF(_motor.AlanGenisligi / 2 - 230, _motor.OyunAlaniYuksekligi / 2 - 85, 460, 170);
        using (var arka = new SolidBrush(Color.FromArgb(190, 20, 24, 32)))
            g.FillRectangle(arka, kutu);
        g.DrawRectangle(Pens.White, kutu.X, kutu.Y, kutu.Width, kutu.Height);

        Color baslikRengi = _motor.Durum == OyunDurumu.Bitti ? Color.OrangeRed : Color.Gold;
        MetniOrtala(g, baslik, 28, kutu.Y + 15, baslikRengi);
        MetniOrtala(g, satir1, 12, kutu.Y + 80, Color.White);
        if (!string.IsNullOrEmpty(satir2))
            MetniOrtala(g, satir2, 11, kutu.Y + 115, Color.LightGray);
    }
}
