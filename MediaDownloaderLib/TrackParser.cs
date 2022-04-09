using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaDownloaderLib
{
    public static class TrackParser
    {
        public static IEnumerable<Track> GetTracks(string albumPage, string streamBaseUrl)
        {
            if (string.IsNullOrWhiteSpace(albumPage))
                throw new ArgumentNullException(nameof(albumPage));
            if (string.IsNullOrWhiteSpace(streamBaseUrl))
                throw new ArgumentNullException(nameof(streamBaseUrl));

            var trackStrings = albumPage
                .Split(streamBaseUrl)
                .Skip(1)
                .ToArray();
            
            if (!trackStrings.Any())
                throw new DownloaderException(ExceptionReason.NoTracksFoundOnPage);

            var tracks = trackStrings.Select(trackString =>
            {
                var trackName = GetTrackName(trackString);
                var trackNumber = GetTrackNumber(trackString);
                var trackUri = GetTrackUri(trackString, streamBaseUrl);
                return new Track(trackName, trackNumber, trackUri, TrackDownloadStatus.Pending);
            });

            return tracks.ToArray();
        }
        
        public static string GetTrackName(string trackString)
        {
            if (string.IsNullOrWhiteSpace(trackString))
                throw new ArgumentNullException(nameof(trackString));
            
            var encodedTrackName = trackString
                .Split("&quot;title&quot;:&quot;").Skip(1).FirstOrDefault()?
                .Split("&quot;").FirstOrDefault();
            
            if (string.IsNullOrWhiteSpace(encodedTrackName))
                throw new DownloaderException(ExceptionReason.CouldNotParseTrackName);
            
            return HttpUtility.HtmlDecode(encodedTrackName);
        }

        public static int GetTrackNumber(string trackString)
        {
            if (string.IsNullOrWhiteSpace(trackString))
                throw new ArgumentNullException(nameof(trackString));
            
            var trackNumberString = trackString
                .Split("track_num&quot;:").Skip(1).FirstOrDefault()?
                .Split(",&quot;").FirstOrDefault() ?? string.Empty;

            if (!int.TryParse(trackNumberString, out int trackNumber))
                throw new DownloaderException(ExceptionReason.CouldNotParseTrackNumber);

            return trackNumber;
        }

        public static Uri GetTrackUri(string trackString, string streamBaseUrl)
        {
            if (string.IsNullOrWhiteSpace(streamBaseUrl))
                throw new ArgumentNullException(nameof(streamBaseUrl));
            if (string.IsNullOrWhiteSpace(trackString))
                throw new ArgumentNullException(nameof(trackString));
            
            var trackUrl = trackString
                .Split("&quot;")
                .FirstOrDefault();
            
            if (string.IsNullOrWhiteSpace(trackUrl))
                throw new DownloaderException(ExceptionReason.CouldNotParseTrackUrl);
            
            return new Uri($"{streamBaseUrl}{trackUrl}");
        }
    }
}