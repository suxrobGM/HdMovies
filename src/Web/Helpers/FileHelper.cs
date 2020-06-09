using System.IO;

namespace HdMovies.Helpers
{
    public static class FileHelper
    {
        public static async void SaveFileAsync(Stream stream, string filePath)
        {
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await stream.CopyToAsync(fileStream);
        }
    }
}
