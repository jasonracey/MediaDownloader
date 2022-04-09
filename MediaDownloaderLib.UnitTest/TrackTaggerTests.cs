using System;
using NUnit.Framework;

namespace MediaDownloaderLib.UnitTest
{
    [TestFixture]
    public class TrackTaggerTests
    {
        private const string MockFilePath = "MockFilePath";
        private const string MockAlbumName = "MockAlbumName";
        private const string MockArtistName = "MockArtistName";
        private const string MockTrackName = "MockTrackName";
        private const int MockTrackNumber = 1;
        private const int MockTrackCount = 10;
    
        private readonly ITrackTagger _trackTagger;

        public TrackTaggerTests()
        {
            _trackTagger = new TrackTagger();
        }

        [Test]
        public void ValidatesArgs()
        {
            Assert.Throws<ArgumentNullException>(() => _trackTagger.TagTrack(null, MockAlbumName, MockArtistName, MockTrackName, MockTrackNumber, MockTrackCount));
            Assert.Throws<ArgumentNullException>(() => _trackTagger.TagTrack(MockFilePath, null, MockArtistName, MockTrackName, MockTrackNumber, MockTrackCount));
            Assert.Throws<ArgumentNullException>(() => _trackTagger.TagTrack(MockFilePath, MockAlbumName, null, MockTrackName, MockTrackNumber, MockTrackCount));
            Assert.Throws<ArgumentNullException>(() => _trackTagger.TagTrack(MockFilePath, MockAlbumName, MockArtistName, null, MockTrackNumber, MockTrackCount));
            Assert.Throws<ArgumentException>(() => _trackTagger.TagTrack(MockFilePath, MockAlbumName, MockArtistName, MockTrackName, 0, MockTrackCount));
            Assert.Throws<ArgumentException>(() => _trackTagger.TagTrack(MockFilePath, MockAlbumName, MockArtistName, MockTrackName, MockTrackNumber, 0));
        }
    }
}