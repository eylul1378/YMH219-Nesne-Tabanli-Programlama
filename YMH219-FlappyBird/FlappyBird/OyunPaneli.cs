namespace FlappyBird;

/// <summary>
/// Titremesiz (double buffered) çizim yapabilen oyun alanı paneli.
/// </summary>
public class OyunPaneli : Panel
{
    public OyunPaneli()
    {
        DoubleBuffered = true;
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                 ControlStyles.OptimizedDoubleBuffer, true);
        TabStop = false;
    }
}
