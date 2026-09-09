namespace ASP_P42.Services.Storage
{

    // создаём, чтобы переданный файл где-то в памяти сохранялся, а не пропадал
    public interface IStorageService
    {
        String Save(IFormFile file);

        byte[] Load(String filename);
    }
}
