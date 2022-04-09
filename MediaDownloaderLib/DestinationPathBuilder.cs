using System;
using System.IO;

namespace MediaDownloaderLib
{
    public interface IDestinationPathBuilder
    {
        string CreateDestinationPath(Uri uri);
    }

    public class DestinationPathBuilder : IDestinationPathBuilder
    {
        private readonly IDirectoryWrapper _directoryWrapper;
        private readonly IFileWrapper _fileWrapper;

        public DestinationPathBuilder(
            IDirectoryWrapper directoryWrapper,
            IFileWrapper fileWrapper)
        {
            _directoryWrapper = directoryWrapper ?? throw new ArgumentNullException(nameof(directoryWrapper));
            _fileWrapper = fileWrapper ?? throw new ArgumentNullException(nameof(fileWrapper));
        }

        public string CreateDestinationPath(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var parts = uri.AbsolutePath.Split(Path.DirectorySeparatorChar);
            var directoryName = parts[^2];
            var fileName = parts[^1];

            var destinationDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", directoryName);
        
            // only creates dir if it doesn't already exist
            _directoryWrapper.CreateDirectory(destinationDirectory);

            var destinationFilePath = Path.Combine(destinationDirectory, fileName);

            if (_fileWrapper.Exists(destinationFilePath))
            {
                _fileWrapper.Delete(destinationFilePath);
            }

            return destinationFilePath;
        }
    }
}