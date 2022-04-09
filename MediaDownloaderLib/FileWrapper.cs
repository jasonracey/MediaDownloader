using System;
using System.IO;

namespace MediaDownloaderLib
{
    public interface IFileWrapper
    {
        void Delete(string path);

        bool Exists(string path);
    }

    public class FileWrapper : IFileWrapper
    {
        public void Delete(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));
        
            File.Delete(path);
        }

        public bool Exists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));
        
            return File.Exists(path);
        }
    }
}