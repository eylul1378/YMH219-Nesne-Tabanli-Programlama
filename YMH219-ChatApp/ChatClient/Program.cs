namespace ChatClient;

static class Program
{
    /// <summary>
    /// Sohbet istemcisi uygulamasının giriş noktası.
    /// Komut satırından kullanıcı adı verilebilir: ChatClient.exe Ayse
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new IstemciFormu(args.FirstOrDefault()));
    }
}
