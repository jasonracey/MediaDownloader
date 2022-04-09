using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace MediaDownloaderLib.UnitTest
{
    [TestFixture]
    public class PlaylistDownloaderTests
    {
        private Mock<IDestinationPathBuilder> _mockDestinationPathBuilder = null!;
        private Mock<IHttpClientWrapper> _mockHttpClientWrapper = null!;

        private PlaylistDownloader _playlistDownloader = null!;

        [SetUp]
        public void TestInitialize()
        {
            _mockDestinationPathBuilder = new Mock<IDestinationPathBuilder>();
            _mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
            _playlistDownloader = new PlaylistDownloader(_mockDestinationPathBuilder.Object, _mockHttpClientWrapper.Object);
        }

        [Test]
        public void WhenConstructorArgNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new PlaylistDownloader(null!, _mockHttpClientWrapper.Object);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new PlaylistDownloader(_mockDestinationPathBuilder.Object, null!);
            });
            Assert.IsNotNull(new PlaylistDownloader(_mockDestinationPathBuilder.Object, _mockHttpClientWrapper.Object));
        }

        [Test]
        public void WhenUrisNull_Throws()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _playlistDownloader.DownloadFilesAsync(null!));
        }

        [Test]
        public async Task WhenUrisEmpty_Skipped()
        {
            // act
            await _playlistDownloader.DownloadFilesAsync(new List<Track>());

            // assert
            Assert.AreEqual("Downloading...", _playlistDownloader.ProcessingStatus?.Message);
            Assert.AreEqual(0, _playlistDownloader.ProcessingStatus?.CountTotal);
            Assert.AreEqual(0, _playlistDownloader.ProcessingStatus?.CountCompleted);
        }

        [Test]
        public async Task WhenDownloadSucceeds_UpdatesCount()
        {
            // arrange
            var tracks = new []
            {
                new Track(new Uri("https://www.contoso.com/path/file1"), 1),
                new Track(new Uri("https://www.contoso.com/path/file2"), 2),
                new Track(new Uri("https://www.contoso.com/path/file3"), 3),
            };

            // act
            await _playlistDownloader.DownloadFilesAsync(tracks);

            // assert
            Assert.AreEqual("Downloaded 3/3", _playlistDownloader.ProcessingStatus?.Message);
            Assert.AreEqual(tracks.Length, _playlistDownloader.ProcessingStatus?.CountTotal);
            Assert.AreEqual(tracks.Length, _playlistDownloader.ProcessingStatus?.CountCompleted);
        }

        [Test]
        public async Task WhenDownloadStarts_ResetsCount()
        {
            // arrange
            var tracks = new []
            {
                new Track(new Uri("https://www.contoso.com/path/file1"), 1),
                new Track(new Uri("https://www.contoso.com/path/file2"), 2),
                new Track(new Uri("https://www.contoso.com/path/file3"), 3),
            };

            // act
            await _playlistDownloader.DownloadFilesAsync(tracks);

            // assert
            Assert.AreEqual(tracks.Length, _playlistDownloader.ProcessingStatus?.CountCompleted);
        }
    }
}