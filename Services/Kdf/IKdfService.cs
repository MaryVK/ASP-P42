namespace ASP_P42.Services.Kdf
{
    // KDF key derivation function By PFC 28
    public interface IKdfService
    {
        String Dk(String password, String salt);
    }
}
