using NAudio.Wave;

namespace SpeechApp.Services;

/// <summary>Bilgisayardaki mikrofon (kayıt) aygıtlarını listeler.</summary>
public static class Mikrofon
{
    public static List<string> Aygitlar()
    {
        var liste = new List<string>();
        try
        {
            for (int i = 0; i < WaveInEvent.DeviceCount; i++)
                liste.Add(WaveInEvent.GetCapabilities(i).ProductName);
        }
        catch (NAudio.MmException)
        {
            // Ses sürücüsü sorunu: boş liste döner, arayüz kullanıcıyı bilgilendirir.
        }
        return liste;
    }

    /// <summary>NAudio'nun kayıt hata kodlarını anlaşılır Türkçe mesaja çevirir.</summary>
    public static string HataAciklamasi(NAudio.MmException ex) => ex.Result switch
    {
        NAudio.MmResult.BadDeviceId or NAudio.MmResult.NoDriver =>
            "Mikrofon bulunamadı. Bir mikrofon bağlı olduğundan emin olun.",
        NAudio.MmResult.AlreadyAllocated =>
            "Mikrofon başka bir uygulama tarafından kullanılıyor.",
        _ => "Mikrofona erişilemedi. Windows Ayarlar > Gizlilik ve güvenlik > Mikrofon bölümünden " +
             "masaüstü uygulamalarının mikrofona erişimine izin verildiğini kontrol edin. (" + ex.Result + ")"
    };
}
