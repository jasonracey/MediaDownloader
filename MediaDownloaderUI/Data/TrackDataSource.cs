using System;
using System.Collections.Generic;
using AppKit;

namespace MediaDownloaderUI.Data
{
    public class TrackDataSource : NSTableViewDataSource
    {
        public readonly List<TrackModel> Tracks = new List<TrackModel>();

        public override nint GetRowCount(NSTableView tableView)
        {
            return Tracks.Count;
        }
    }
}