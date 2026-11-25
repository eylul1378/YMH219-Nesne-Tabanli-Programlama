namespace FlappyBird.Oyun;

/// <summary>
/// Ekranda konumu ve boyutu olan tüm oyun nesneleri için ortak taban sınıf.
/// </summary>
public abstract class OyunNesnesi
{
    public float X { get; protected set; }
    public float Y { get; protected set; }
    public float Genislik { get; protected set; }
    public float Yukseklik { get; protected set; }

    /// <summary>Çarpışma kontrolünde kullanılan dikdörtgen.</summary>
    public virtual RectangleF Sinirlar => new(X, Y, Genislik, Yukseklik);

    protected OyunNesnesi(float x, float y, float genislik, float yukseklik)
    {
        X = x;
        Y = y;
        Genislik = genislik;
        Yukseklik = yukseklik;
    }

    /// <summary>Nesnenin bir oyun karesindeki (tick) hareketini hesaplar.</summary>
    public abstract void Guncelle(float hiz);

    /// <summary>Nesneyi verilen Graphics üzerine çizer.</summary>
    public abstract void Ciz(Graphics g);
}
