// <copyright file="IFileService.cs">
//     Copyright (c) 2017 Jacob Ebey
// </copyright>

namespace MovieExplorer.Services
{
    public interface IFileService
    {
        void SaveText(string filename, string text);
        string LoadText(string filename);
    }
}
