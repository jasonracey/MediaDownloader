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
		AppKit.NSTableColumn ColumnDownloadStatus { get; set; }

		[Outlet]
		AppKit.NSTableColumn ColumnTitle { get; set; }

		[Outlet]
		AppKit.NSTableColumn ColumnTrackNumber { get; set; }

		[Outlet]
		AppKit.NSProgressIndicator Progress { get; set; }

		[Outlet]
		AppKit.NSTextField Status { get; set; }

		[Outlet]
		AppKit.NSTableView TableTracks { get; set; }

		[Outlet]
		AppKit.NSTextField TextUrl { get; set; }

		[Action ("AddPlaylistClicked:")]
		partial void AddPlaylistClicked (AppKit.NSButton sender);

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

			if (Progress != null) {
				Progress.Dispose ();
				Progress = null;
			}

			if (Status != null) {
				Status.Dispose ();
				Status = null;
			}

			if (TextUrl != null) {
				TextUrl.Dispose ();
				TextUrl = null;
			}

			if (TableTracks != null) {
				TableTracks.Dispose ();
				TableTracks = null;
			}

			if (ColumnTrackNumber != null) {
				ColumnTrackNumber.Dispose ();
				ColumnTrackNumber = null;
			}

			if (ColumnTitle != null) {
				ColumnTitle.Dispose ();
				ColumnTitle = null;
			}

			if (ColumnDownloadStatus != null) {
				ColumnDownloadStatus.Dispose ();
				ColumnDownloadStatus = null;
			}

		}
	}
}
