using System.Drawing.Drawing2D;

namespace FlappyBird.Oyun;

/// <summary>
/// Üst ve alt parçadan oluşan engel (boru çifti). Sağdan sola hareket eder,
/// ekran dışına çıkınca yeniden sağa konumlandırılır.
/// </summary>
public class Boru : OyunNesnesi
{
    public const float BoruGenisligi = 70f;
    private const float MinimumParcaYuksekligi = 50f;

    private readonly float _alanYuksekligi;

    /// <summary>Kuşun geçebileceği boşluğun üst kenarının Y değeri.</summary>
    public float BoslukY { get; private set; }

    /// <summary>Kuşun geçebileceği boşluğun yüksekliği.</summary>
    public float BoslukYuksekligi { get; private set; }

    /// <summary>Kuş bu boruyu geçtiğinde true olur; aynı boru için iki kez puan verilmez.</summary>
    public bool Gecildi { get; set; }

    public RectangleF UstParca => new(X, 0, Genislik, BoslukY);
    public RectangleF AltParca => new(X, BoslukY + BoslukYuksekligi, Genislik, _alanYuksekligi - (BoslukY + BoslukYuksekligi));

    public Boru(float x, float alanYuksekligi, float boslukYuksekligi, Random rastgele)
        : base(x, 0, BoruGenisligi, alanYuksekligi)
    {
        _alanYuksekligi = alanYuksekligi;
        YenidenKonumlandir(x, boslukYuksekligi, rastgele);
    }

    /// <summary>Boruyu verilen X konumuna taşır ve boşluğu rastgele bir yüksekliğe yerleştirir.</summary>
    public void YenidenKonumlandir(float x, float boslukYuksekligi, Random rastgele)
    {
        X = x;
        BoslukYuksekligi = boslukYuksekligi;
        float enAz = MinimumParcaYuksekligi;
        float enCok = _alanYuksekligi - boslukYuksekligi - MinimumParcaYuksekligi;
        BoslukY = enAz + (float)rastgele.NextDouble() * Math.Max(0, enCok - enAz);
        Gecildi = false;
    }

    /// <summary>Boru tamamen ekranın solundan çıktı mı?</summary>
    public bool EkranDisinda => X + Genislik < 0;

    public override void Guncelle(float hiz) => X -= hiz;

    /// <summary>Verilen dikdörtgen (kuş) borunun üst veya alt parçasına çarpıyor mu?</summary>
    public bool CarpismaVarMi(RectangleF hedef) =>
        hedef.IntersectsWith(UstParca) || hedef.IntersectsWith(AltParca);

    public override void Ciz(Graphics g)
    {
        ParcaCiz(g, UstParca, ustte: true);
        ParcaCiz(g, AltParca, ustte: false);
    }

    private static void ParcaCiz(Graphics g, RectangleF r, bool ustte)
    {
        if (r.Height <= 0) return;

        using var firca = new LinearGradientBrush(r, Color.FromArgb(90, 190, 60), Color.FromArgb(40, 120, 30),
            LinearGradientMode.Horizontal);
        g.FillRectangle(firca, r);
        g.DrawRectangle(Pens.DarkGreen, r.X, r.Y, r.Width, r.Height);

        // Borunun ağız kısmı
        const float agizYuksekligi = 22f;
        var agiz = new RectangleF(r.X - 5, ustte ? r.Bottom - agizYuksekligi : r.Y, r.Width + 10, agizYuksekligi);
        using var agizFirca = new LinearGradientBrush(agiz, Color.FromArgb(110, 210, 70), Color.FromArgb(50, 140, 35),
            LinearGradientMode.Horizontal);
        g.FillRectangle(agizFirca, agiz);
        g.DrawRectangle(Pens.DarkGreen, agiz.X, agiz.Y, agiz.Width, agiz.Height);
    }
}
