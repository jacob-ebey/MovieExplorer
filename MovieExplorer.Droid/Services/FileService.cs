using System;
using MovieExplorer.Services;
using System.IO;

namespace MovieExplorer.Droid.Services
{
    class FileService : IFileService
    {
        public void SaveText(string filename, string text)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            File.WriteAllText(filePath, text);
        }

        public string LoadText(string filename)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }

            return null;
        }
    }
}