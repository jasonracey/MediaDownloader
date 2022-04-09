// WARNING
//
// This file has been generated automatically by Rider IDE
//   to store outlets and actions made in Xcode.
// If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MediaDownloaderUI
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSButton ButtonAddPlaylist { get; set; }

		[Outlet]
		AppKit.NSButton ButtonClearBandcamp { get; set; }

		[Outlet]
		AppKit.NSButton ButtonClearPlaylist { get; set; }

		[Outlet]
		AppKit.NSButton ButtonDownloadBandcamp { get; set; }

		[Outlet]
		AppKit.NSButton ButtonDownloadPlaylist { get; set; }

		[Outlet]
		AppKit.NSTableColumn ColumnBandcampDownloadStatus { get; set; }

		[Outlet]
		AppKit.NSTableColumn ColumnBandcampTitle { get; set; }

		[Outlet]
		AppKit.NSTableColumn ColumnBandcampTrackNumber { get; set; }

		[Outlet]
		AppKit.NSTableColumn ColumnPlaylistDownloadStatus { get; set; }

		[Outlet]
		AppKit.NSTableColumn ColumnPlaylistTitle { get; set; }

		[Outlet]
		AppKit.NSTableColumn ColumnPlaylistTrackNumber { get; set; }

		[Outlet]
		AppKit.NSProgressIndicator Progress { get; set; }

		[Outlet]
		AppKit.NSTextField Status { get; set; }

		[Outlet]
		AppKit.NSTableView TableBandcamp { get; set; }

		[Outlet]
		AppKit.NSTableView TablePlaylist { get; set; }

		[Outlet]
		AppKit.NSTextField TextPlaylist { get; set; }

		[Outlet]
		AppKit.NSTextField TextUrl { get; set; }

		[Action ("AddPlaylistClicked:")]
		partial void AddPlaylistClicked (AppKit.NSButton sender);

		[Action ("AddPlaylistClicked:")]
		partial void AddPlaylistClicked (Foundation.NSObject sender);

		[Action ("ClearBandcampClicked:")]
		partial void ClearBandcampClicked (AppKit.NSButton sender);

		[Action ("ClearPlaylistClicked:")]
		partial void ClearPlaylistClicked (AppKit.NSButton sender);

		[Action ("DownloadBandcampClicked:")]
		partial void DownloadBandcampClicked (AppKit.NSButton sender);

		[Action ("DownloadPlaylistClicked:")]
		partial void DownloadPlaylistClicked (AppKit.NSButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (ButtonAddPlaylist != null) {
				ButtonAddPlaylist.Dispose ();
				ButtonAddPlaylist = null;
			}

			if (ButtonClearBandcamp != null) {
				ButtonClearBandcamp.Dispose ();
				ButtonClearBandcamp = null;
			}

			if (ButtonClearPlaylist != null) {
				ButtonClearPlaylist.Dispose ();
				ButtonClearPlaylist = null;
			}

			if (ButtonDownloadBandcamp != null) {
				ButtonDownloadBandcamp.Dispose ();
				ButtonDownloadBandcamp = null;
			}

			if (ButtonDownloadPlaylist != null) {
				ButtonDownloadPlaylist.Dispose ();
				ButtonDownloadPlaylist = null;
			}

			if (ColumnBandcampTrackNumber != null) {
				ColumnBandcampTrackNumber.Dispose ();
				ColumnBandcampTrackNumber = null;
			}

			if (ColumnBandcampTitle != null) {
				ColumnBandcampTitle.Dispose ();
				ColumnBandcampTitle = null;
			}

			if (ColumnBandcampDownloadStatus != null) {
				ColumnBandcampDownloadStatus.Dispose ();
				ColumnBandcampDownloadStatus = null;
			}

			if (Progress != null) {
				Progress.Dispose ();
				Progress = null;
			}

			if (Status != null) {
				Status.Dispose ();
				Status = null;
			}

			if (TableBandcamp != null) {
				TableBandcamp.Dispose ();
				TableBandcamp = null;
			}

			if (TablePlaylist != null) {
				TablePlaylist.Dispose ();
				TablePlaylist = null;
			}

			if (ColumnPlaylistTrackNumber != null) {
				ColumnPlaylistTrackNumber.Dispose ();
				ColumnPlaylistTrackNumber = null;
			}

			if (ColumnPlaylistTitle != null) {
				ColumnPlaylistTitle.Dispose ();
				ColumnPlaylistTitle = null;
			}

			if (ColumnPlaylistDownloadStatus != null) {
				ColumnPlaylistDownloadStatus.Dispose ();
				ColumnPlaylistDownloadStatus = null;
			}

			if (TextPlaylist != null) {
				TextPlaylist.Dispose ();
				TextPlaylist = null;
			}

			if (TextUrl != null) {
				TextUrl.Dispose ();
				TextUrl = null;
			}

		}
	}
}
