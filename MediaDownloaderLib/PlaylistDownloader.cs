using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaDownloaderLib
{
    public class PlaylistDownloader : IProcessingStatusReporter
    {
        private readonly IDestinationPathBuilder _destinationPathBuilder;
        private readonly IHttpClientWrapper _httpClientWrapper;
        
        private readonly object _downloadCompletionLock = new object();

        public ProcessingStatus? ProcessingStatus { get; private set; }

        public PlaylistDownloader(
            IDestinationPathBuilder destinationPathBuilder,
            IHttpClientWrapper httpClientWrapper)
        {
            _destinationPathBuilder = destinationPathBuilder ?? throw new ArgumentNullException(nameof(destinationPathBuilder));
            _httpClientWrapper = httpClientWrapper ?? throw new ArgumentNullException(nameof(httpClientWrapper));
            SetState(0, 0, Enumerable.Empty<Track>());
        }

        public async Task DownloadFilesAsync(IEnumerable<Track> tracks)
        {
            if (tracks == null)
                throw new ArgumentNullException(nameof(tracks));

            var trackArray = tracks.ToArray();

            SetState(0, trackArray.Length, trackArray, "Downloading...");

            var attemptsCompleted = 0;
            await Task.WhenAll(trackArray.Select(async track =>
            {
                track.TrackDownloadStatus = TrackDownloadStatus.InProgress;
                
                var destinationPath = _destinationPathBuilder.CreateDestinationPath(track.TrackUri);
                
                await _httpClientWrapper.DownloadFileAsync(track.TrackUri, destinationPath).ConfigureAwait(false);

                track.TrackDownloadStatus = TrackDownloadStatus.Done;

                lock (_downloadCompletionLock)
                {
                    attemptsCompleted++;
                    SetState(
                        attemptsCompleted,
                        trackArray.Length, 
                        trackArray,
                        $"Downloaded {attemptsCompleted}/{trackArray.Length}");
                }
            }));
        }
        
        private void SetState(int countCompleted, int countTotal, IEnumerable<Track> tracks, string? message = null)
        {
            ProcessingStatus = new ProcessingStatus(countCompleted, countTotal, tracks, message);
        }
    }
}