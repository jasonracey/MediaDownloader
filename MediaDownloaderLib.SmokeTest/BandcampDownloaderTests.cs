using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MediaDownloaderLib.SmokeTest
{
    public class BandcampDownloaderTests
    {
        private readonly BandcampDownloader _downloader = new(
            new ResourceService(),
            new TrackTagger());
        
        private static readonly Uri AlbumUri = new("https://planetcruiser.bandcamp.com/album/riders-of-the-edge");
        private static readonly string UserProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        private static readonly string DestinationPath = $"{UserProfile}/Downloads/Planet Cruiser - Riders Of The Edge";

        private static void RemoveTestDirectory()
        {
            if (Directory.Exists(DestinationPath))
                Directory.Delete(DestinationPath, recursive: true);
        }
        
        [SetUp]
        public void SetUp()
        {
            RemoveTestDirectory();
        }
        
        [TearDown]
        public void TearDown()
        {
            RemoveTestDirectory();
        }

        private static readonly List<string> TrackNames = new()
        {
            "Emperor's Curse",
            "The Moon Mountain",
            "The Windmill Grave",
            "Sleeping Giants",
            "Broken Sail"
        };

        [Test]
        public async Task CanDownloadTracksAsync()
        {
            // act
            await _downloader.DownloadTracksAsync(AlbumUri);
            
            // assert
            Assert.IsTrue(Directory.Exists(DestinationPath));

            var trackNumber = 0;
            foreach (var trackName in TrackNames)
            {
                var filePath = $"{DestinationPath}/0{++trackNumber} {trackName}.mp3";
                Assert.IsTrue(File.Exists(filePath));
                
                var file = TagLib.File.Create(filePath);
                Assert.IsNotNull(file);
                
                Assert.AreEqual(new []{"Planet Cruiser"}, file.Tag.AlbumArtists);
                Assert.AreEqual(new []{"Planet Cruiser"}, file.Tag.Composers);
                Assert.AreEqual(1, file.Tag.Disc);
                Assert.AreEqual(1, file.Tag.DiscCount);
                Assert.AreEqual(new []{"Planet Cruiser"}, file.Tag.Performers);
                Assert.AreEqual(trackName, file.Tag.Title);
                Assert.AreEqual(trackNumber, file.Tag.Track);
                Assert.AreEqual(5, file.Tag.TrackCount);
            }
        }
    }
}