namespace ChatServer;

static class Program
{
    /// <summary>
    /// Sohbet sunucusu uygulamasının giriş noktası.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new SunucuFormu());
    }
}
