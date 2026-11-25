namespace FlappyBird.Oyun;

public enum OyunDurumu
{
    Baslamadi,
    Oynaniyor,
    Duraklatildi,
    Bitti
}

public enum Zorluk
{
    Kolay,
    Orta,
    Zor
}

/// <summary>
/// Oyunun tüm durumunu (kuş, engeller, skor, hız, seviye) yöneten sınıf.
/// Form yalnızca bu sınıfın metotlarını çağırır ve sonucu çizer.
/// </summary>
public class OyunMotoru
{
    private const int BoruSayisi = 3;
    private const int SeviyeBasinaSkor = 5;
    private const float SeviyeBasinaHizArtisi = 0.6f;
    private const float MaksimumHiz = 10f;

    private readonly Random _rastgele = new();
    private readonly List<Boru> _borular = new();

    public float AlanGenisligi { get; }
    public float AlanYuksekligi { get; }
    public float ZeminYuksekligi { get; } = 70f;

    /// <summary>Oynanabilir alanın yüksekliği (zemin hariç).</summary>
    public float OyunAlaniYuksekligi => AlanYuksekligi - ZeminYuksekligi;

    public Kus Kus { get; }
    public IReadOnlyList<Boru> Borular => _borular;

    public OyunDurumu Durum { get; private set; } = OyunDurumu.Baslamadi;
    public Zorluk Zorluk { get; private set; } = Zorluk.Orta;

    public int Skor { get; private set; }
    public int EnYuksekSkor { get; private set; }
    public int Seviye => Skor / SeviyeBasinaSkor + 1;

    /// <summary>Engellerin her karede sola kayma miktarı (piksel).</summary>
    public float OyunHizi { get; private set; }

    /// <summary>Zeminin kayma efekti için kullanılan konum.</summary>
    public float ZeminKaymasi { get; private set; }

    public event EventHandler? SkorDegisti;
    public event EventHandler? OyunBitti;

    public OyunMotoru(float alanGenisligi, float alanYuksekligi, int enYuksekSkor)
    {
        AlanGenisligi = alanGenisligi;
        AlanYuksekligi = alanYuksekligi;
        EnYuksekSkor = enYuksekSkor;
        Kus = new Kus(alanGenisligi * 0.22f, OyunAlaniYuksekligi / 2);
        YeniOyun(Zorluk);
    }

    private float BaslangicHizi => Zorluk switch
    {
        Zorluk.Kolay => 3.0f,
        Zorluk.Zor => 5.0f,
        _ => 4.0f
    };

    private float BoslukYuksekligi => Zorluk switch
    {
        Zorluk.Kolay => 190f,
        Zorluk.Zor => 135f,
        _ => 160f
    };

    private float BoruAraligi => AlanGenisligi / BoruSayisi + 60;

    /// <summary>Oyunu verilen zorlukta baştan kurar (Yeni Oyun / Yeniden Başlat).</summary>
    public void YeniOyun(Zorluk zorluk)
    {
        Zorluk = zorluk;
        Skor = 0;
        OyunHizi = BaslangicHizi;
        Kus.Sifirla(OyunAlaniYuksekligi / 2 - Kus.Yukseklik / 2);

        _borular.Clear();
        for (int i = 0; i < BoruSayisi; i++)
        {
            float x = AlanGenisligi + 100 + i * BoruAraligi;
            _borular.Add(new Boru(x, OyunAlaniYuksekligi, BoslukYuksekligi, _rastgele));
        }

        Durum = OyunDurumu.Baslamadi;
        SkorDegisti?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Oyuncu zıplama tuşuna bastığında çağrılır.</summary>
    public void Zipla()
    {
        switch (Durum)
        {
            case OyunDurumu.Baslamadi:
                Durum = OyunDurumu.Oynaniyor;
                Kus.Zipla();
                break;
            case OyunDurumu.Oynaniyor:
                Kus.Zipla();
                break;
        }
    }

    public void DuraklatDevamEt()
    {
        if (Durum == OyunDurumu.Oynaniyor) Durum = OyunDurumu.Duraklatildi;
        else if (Durum == OyunDurumu.Duraklatildi) Durum = OyunDurumu.Oynaniyor;
    }

    /// <summary>
    /// Oyun döngüsünün bir adımı. Timer'ın her Tick olayında çağrılır.
    /// </summary>
    public void Guncelle()
    {
        if (Durum != OyunDurumu.Oynaniyor) return;

        Kus.Guncelle(OyunHizi);
        ZeminKaymasi = (ZeminKaymasi + OyunHizi) % 40;

        foreach (var boru in _borular)
        {
            boru.Guncelle(OyunHizi);

            // Ekran dışına çıkan boruyu en sağdaki borunun arkasına yeniden konumlandır.
            if (boru.EkranDisinda)
            {
                float enSagX = _borular.Max(b => b.X);
                boru.YenidenKonumlandir(enSagX + BoruAraligi, BoslukYuksekligi, _rastgele);
            }

            // Kuş borunun sağ kenarını geçtiyse puan ver.
            if (!boru.Gecildi && boru.X + boru.Genislik < Kus.X)
            {
                boru.Gecildi = true;
                SkorArtir();
            }

            if (boru.CarpismaVarMi(Kus.Sinirlar))
            {
                OyunuBitir();
                return;
            }
        }

        // Zemine çarpma kontrolü
        if (Kus.Sinirlar.Bottom >= OyunAlaniYuksekligi)
        {
            OyunuBitir();
        }
    }

    private void SkorArtir()
    {
        int eskiSeviye = Seviye;
        Skor++;

        // Seviye atlandığında hız artar.
        if (Seviye > eskiSeviye)
            OyunHizi = Math.Min(OyunHizi + SeviyeBasinaHizArtisi, MaksimumHiz);

        if (Skor > EnYuksekSkor)
            EnYuksekSkor = Skor;

        SkorDegisti?.Invoke(this, EventArgs.Empty);
    }

    private void OyunuBitir()
    {
        Durum = OyunDurumu.Bitti;
        OyunBitti?.Invoke(this, EventArgs.Empty);
    }
}
