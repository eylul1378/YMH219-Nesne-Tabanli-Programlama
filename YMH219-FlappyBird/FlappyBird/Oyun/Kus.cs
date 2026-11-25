using System.Drawing.Drawing2D;

namespace FlappyBird.Oyun;

/// <summary>
/// Oyuncunun kontrol ettiği kuş. Klavye ile yukarı zıplar, yer çekimi ile aşağı düşer.
/// </summary>
public class Kus : OyunNesnesi
{
    public const float YerCekimi = 0.55f;
    public const float ZiplamaGucu = -8.5f;
    public const float MaksimumDususHizi = 11f;

    /// <summary>Kuşun dikey hızı (pozitif = aşağı).</summary>
    public float DikeyHiz { get; private set; }

    public Kus(float x, float y) : base(x, y, 38, 28) { }

    /// <summary>Kuşa yukarı doğru anlık hız verir.</summary>
    public void Zipla() => DikeyHiz = ZiplamaGucu;

    /// <summary>Kuşu başlangıç konumuna alır.</summary>
    public void Sifirla(float y)
    {
        Y = y;
        DikeyHiz = 0;
    }

    public override void Guncelle(float hiz)
    {
        // Yer çekimi her karede dikey hızı artırır.
        DikeyHiz = Math.Min(DikeyHiz + YerCekimi, MaksimumDususHizi);
        Y += DikeyHiz;
        if (Y < 0)
        {
            Y = 0;
            DikeyHiz = 0;
        }
    }

    /// <summary>Çarpışma kutusu, kuşun görselinden biraz küçük tutulur (daha adil oyun için).</summary>
    public override RectangleF Sinirlar => new(X + 3, Y + 3, Genislik - 6, Yukseklik - 6);

    public override void Ciz(Graphics g)
    {
        var durum = g.Save();
        // Kuş, hızına göre hafifçe yukarı/aşağı eğilir.
        float aci = Math.Clamp(DikeyHiz * 4f, -25f, 70f);
        g.TranslateTransform(X + Genislik / 2, Y + Yukseklik / 2);
        g.RotateTransform(aci);
        g.TranslateTransform(-Genislik / 2, -Yukseklik / 2);

        // Gövde
        using (var govde = new LinearGradientBrush(new RectangleF(0, 0, Genislik, Yukseklik),
                   Color.Gold, Color.Orange, LinearGradientMode.Vertical))
        {
            g.FillEllipse(govde, 0, 0, Genislik, Yukseklik);
        }
        g.DrawEllipse(Pens.SaddleBrown, 0, 0, Genislik, Yukseklik);

        // Kanat
        g.FillEllipse(Brushes.LightYellow, 4, Yukseklik / 2 - 2, 16, 10);
        g.DrawEllipse(Pens.SaddleBrown, 4, Yukseklik / 2 - 2, 16, 10);

        // Göz
        g.FillEllipse(Brushes.White, Genislik - 16, 3, 12, 12);
        g.FillEllipse(Brushes.Black, Genislik - 10, 7, 5, 5);

        // Gaga
        PointF[] gaga = { new(Genislik - 4, 14), new(Genislik + 8, 17), new(Genislik - 4, 21) };
        g.FillPolygon(Brushes.OrangeRed, gaga);

        g.Restore(durum);
    }
}
