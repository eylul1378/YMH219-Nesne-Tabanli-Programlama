using OpenCvSharp;

namespace FaceRecognition.Services;

/// <summary>
/// VideoCapture ile kameradan arka planda kare okur ve her kareyi olay (event) ile bildirir.
/// </summary>
public sealed class Kamera : IDisposable
{
    private VideoCapture? _yakalayici;
    private CancellationTokenSource? _iptal;
    private Task? _dongu;

    public bool Calisiyor => _dongu is { IsCompleted: false };

    /// <summary>Yeni kare geldiğinde tetiklenir (arka plan iş parçacığında). Mat'ı olayı alan dispose etmez.</summary>
    public event EventHandler<Mat>? KareAlindi;

    /// <summary>Kamera beklenmedik şekilde durduğunda tetiklenir.</summary>
    public event EventHandler<string>? HataOlustu;

    /// <summary>Kamerayı açar. Açılamazsa açıklayıcı bir InvalidOperationException fırlatır.</summary>
    public async Task BaslatAsync(int kameraNo)
    {
        if (Calisiyor) return;

        var yakalayici = await Task.Run(() =>
        {
            var v = new VideoCapture(kameraNo, VideoCaptureAPIs.DSHOW);
            if (!v.IsOpened())
            {
                v.Dispose();
                v = new VideoCapture(kameraNo); // farklı arka uç ile tekrar dene
            }
            return v;
        });

        if (!yakalayici.IsOpened())
        {
            yakalayici.Dispose();
            throw new InvalidOperationException(
                $"Kamera ({kameraNo}) açılamadı. Kameranın bağlı olduğundan, başka bir uygulama tarafından " +
                "kullanılmadığından ve Windows gizlilik ayarlarında kamera erişimine izin verildiğinden emin olun.");
        }

        yakalayici.Set(VideoCaptureProperties.FrameWidth, 640);
        yakalayici.Set(VideoCaptureProperties.FrameHeight, 480);

        _yakalayici = yakalayici;
        _iptal = new CancellationTokenSource();
        var token = _iptal.Token;
        _dongu = Task.Run(() => KareDongusu(yakalayici, token));
    }

    private void KareDongusu(VideoCapture yakalayici, CancellationToken token)
    {
        int bosKare = 0;
        using var kare = new Mat();
        while (!token.IsCancellationRequested)
        {
            if (!yakalayici.Read(kare) || kare.Empty())
            {
                if (++bosKare > 50)
                {
                    HataOlustu?.Invoke(this, "Kameradan görüntü alınamıyor; kamera bağlantısı kesilmiş olabilir.");
                    return;
                }
                Thread.Sleep(30);
                continue;
            }
            bosKare = 0;
            KareAlindi?.Invoke(this, kare);
        }
    }

    public void Durdur()
    {
        _iptal?.Cancel();
        try { _dongu?.Wait(1000); } catch (AggregateException) { }
        _yakalayici?.Release();
        _yakalayici?.Dispose();
        _yakalayici = null;
        _iptal?.Dispose();
        _iptal = null;
        _dongu = null;
    }

    public void Dispose() => Durdur();
}
