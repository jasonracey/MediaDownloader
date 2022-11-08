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
        
        [TestCase("<html></html>")]
        [TestCase("<html><title></title></html>")]
        public void GetAlbumAndArtist_CannotGetAlbum_ReturnsUnknown(string albumPage)
        {
            // act
            var result = AlbumAndArtistParser.GetAlbumAndArtist(albumPage);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Unknown", result.Album);
        }

        [TestCase("<html></html>")]
        [TestCase("<html><title></title></html>")]
        [TestCase("<html><title>Album</title></html>")]
        [TestCase("<html><title>Album | </title></html>")]
        public void GetAlbumAndArtist_CannotGetArtist_ReturnsUnknown(string albumPage)
        {
            // act
            var result = AlbumAndArtistParser.GetAlbumAndArtist(albumPage);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Unknown", result.Artist);
        }
    }
}