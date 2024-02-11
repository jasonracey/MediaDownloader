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
        
        private static readonly string UserProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        
        private static readonly Uri AlbumUri = new("https://planetcruiser.bandcamp.com/album/riders-of-the-edge");
        private static readonly string AlbumDestinationPath = $"{UserProfile}/Downloads/Planet Cruiser - Riders Of The Edge";

        private static readonly Uri TrackUri = new("https://planetcruiser.bandcamp.com/track/hollow-dancer");
        private static readonly string TrackDestinationPath = $"{UserProfile}/Downloads/Planet Cruiser - Hollow Dancer";
        
        private static void RemoveTestDirectory()
        {
            if (Directory.Exists(AlbumDestinationPath))
                Directory.Delete(AlbumDestinationPath, recursive: true);
            if (Directory.Exists(TrackDestinationPath))
                Directory.Delete(TrackDestinationPath, recursive: true);
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

        [Test]
        public async Task CanDownloadAlbumAsync()
        {
            // arrange
            string[] trackNames = 
            {
                "Emperor's Curse",
                "The Moon Mountain",
                "The Windmill Grave",
                "Sleeping Giants",
                "Broken Sail"
            };
            
            // act
            await _downloader.DownloadTracksAsync(AlbumUri);
            
            // assert
            AssertTracks(AlbumDestinationPath, trackNames);
        }
        
        [Test]
        public async Task CanDownloadTrackAsync()
        {
            // arrange
            string[] trackNames = 
            {
                "Hollow Dancer"
            };
            
            // act
            await _downloader.DownloadTracksAsync(TrackUri);
            
            // assert
            AssertTracks(TrackDestinationPath, trackNames);
        }

        private static void AssertTracks(
            string destinationPath,
            IReadOnlyCollection<string> trackNames)
        {
            Assert.IsTrue(Directory.Exists(destinationPath));

            var trackNumber = 0;
            foreach (var trackName in trackNames)
            {
                var filePath = $"{destinationPath}/0{++trackNumber} {trackName}.mp3";
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
                Assert.AreEqual(trackNames.Count, file.Tag.TrackCount);
            }   
        }
    }
}