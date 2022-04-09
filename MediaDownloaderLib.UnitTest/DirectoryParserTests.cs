using System;
using System.IO;
using NUnit.Framework;

namespace MediaDownloaderLib.UnitTest
{
    [TestFixture]
    public class DirectoryParserTests
    {
        private const string MockAlbum = "MockAlbum";
        private const string MockArtist = "MockArtist";
        private const string MockDestinationDirectory = "/Home/Downloads/Foo - Bar/";
        private const int MockTrackNumber = 3;
        private const string MockTrackName = "  MockTrack / Name ";
        
        private static readonly char[] InvalidPathChars = Path.GetInvalidPathChars();
        private static readonly string InvalidPathCharsString = new(InvalidPathChars);
        private static readonly string HomePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        [Test]
        public void GetDestinationDirectory_ValidatesArgs()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryParser.GetDestinationDirectory(null, MockAlbum));
            Assert.Throws<ArgumentNullException>(() => DirectoryParser.GetDestinationDirectory(MockArtist, null));
        }

        [Test]
        public void GetDestinationDirectory_CanGetDestinationDirectory_RemovesInvalidChars()
        {
            // arrange
            var mockArtistWithInvalidChars = $"{InvalidPathCharsString}{MockArtist}{InvalidPathCharsString}";
            var mockAlbumWithInvalidChars = $"{InvalidPathCharsString}{MockAlbum}{InvalidPathCharsString}";
            
            // act
            var result = DirectoryParser.GetDestinationDirectory(mockArtistWithInvalidChars, mockAlbumWithInvalidChars);
            
            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual($"{HomePath}/Downloads/{MockArtist} - {MockAlbum}", result);
        }
        
        [Test]
        public void GetDestinationFilePath_ValidatesArgs()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryParser.GetDestinationFilePath(null, MockTrackNumber, MockTrackName));
            Assert.Throws<ArgumentException>(() => DirectoryParser.GetDestinationFilePath(MockDestinationDirectory, 0, MockTrackName));
            Assert.Throws<ArgumentNullException>(() => DirectoryParser.GetDestinationFilePath(MockDestinationDirectory, MockTrackNumber, null));
        }

        [TestCase("MockTrackName", "MockTrackName")]
        [TestCase("MockTrack/Name", "MockTrackName")]
        [TestCase("MockTrack/ Name", "MockTrack Name")]
        [TestCase("MockTrack /Name", "MockTrack Name")]
        [TestCase("MockTrack / Name", "MockTrack Name")]
        [TestCase("   MockTrack / Name", "MockTrack Name")]
        [TestCase("MockTrack / Name   ", "MockTrack Name")]
        [TestCase("   MockTrack / Name  ", "MockTrack Name")]
        public void GetDestinationFilePath_CanGetDestinationFilePath_CleansTrackName(string inputTrackName, string expectedOutputTrackName)
        {
            // arrange
            var mockDestinationDirectoryWithInvalidChars = $"{InvalidPathCharsString}{MockDestinationDirectory}{InvalidPathCharsString}";
            
            // act
            var result = DirectoryParser.GetDestinationFilePath(mockDestinationDirectoryWithInvalidChars, MockTrackNumber, inputTrackName);
            
            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual($"{MockDestinationDirectory}/{MockTrackNumber:D2} {expectedOutputTrackName}.mp3", result);
        }
        
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void RemoveInvalidFileNameChars_ValidatesArgs(string fileName)
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryParser.RemoveInvalidFileNameChars(fileName));
        }

        [Test]
        public void RemoveInvalidFileNameChars_CanRemoveInvalidPathChars()
        {
            // arrange
            const string fileNameWithInvalidChars = "VI / Outro";
            
            // act
            var result = DirectoryParser.RemoveInvalidFileNameChars(fileNameWithInvalidChars);
            
            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("VI  Outro", result);
        }
        
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void RemoveInvalidPathChars_ValidatesArgs(string path)
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryParser.RemoveInvalidPathChars(path));
        }

        [Test]
        public void RemoveInvalidPathChars_CanRemoveInvalidPathChars()
        {
            // arrange
            const string mockPath = "/MockPath";
            var pathWithInvalidChars = $"{InvalidPathCharsString}{mockPath}{InvalidPathCharsString}";
            
            // act
            var result = DirectoryParser.RemoveInvalidPathChars(pathWithInvalidChars);
            
            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mockPath, result);
        }
    }
}