using System;
using System.Linq;

namespace MediaDownloaderLib
{
    public static class AlbumAndArtistParser
    {
        private const string Unknown = "Unknown";
        
        public static (string Album, string Artist) GetAlbumAndArtist(string albumPage)
        {
            if (string.IsNullOrWhiteSpace(albumPage))
                throw new ArgumentNullException(nameof(albumPage));
            
            var items = albumPage
                .Split("<title>").Skip(1).FirstOrDefault()?
                .Split("</title>").FirstOrDefault()?
                .Split(" | ") ?? Array.Empty<string>();

            var album = items.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(album))
                album = Unknown;

            var artist = items.Skip(1).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(artist))
                artist = Unknown;
            
            return (album, artist);
        }
    }
}