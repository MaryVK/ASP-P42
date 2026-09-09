using ASP_P42.Services.Storage;

namespace ASP_P42.LocalStorage
{
    public class LocalStorageService : IStorageService
    {
        private readonly String[] allowedExtensions = [".jpg", ".png", ".jpeg", ".webp", ".pdf"];
        private readonly String localFolder = "LocalStorage";
        public byte[] Load(string filename)
        {
            return File.ReadAllBytes(
                Path.Combine(localFolder, filename)
            );
        }

        public string Save(IFormFile file)
        {
            // выполняем проверку на наличие и валидность данных
            if(file == null) throw new ArgumentNullException(
                nameof(file), "Data not received");  // первое - название параметра, 2 - message
            if (file.Length < 256) throw new ArgumentException("File too short");
            if (file.Length > 1e7) throw new ArgumentException("File too long");

            // определяем расширение файла, с него тип файла
            int dotPosition = file.FileName.LastIndexOf('.');
            if (dotPosition < 0) throw new ArgumentException("File must have extension");
            String ext = file.FileName[dotPosition..];
            if(!allowedExtensions.Contains(ext))
            {
                throw new ArgumentException("File type not allowed");
            }

            // генерируем новое имя для файла, расширение сохраняем
            String savedName = Guid.NewGuid() + ext;
            using FileStream stream = File.OpenWrite(
                Path.Combine(localFolder, savedName)
            );
            file.CopyTo(stream);
            return savedName;
        }
    }
}
