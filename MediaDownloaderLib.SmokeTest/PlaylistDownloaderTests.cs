using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MediaDownloaderLib.SmokeTest
{
    public class PlaylistDownloaderTests
    {
        private static readonly Uri RemoteFileUri = new ("http://archive.org/download/gd1976-06-03.123608.sbd.miller.flac24/gd76-06-03s2t01.mp3");
        private static readonly string TestDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "gd1976-06-03.123608.sbd.miller.flac24");
        private static readonly string TestFilePath = Path.Combine(TestDirectoryPath, "gd76-06-03s2t01.mp3");
    
        private PlaylistDownloader _playlistDownloader = null!;
    
        private static void RemoveTestDirectory()
        {
            if (Directory.Exists(TestDirectoryPath))
                Directory.Delete(TestDirectoryPath, true);
        }
    
        [SetUp]
        public void Setup()
        {
            RemoveTestDirectory();
        
            var directoryWrapper = new DirectoryWrapper();
            var fileWrapper = new FileWrapper();
            var destinationPathBuilder = new DestinationPathBuilder(directoryWrapper, fileWrapper);
            var httpClientWrapper = new HttpClientWrapper();
    
            _playlistDownloader = new PlaylistDownloader(
                destinationPathBuilder,
                httpClientWrapper);
        }

        [TearDown]
        public void TearDown()
        {
            RemoveTestDirectory();
        }

        [Test]
        public async Task CanDownloadFiles()
        {
            // arrange
            Assert.IsFalse(File.Exists(TestFilePath));
            var tracks = new [] { new Track(RemoteFileUri, 1) };
        
            // act
            await _playlistDownloader.DownloadFilesAsync(tracks);
        
            // assert
            Assert.IsTrue(File.Exists(TestFilePath));
        }
    }
}