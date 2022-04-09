using System;

namespace MediaDownloaderLib
{
    public interface ITrackTagger
    {
        void TagTrack(
            string? filePath,
            string? albumName,
            string? artistName,
            string? trackName,
            int trackNumber,
            int trackCount);
    }
    
    public class TrackTagger : ITrackTagger
    {
        public void TagTrack(
            string? filePath,
            string? albumName,
            string? artistName,
            string? trackName,
            int trackNumber,
            int trackCount)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));
            if (string.IsNullOrWhiteSpace(albumName))
                throw new ArgumentNullException(nameof(albumName));
            if (string.IsNullOrWhiteSpace(artistName))
                throw new ArgumentNullException(nameof(artistName));
            if (string.IsNullOrWhiteSpace(trackName))
                throw new ArgumentNullException(nameof(trackName));
            if (trackNumber <= 0)
                throw new ArgumentException("Must be greater than 0.", nameof(trackNumber));
            if (trackCount <= 0)
                throw new ArgumentException("Must be greater than 0.", nameof(trackCount));
            
            var track = TagLib.File.Create(filePath);
            
            track.Tag.Album = albumName;
            track.Tag.AlbumArtists = new[] { artistName };
            track.Tag.Composers = new[] { artistName };
            track.Tag.Disc = 1;
            track.Tag.DiscCount = 1;
            track.Tag.Performers = new[] { artistName };
            track.Tag.Title = trackName;
            track.Tag.Track = (uint)trackNumber;
            track.Tag.TrackCount = (uint)trackCount;
            
            track.Save();
        }
    }
}