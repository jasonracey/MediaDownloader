using System;
using System.Linq;
using System.Web;
using NUnit.Framework;

namespace MediaDownloaderLib.UnitTest
{
    [TestFixture]
    public class TrackParserTests
    {
        private const string MockAlbumPage = "MockAlbumPage";
        private const string MockTrackString = "some-track-name.mp3";
        private const string MockTrackName = "Wilma's Rainbow";

        private static readonly string EncodedTrackName = HttpUtility.HtmlEncode(MockTrackName);
        
        [TestCase(null, BandcampDownloader.StreamBaseUrl)]
        [TestCase("", BandcampDownloader.StreamBaseUrl)]
        [TestCase(" ", BandcampDownloader.StreamBaseUrl)]
        [TestCase(MockAlbumPage, null)]
        [TestCase(MockAlbumPage, "")]
        [TestCase(MockAlbumPage, " ")]
        public void GetTracks_ValidatesArgs(string albumPage, string streamBaseUrl)
        {
            Assert.Throws<ArgumentNullException>(() => TrackParser.GetTracks(albumPage, streamBaseUrl));
        }
        
        [Test]
        public void GetTracks_CanGetTracks()
        {
            // arrange
            var albumPage = System.IO.File.ReadAllText("Data/AlbumPage.html");
            
            // act
            var tracks = TrackParser
                .GetTracks(albumPage, BandcampDownloader.StreamBaseUrl)
                .ToArray();
            
            // assert
            Assert.IsNotNull(tracks);
            Assert.AreEqual(5, tracks.Length);
            foreach (var track in tracks)
            {
                Assert.IsNotNull(track);
                Assert.IsNotNull(track.TrackName);
                Assert.IsNotNull(track.TrackUri);
            }
        }
        
        [Test]
        public void GetTracks_NoTracksFound_Throws()
        {
            // arrange
            const string albumPage = "<html>wrong page</html>";
            
            // act
            var thrown = Assert.Throws<DownloaderException>(() => TrackParser.GetTracks(albumPage, BandcampDownloader.StreamBaseUrl));
            
            // assert
            Assert.IsNotNull(thrown);
            Assert.AreEqual(ExceptionReason.NoTracksFoundOnPage, thrown?.Message);
        }
        
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GetTrackName_ValidatesArgs(string trackString)
        {
            Assert.Throws<ArgumentNullException>(() => TrackParser.GetTrackName(trackString));
        }
        
        [TestCase("&quot;title&quot;:&quot;", "&quot;")]
        [TestCase("&quot;title&quot;:&quot;", "&quot;some-other-data")]
        [TestCase("&quot;title&quot;:&quot;", "")]
        [TestCase("some-track-data&quot;title&quot;:&quot;", "&quot;")]
        [TestCase("some-track-data&quot;title&quot;:&quot;", "&quot;some-other-data")]
        [TestCase("some-track-data&quot;title&quot;:&quot;", "")]
        public void GetTrackName_CanGetTrackName(string beforeTrackName, string afterTrackName)
        {
            // arrange
            var trackString = $"{beforeTrackName}{EncodedTrackName}{afterTrackName}";
            
            // act
            var trackName = TrackParser.GetTrackName(trackString);
            
            // assert
            Assert.IsNotNull(trackName);
            Assert.AreEqual(MockTrackName, trackName);
        }
        
        [TestCase("", "&quot;")]
        [TestCase("", "&quot;some-other-data")]
        [TestCase("", "")]
        [TestCase("some-track-data", "&quot;")]
        [TestCase("some-track-data", "&quot;some-other-data")]
        [TestCase("some-track-data", "")]
        public void GetTrackName_CannotGetTrackName_Throws(string beforeTrackName, string afterTrackName)
        {
            // arrange
            var trackString = $"{beforeTrackName}{EncodedTrackName}{afterTrackName}";
            
            // act
            var thrown = Assert.Throws<DownloaderException>(() => TrackParser.GetTrackName(trackString));
            
            // assert
            Assert.IsNotNull(thrown);
            Assert.AreEqual(ExceptionReason.CouldNotParseTrackName, thrown?.Message);
        }
        
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GetTrackNumber_ValidatesArgs(string trackString)
        {
            Assert.Throws<ArgumentNullException>(() => TrackParser.GetTrackNumber(trackString));
        }
        
        [TestCase("track_num&quot;:", ",&quot;")]
        [TestCase("track_num&quot;:", ",&quot;some-other-data")]
        [TestCase("track_num&quot;:", "")]
        [TestCase("some-track-data;track_num&quot;:", ",&quot;")]
        [TestCase("some-track-data;track_num&quot;:", ",&quot;some-other-data")]
        [TestCase("some-track-data;track_num&quot;:", "")]
        public void GetTrackNumber_CanGetTrackNumber(string beforeTrackNumber, string afterTrackNumber)
        {
            // arrange
            var trackString = $"{beforeTrackNumber}3{afterTrackNumber}";
            
            // act
            var trackNumber = TrackParser.GetTrackNumber(trackString);
            
            // assert
            Assert.AreEqual(3, trackNumber);
        }
        
        [TestCase("", ",&quot;")]
        [TestCase("", ",&quot;some-other-data")]
        [TestCase("", "")]
        [TestCase("some-track-data", ",&quot;")]
        [TestCase("some-track-data", ",&quot;some-other-data")]
        [TestCase("some-track-data", "")]
        public void GetTrackNumber_CannotGetTrackNumber_Throws(string beforeTrackNumber, string afterTrackNumber)
        {
            // arrange
            var trackString = $"{beforeTrackNumber}7{afterTrackNumber}";
            
            // act
            var thrown = Assert.Throws<DownloaderException>(() => TrackParser.GetTrackNumber(trackString));
            
            // assert
            Assert.IsNotNull(thrown);
            Assert.AreEqual(ExceptionReason.CouldNotParseTrackNumber, thrown?.Message);
        }

        [TestCase(null, BandcampDownloader.StreamBaseUrl)]
        [TestCase("", BandcampDownloader.StreamBaseUrl)]
        [TestCase(" ", BandcampDownloader.StreamBaseUrl)]
        [TestCase(MockTrackString, null)]
        [TestCase(MockTrackString, "")]
        [TestCase(MockTrackString, " ")]
        public void GetTrackUri_ValidatesArgs(string trackString, string streamBaseUrl)
        {
            Assert.Throws<ArgumentNullException>(() => TrackParser.GetTrackUri(trackString, streamBaseUrl));
        }
        
        [TestCase("")]
        [TestCase("&quot;")]
        [TestCase("&quot;some-other-data")]
        public void GetTrackUri_CanGetTrackUri(string trailingMarkup)
        {
            // arrange
            var trackString = $"{MockTrackString}{trailingMarkup}";
            
            // act
            var trackUri = TrackParser.GetTrackUri(trackString, BandcampDownloader.StreamBaseUrl);
            
            // assert
            Assert.IsNotNull(trackUri);
            Assert.AreEqual($"{BandcampDownloader.StreamBaseUrl}{MockTrackString}", trackUri.OriginalString);
        }
        
        [TestCase("&quot;")]
        [TestCase("&quot;some-other-data")]
        public void GetTrackUri_CannotGetTrackUri_Throws(string trackString)
        {
            // act
            var thrown = Assert.Throws<DownloaderException>(() => TrackParser.GetTrackUri(trackString, BandcampDownloader.StreamBaseUrl));
            
            // assert
            Assert.IsNotNull(thrown);
            Assert.AreEqual(ExceptionReason.CouldNotParseTrackUrl, thrown?.Message);
        }
    }
}