using System;
using System.Linq;
using AppKit;

namespace MediaDownloaderUI.Data
{
    public class TrackTableDelegate : NSTableViewDelegate
    {
        private const string CellIdentifier = "TrackCell";

        private readonly TrackDataSource _dataSource;

        public TrackTableDelegate (TrackDataSource dataSource)
        {
            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }

        public override NSView GetViewForItem(
            NSTableView tableView, 
            NSTableColumn tableColumn, 
            nint row)
        {
            // This pattern allows you reuse existing views when they are no-longer in
            // use. If the returned view is null, you create a new view. If a non-null
            // view is returned, you modify it enough to reflect the new data.
            var view = (NSTextField)tableView.MakeView(CellIdentifier, this);
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (view == null)
            {
                view = new NSTextField();
                view.Identifier = CellIdentifier;
                // ReSharper disable once HeuristicUnreachableCode
                view.BackgroundColor = NSColor.Clear;
                view.Bordered = false;
                view.Selectable = false;
                view.Editable = false;
            }

            switch (tableColumn.Title) 
            {
                case "Track #":
                    var trackNumber = _dataSource.Tracks[(int)row].Number;
                    var trackCount = _dataSource.Tracks[(int)row].Count;
                    view.StringValue = $"{trackNumber} of {trackCount}";
                    view.Alignment = NSTextAlignment.Right;
                    break;
                case "Title":
                    view.StringValue = _dataSource.Tracks[(int)row].Title?.Split("/")?.Last() ?? "(null)";
                    break;
                case "Download Status":
                    view.StringValue = _dataSource.Tracks[(int)row].TrackDownloadStatus ?? "(null)";
                    break;
            }

            return view;
        }
    }
}