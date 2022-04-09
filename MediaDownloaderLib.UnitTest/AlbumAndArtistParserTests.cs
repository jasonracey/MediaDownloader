using System;
using NUnit.Framework;

namespace MediaDownloaderLib.UnitTest
{
    [TestFixture]
    public class AlbumAndArtistParserTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GetAlbumAndArtist_ValidatesArgs(string albumPage)
        {
            Assert.Throws<ArgumentNullException>(() => AlbumAndArtistParser.GetAlbumAndArtist(albumPage));
        }

        [Test]
        public void GetAlbumAndArtist_CanGetAlbumAndArtist()
        {
            // act
            const string mockAlbum = "Ozma";
            const string mockArtist = "Melvins";
            var (album, artist) =
                AlbumAndArtistParser.GetAlbumAndArtist($"<html><title>{mockAlbum} | {mockArtist}</title></html>");

            // assert
            Assert.IsNotNull(album);
            Assert.AreEqual(mockAlbum, album);
            Assert.IsNotNull(artist);
            Assert.AreEqual(mockArtist, artist);
        }

        [TestCase("<html>wrong page</html>")]
        [TestCase("<html><title>Album</html>")]
        [TestCase("<html><title>Album</title></html>")]
        [TestCase("<html><title>Album | </title></html>")]
        [TestCase("<html><title> | Artist</title></html>")]
        [TestCase("<html><title>Album - Artist</title></html>")]
        public void GetAlbumAndArtist_CannotGetAlbumAndArtist_Throws(string albumPage)
        {
            // act
            var thrown =
                Assert.Throws<DownloaderException>(() => AlbumAndArtistParser.GetAlbumAndArtist(albumPage));

            // assert
            Assert.IsNotNull(thrown);
            Assert.AreEqual(ExceptionReason.CouldNotParseAlbumAndArtist, thrown?.Message);
        }
    }
}