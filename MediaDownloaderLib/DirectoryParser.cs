using System;
using System.IO;

namespace MediaDownloaderLib
{
    public static class DirectoryParser
    {
        private const string DoubleSpace = "  ";
        private const string SingleSpace = " ";
        
        private static readonly char[] InvalidFileNameChars = Path.GetInvalidFileNameChars();
        private static readonly char[] InvalidPathChars = Path.GetInvalidPathChars();

        public static string GetDestinationDirectory(string? artist, string? album)
        {
            if (string.IsNullOrWhiteSpace(artist))
                throw new ArgumentNullException(nameof(artist));
            if (string.IsNullOrWhiteSpace(album))
                throw new ArgumentNullException(nameof(album));
            
            return $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/Downloads/{RemoveInvalidPathChars($"{artist} - {album}")}";
        }

        public static string GetDestinationFilePath(string? destinationDirectory, int trackNumber, string? trackName)
        {
            if (string.IsNullOrWhiteSpace(destinationDirectory))
                throw new ArgumentNullException(nameof(destinationDirectory));
            if (trackNumber <= 0)
                throw new ArgumentException("Must be greater than 0.", nameof(trackNumber));
            if (string.IsNullOrWhiteSpace(trackName))
                throw new ArgumentNullException(nameof(trackName));

            var trackNameClean = RemoveInvalidFileNameChars(trackName)
                .RepeatedlyReplace(DoubleSpace, SingleSpace)
                .Trim();
            
            return $"{RemoveInvalidPathChars(destinationDirectory)}/{trackNumber.ToString("D2")} {trackNameClean}.mp3";
        }

        public static string RemoveInvalidFileNameChars(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));
            
            return string.Join(string.Empty, fileName.Split(InvalidFileNameChars));    
        }
        
        public static string RemoveInvalidPathChars(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            return string.Join(string.Empty, path.Split(InvalidPathChars));
        }
    }
}