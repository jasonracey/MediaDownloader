using System;
using System.Linq;

namespace MediaDownloaderLib
{
    public static class AlbumAndArtistParser
    {
        public static (string Album, string Artist) GetAlbumAndArtist(string albumPage)
        {
            if (string.IsNullOrWhiteSpace(albumPage))
                throw new ArgumentNullException(nameof(albumPage));
            
            var items = albumPage
                .Split("<title>").Skip(1).FirstOrDefault()?
                .Split("</title>").FirstOrDefault()?
                .Split(" | ") ?? Array.Empty<string>();

            if (items.Length <= 1 || string.IsNullOrWhiteSpace(items[0]) || string.IsNullOrWhiteSpace(items[1]))
            {
                throw new DownloaderException(ExceptionReason.CouldNotParseAlbumAndArtist);
            }
        
            return (items[0], items[1]);
        }
    }
}