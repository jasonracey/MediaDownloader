using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppKit;
using Foundation;
using MediaDownloaderLib;
using MediaDownloaderUI.Data;

namespace MediaDownloaderUI
{
    public partial class ViewController : NSViewController
    {
        private const int OpenFile = 1;
        
        private static readonly string[] PlaylistFileTypes = { "m3u" };
        
        private static readonly string DownloadsPath = Path.Combine(
            Environment.SpecialFolder.UserProfile.ToString(), 
            "Downloads");
        
        private static readonly NSOpenPanel FileOpenPanel = new NSOpenPanel
        {
            AllowedFileTypes = PlaylistFileTypes,
            AllowsMultipleSelection = false,
            CanChooseDirectories = false,
            CanChooseFiles = true,
            CanCreateDirectories = false,
            Directory = DownloadsPath,
            ReleasedWhenClosed = false,
            ShowsHiddenFiles = false,
            ShowsResizeIndicator = true
        };
        
        private static readonly BandcampDownloader BandcampDownloader = new BandcampDownloader(
            new ResourceService(),
            new TrackTagger());
        
        private static readonly PlaylistDownloader PlaylistDownloader = new PlaylistDownloader(
            new DestinationPathBuilder(
                new DirectoryWrapper(), 
                new FileWrapper()), 
            new HttpClientWrapper());

        private IProcessingStatusReporter _processingStatusReporter;
        private NSTimer _timer;
        private IEnumerable<Track> _tracks;
        
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Progress.Indeterminate = false;
            Status.StringValue = string.Empty;
            TextUrl.Changed += (sender, e) => BandcampUrlChanged();
            
            SetIdleState();
        }

        partial void ClearBandcampClicked(NSButton sender)
        {
            SetIdleState();
        }

        partial void ClearPlaylistClicked(NSButton sender)
        {
            SetIdleState();
        }
        
        async partial void DownloadBandcampClicked(NSButton sender)
        {
            _processingStatusReporter = BandcampDownloader;
            await ExecuteDownloadTaskAsync(BandcampDownloader.DownloadTracksAsync(new Uri(TextUrl.StringValue)));
        }
        
        async partial void DownloadPlaylistClicked(NSButton sender)
        {
            _processingStatusReporter = PlaylistDownloader;
            await ExecuteDownloadTaskAsync(PlaylistDownloader.DownloadFilesAsync(_tracks));
        }

        private async Task ExecuteDownloadTaskAsync(Task downloadTask)
        {
            SetBusyState();
            StartTimer();

            try
            {
                await downloadTask;
                SetIdleState();
            }
            catch (Exception e)
            {
                SetErrorState($"Error: {e.Message}");
            }
            finally
            {
                StopTimer();
            }
        }

        partial void AddPlaylistClicked(NSButton sender)
        {
            var result = FileOpenPanel.RunModal();
            if (result != OpenFile) return;

            var validUris = new List<Uri>();
            foreach (var line in File.ReadAllLines(FileOpenPanel.Url.Path))
            {
                if (Uri.TryCreate(line, UriKind.Absolute, out var uri))
                {
                    validUris.Add(uri);
                }
            }
            
            var trackNumbers = Enumerable.Range(1, validUris.Count);
            _tracks = validUris.Zip(trackNumbers, (uri, trackNumber) => new Track(
                uri,
                trackNumber));

            LoadTrackTableView(_tracks);

            SetPlaylistAddedState();
        }
        
        private void BandcampUrlChanged()
        {
            ButtonClearBandcamp.Enabled = BandcampUrlSpecified();
            ButtonDownloadBandcamp.Enabled = BandcampUrlValid();
        }

        private bool BandcampUrlSpecified()
        {
            return !string.IsNullOrWhiteSpace(TextUrl.StringValue);
        }

        private bool BandcampUrlValid()
        {
            return Uri.TryCreate(TextUrl.StringValue, UriKind.Absolute, out _);
        }

        private void SetPlaylistAddedState()
        {
            // playlist downloader
            ButtonClearPlaylist.Enabled = true;
            ButtonDownloadPlaylist.Enabled = true;
            ButtonAddPlaylist.Enabled = false;
            
            // status section
            Progress.DoubleValue = 0.0D;
            Progress.Hidden = true;
            Status.StringValue = string.Empty;
            Status.TextColor = NSColor.White;
        }
        
        private void SetBusyState()
        {
            // bandcamp downloader
            TextUrl.Editable = false;
            ButtonClearBandcamp.Enabled = false;
            ButtonDownloadBandcamp.Enabled = false;
            
            // playlist downloader
            ButtonClearPlaylist.Enabled = false;
            ButtonDownloadPlaylist.Enabled = false;
            ButtonAddPlaylist.Enabled = false;
            
            // status section
            Progress.DoubleValue = 0.0D;
            Progress.Hidden = false;
            Status.StringValue = string.Empty;
            Status.TextColor = NSColor.White;
        }

        private void SetErrorState(string message)
        {
            // bandcamp downloader
            TextUrl.Editable = false;
            ButtonClearBandcamp.Enabled = true;
            ButtonDownloadBandcamp.Enabled = false;
            
            // playlist downloader
            ButtonClearPlaylist.Enabled = true;
            ButtonDownloadPlaylist.Enabled = false;
            ButtonAddPlaylist.Enabled = false;
            
            // status section
            Progress.DoubleValue = 0.0D;
            Progress.Hidden = true;
            Status.StringValue = message;
            Status.TextColor = NSColor.Red;
        }

        private void SetIdleState()
        {
            // track table
            TableTracks.DataSource = new TrackDataSource();

            // bandcamp downloader
            TextUrl.Editable = true;
            TextUrl.StringValue = string.Empty;
            ButtonClearBandcamp.Enabled = false;
            ButtonDownloadBandcamp.Enabled = false;
            
            // playlist downloader
            ButtonClearPlaylist.Enabled = false;
            ButtonDownloadPlaylist.Enabled = false;
            ButtonAddPlaylist.Enabled = true;
            
            // status section
            Progress.DoubleValue = 0.0D;
            Progress.Hidden = true;
            Status.StringValue = string.Empty;
            Status.TextColor = NSColor.White;
        }

        private void StartTimer()
        {
            StopTimer();

            const double seconds = 0.25D;
            
            _timer = NSTimer.CreateRepeatingScheduledTimer(seconds, _ => {
                
                Progress.DoubleValue = _processingStatusReporter?
                    .ProcessingStatus?
                    .PercentCompleted ?? 0.0D;
                
                Status.StringValue = _processingStatusReporter?
                    .ProcessingStatus?
                    .Message ?? string.Empty;

                LoadTrackTableView(_processingStatusReporter?
                    .ProcessingStatus?
                    .Tracks ?? Enumerable.Empty<Track>());
            });
        }
        
        private void LoadTrackTableView(IEnumerable<Track> tracks)
        {
            var trackArray = tracks.ToArray();
            var dataSource = new TrackDataSource();
            foreach (var track in trackArray)
            {
                var trackModel = new TrackModel(
                    track.TrackNumber,
                    trackArray.Length,
                    track.TrackName,
                    track.TrackDownloadStatus);
                dataSource.Tracks.Add(trackModel);
            }
            TableTracks.DataSource = dataSource;
            TableTracks.Delegate = new TrackTableDelegate(dataSource);
        }

        private void StopTimer()
        {
            if (_timer == null) return;
            _timer.Invalidate();
            _timer.Dispose();
            _timer = null;
        }
    }
}