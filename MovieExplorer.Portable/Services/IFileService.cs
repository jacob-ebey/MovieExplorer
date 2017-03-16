namespace MovieExplorer.Services
{
    public interface IFileService
    {
        void SaveText(string filename, string text);
        string LoadText(string filename);
    }
}
