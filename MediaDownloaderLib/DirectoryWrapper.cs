using System;
using System.IO;

namespace MediaDownloaderLib
{
    public interface IDirectoryWrapper
    {
        DirectoryInfo CreateDirectory(string path);
    }

    public class DirectoryWrapper : IDirectoryWrapper
    {
        public DirectoryInfo CreateDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));
        
            return Directory.CreateDirectory(path);
        }
    }
}