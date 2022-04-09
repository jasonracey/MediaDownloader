using System;
using MediaDownloaderLib;

namespace MediaDownloaderUI.Data
{
    public class TrackModel
    {
        public int Number { get; }
        public int Count { get; }
        public string Title { get; }
        public string TrackDownloadStatus { get; }

        public TrackModel(
            int number,
            int count,
            string title,
            TrackDownloadStatus trackDownloadStatus)
        {
            Number = number;
            Count = count;
            Title = title;
            TrackDownloadStatus = Enum.GetName(typeof(TrackDownloadStatus), trackDownloadStatus);
        }
    }
}