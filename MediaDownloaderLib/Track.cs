using System;

namespace MediaDownloaderLib
{
    public class Track
    {
        public string TrackName { get; }
        public int TrackNumber { get; }
        public Uri TrackUri { get; }
        public TrackDownloadStatus TrackDownloadStatus { get; set; }
        
        public Track(
            Uri trackUri, 
            int trackNumber) : this(trackUri.OriginalString, trackNumber, trackUri, TrackDownloadStatus.Pending)
        {
        }

        public Track(
            string trackName, 
            int trackNumber, 
            Uri trackUri,
            TrackDownloadStatus trackDownloadStatus)
        {
            if (string.IsNullOrWhiteSpace(trackName))
                throw new ArgumentNullException(nameof(trackName));
            if (trackNumber <= 0)
                throw new ArgumentException("Must be greater than 0", nameof(trackNumber));
            if (trackUri == null)
                throw new ArgumentNullException(nameof(trackUri));
            if (trackDownloadStatus == TrackDownloadStatus.None)
                throw new ArgumentException($"{nameof(TrackDownloadStatus.None)} is not supported", nameof(trackDownloadStatus));

            TrackName = trackName;
            TrackNumber = trackNumber;
            TrackUri = trackUri;
            TrackDownloadStatus = trackDownloadStatus;
        }
    }
}