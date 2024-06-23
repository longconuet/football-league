namespace BetFootballLeague.WebUI.Helpers
{
    public static class Common
    {
        public static async Task<string?> UploadFile(IFormFile file, string sDirectory)
        {
            try
            {
                int timestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                string extension = Path.GetExtension(file.FileName);
                var fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{timestamp}{extension}";

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sDirectory);
                CreateIfMissing(path);
                string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sDirectory, fileName);

                var supportedTypes = new[] { "jpg", "jpeg", "png", "gif" };
                if (!supportedTypes.Contains(extension[1..].ToLower()))
                {
                    return null;
                }

                using (var stream = new FileStream(pathFile, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return $"images/{sDirectory}/{fileName}";
            }
            catch
            {
                return null;
            }
        }

        public static void CreateIfMissing(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }

        public static string GetImageSrc(string image, string sDirectory)
        {
            return !string.IsNullOrEmpty(image) ? $"/images/{sDirectory}/{image}" : "";
        }
    }
}
